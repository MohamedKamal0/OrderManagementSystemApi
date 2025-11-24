using AutoMapper;
using Microsoft.Extensions.Logging;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.ShoppingCartDto;
using OrderManagementSystemApplication.Helpers;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class ShoppingService(IUnitOfWork _unitOfWork,
         ICartItemsRepository _cartItemsRepository,
        IMapper _mapper, ResponseHandler _responseHandler, ILogger<ShoppingService> _logger) : IShoppingService
    {
        public async Task<ApiResponse<string>> AddToCartAsync(AddShoppingDto addToCartDto)
        {
            var transaction = _unitOfWork.Carts.BeginTransaction();

            try
            {
                var product = await _unitOfWork.Products.GetByIdAsync(addToCartDto.ProductId);
                if (product == null)
                {
                    _logger.LogWarning(ShoppingCartLogMessages.ProductNotFound, addToCartDto.ProductId);
                    return _responseHandler.NotFound<string>("Product not found.");
                }

                if (addToCartDto.Quantity > product.StockQuantity)
                {
                    _logger.LogWarning(ShoppingCartLogMessages.InsufficientStock,
                            addToCartDto.ProductId, addToCartDto.Quantity, product.StockQuantity);
                    return _responseHandler.BadRequest<string>($"Only {product.StockQuantity} units of {product.Name} are available.");
                }
                var cart = await _unitOfWork.Carts.GetActiveCartByCustomerAsync(addToCartDto.CustomerId);

                if (cart == null)
                {
                    cart = new Cart
                    {
                        CustomerId = addToCartDto.CustomerId,
                        IsCheckedOut = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CartItems = new List<CartItem>()
                    };
                    await _unitOfWork.Carts.AddAsync(cart);
                    await _unitOfWork.Carts.SaveChangesAsync();
                }

                var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == addToCartDto.ProductId);
                if (existingCartItem != null)
                {

                    if (existingCartItem.Quantity + addToCartDto.Quantity > product.StockQuantity)
                    {
                        return _responseHandler.BadRequest<string>($"Adding {addToCartDto.Quantity} exceeds available stock.");
                    }

                    existingCartItem.Quantity += addToCartDto.Quantity;
                    existingCartItem.TotalPrice = (existingCartItem.UnitPrice - existingCartItem.Discount) * existingCartItem.Quantity;
                    existingCartItem.UpdatedAt = DateTime.UtcNow;

                    await _cartItemsRepository.UpdateAsync(existingCartItem);
                }
                else
                {
                    var discount = product.DiscountPercentage > 0 ? product.Price * product.DiscountPercentage / 100 : 0;

                    var cartItem = new CartItem
                    {
                        CartId = cart.Id,
                        ProductId = product.Id,
                        Quantity = addToCartDto.Quantity,
                        UnitPrice = product.Price,
                        Discount = discount,
                        TotalPrice = (product.Price - discount) * addToCartDto.Quantity,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _cartItemsRepository.AddAsync(cartItem);
                }
                cart.UpdatedAt = DateTime.UtcNow;
                await _unitOfWork.Carts.UpdateAsync(cart);

                product.StockQuantity -= addToCartDto.Quantity;
                await _unitOfWork.Products.UpdateAsync(product);

                await _unitOfWork.Carts.SaveChangesAsync();
                await transaction.CommitAsync();

                cart = await _unitOfWork.Carts.GetCartByIdAsync(cart.Id);
                var cartDTO = _mapper.Map<ShoppingResponseDto>(cart);
                _logger.LogInformation(ShoppingCartLogMessages.TransactionCommitted, addToCartDto.CustomerId);

                return _responseHandler.Created("Created Successfully.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ShoppingCartLogMessages.ErrorAddingToCart,
                    addToCartDto.CustomerId, addToCartDto.ProductId);
                _logger.LogInformation(ShoppingCartLogMessages.TransactionRolledBack, addToCartDto.CustomerId);

                return _responseHandler.InternalServerError<string>(
                    "An error occurred while adding product to cart.");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDto>> ClearCartAsync(int customerId)
        {
            try
            {
                var cart = await _unitOfWork.Carts.GetCartByIdAsync(customerId);

                if (cart == null)
                {
                    _logger.LogWarning(ShoppingCartLogMessages.CartNotFound, customerId);
                    return _responseHandler.NotFound<ConfirmationResponseDto>("Active cart not found.");
                }
                if (cart.CartItems.Any())
                {
                    await _cartItemsRepository.DeleteRangeAsync(cart.CartItems);
                    cart.UpdatedAt = DateTime.UtcNow;
                    await _cartItemsRepository.SaveChangesAsync();
                }
                _logger.LogInformation(ShoppingCartLogMessages.CartCleared, customerId);

                var confirmation = new ConfirmationResponseDto
                {
                    Message = "Cart has been cleared successfully."
                };
                return _responseHandler.Success(confirmation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ShoppingCartLogMessages.ErrorClearingCart, customerId);
                return _responseHandler.InternalServerError<ConfirmationResponseDto>(
                    "An error occurred while clearing the cart.");
            }
        }

        public async Task<ApiResponse<ShoppingResponseDto>> GetCartByCustomerIdAsync(int customerId)
        {
            try
            {
                var cart = await _unitOfWork.Carts.GetActiveCartByCustomerAsync(customerId);

                if (cart == null)
                {
                    _logger.LogInformation(ShoppingCartLogMessages.EmptyCartReturned, customerId);

                    var emptyCartDTO = new ShoppingResponseDto
                    {
                        CustomerId = customerId,
                        IsCheckedOut = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CartItems = new List<ShoppingItemResponseDto>(),
                        TotalBasePrice = 0,
                        TotalDiscount = 0,
                        TotalAmount = 0
                    };
                    return _responseHandler.Success(emptyCartDTO);
                }
                var cartDTO = _mapper.Map<ShoppingResponseDto>(cart);
                _logger.LogInformation(ShoppingCartLogMessages.CartRetrieved,
                       customerId, cart.CartItems.Count);
                return _responseHandler.Success(cartDTO);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cart for customer {CustomerId}", customerId);
                return _responseHandler.InternalServerError<ShoppingResponseDto>(
                    "An error occurred while retrieving the cart.");
            }
        }
        public async Task<ApiResponse<string>> RemoveCartItemAsync(RemoveShoppingItemDto removeCartItemDto)
        {
            try
            {
                var cart = await _unitOfWork.Carts.GetActiveCartByCustomerAsync(removeCartItemDto.CustomerId);

                if (cart == null)
                {
                    _logger.LogWarning(ShoppingCartLogMessages.CartNotFound, removeCartItemDto.CustomerId);
                    return _responseHandler.NotFound<string>("Active cart not found.");
                }
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == removeCartItemDto.CartItemId);
                if (cartItem == null)
                {
                    _logger.LogWarning(ShoppingCartLogMessages.CartItemNotFound, removeCartItemDto.CartItemId);

                    return _responseHandler.NotFound<string>("Cart item not found.");
                }
                await _cartItemsRepository.DeleteAsync(cartItem);
                cart.UpdatedAt = DateTime.UtcNow;
                await _cartItemsRepository.SaveChangesAsync();
                cart = await _unitOfWork.Carts.GetCartByIdAsync(removeCartItemDto.CartItemId);

                var cartDTO = _mapper.Map<ShoppingResponseDto>(cart);
                _logger.LogInformation(ShoppingCartLogMessages.CartItemRemoved,
                      removeCartItemDto.CartItemId, removeCartItemDto.CustomerId);

                return _responseHandler.Deleted<String>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ShoppingCartLogMessages.ErrorRemovingCartItem,
                    removeCartItemDto.CartItemId, removeCartItemDto.CustomerId);

                return _responseHandler.InternalServerError<string>(
                    "An error occurred while removing the cart item.");
            }
        }

        public Task<ApiResponse<string>> UpdateCartItemAsync(UpdateShopingItemDto updateCartItemDTO)
        {
            throw new NotImplementedException();
        }

    }
}
