using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.ShoppingCartDto;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class ShoppingService(IShoppingRepository _shoppingRepository,
        IProductRepository _productRepository,ICartItemsRepository _cartItemsRepository,IMapper _mapper) : IShoppingService
    {
        public async Task<ApiResponse<ShoppingResponseDto>> AddToCartAsync(AddShoppingDto addToCartDTO)
        {
            var transaction =  _shoppingRepository.BeginTransaction(); 

            try
            {
                
                var product = await _productRepository.GetByIdAsync(addToCartDTO.ProductId);
                if (product == null)
                {
                    
                    return new ApiResponse<ShoppingResponseDto>(404, "Product not found.");
                }
            
                if (addToCartDTO.Quantity > product.StockQuantity)
                {
                    return new ApiResponse<ShoppingResponseDto>(400, $"Only {product.StockQuantity} units of {product.Name} are available.");
                }
                var cart = await _shoppingRepository.GetActiveCartByCustomerAsync(addToCartDTO.CustomerId);
              
                if (cart == null)
                {
                    cart = new Cart
                    {
                        CustomerId = addToCartDTO.CustomerId,
                        IsCheckedOut = false,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow,
                        CartItems = new List<CartItem>()
                    };
                 await _shoppingRepository.AddAsync(cart);
                   await _shoppingRepository.SaveChangesAsync();
                }

                var existingCartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == addToCartDTO.ProductId);
                if (existingCartItem != null)
                {

                    if (existingCartItem.Quantity + addToCartDTO.Quantity > product.StockQuantity)
                    {
                        return new ApiResponse<ShoppingResponseDto>(400, $"Adding {addToCartDTO.Quantity} exceeds available stock.");
                    }
                
                    existingCartItem.Quantity += addToCartDTO.Quantity;
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
                        Quantity = addToCartDTO.Quantity,
                        UnitPrice = product.Price,
                        Discount = discount,
                        TotalPrice = (product.Price - discount) * addToCartDTO.Quantity,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _cartItemsRepository.AddAsync(cartItem);
                }
                cart.UpdatedAt = DateTime.UtcNow;
                await _shoppingRepository.UpdateAsync(cart);

                product.StockQuantity -= addToCartDTO.Quantity;
                await _productRepository.UpdateAsync(product);

                await _shoppingRepository.SaveChangesAsync();
                await transaction.CommitAsync();  

                cart = await _shoppingRepository.GetCartByIdAsync(cart.Id);
                var cartDTO = _mapper.Map<ShoppingResponseDto>(cart);

                return new ApiResponse<ShoppingResponseDto>(200, cartDTO);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ShoppingResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDto>> ClearCartAsync(int customerId)
        {
            try
            {
                var cart = await _shoppingRepository.GetCartByIdAsync(customerId);
                
                if (cart == null)
                {
                    return new ApiResponse<ConfirmationResponseDto>(404, "Active cart not found.");
                }
                if (cart.CartItems.Any())
                {
                   await _cartItemsRepository.DeleteRangeAsync(cart.CartItems);
                    cart.UpdatedAt = DateTime.UtcNow;
                    await _cartItemsRepository.SaveChangesAsync();
                }
                var confirmation = new ConfirmationResponseDto
                {
                    Message = "Cart has been cleared successfully."
                };
                return new ApiResponse<ConfirmationResponseDto>(200, confirmation);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ShoppingResponseDto>> GetCartByCustomerIdAsync(int customerId)
        {
            try
            {
                var cart = await _shoppingRepository.GetActiveCartByCustomerAsync(customerId);
              
                if (cart == null)
                {
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
                    return new ApiResponse<ShoppingResponseDto>(200, emptyCartDTO);
                }
                var cartDTO = _mapper.Map<ShoppingResponseDto>(cart);
                return new ApiResponse<ShoppingResponseDto>(200, cartDTO);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ShoppingResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ShoppingResponseDto>> RemoveCartItemAsync(RemoveShoppingItemDto removeCartItemDTO)
        {
            try
            {
                var cart = await _shoppingRepository.GetActiveCartByCustomerAsync(removeCartItemDTO.CustomerId);
                   
                if (cart == null)
                {
                    return new ApiResponse<ShoppingResponseDto>(404, "Active cart not found.");
                }
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == removeCartItemDTO.CartItemId);
                if (cartItem == null)
                {
                    return new ApiResponse<ShoppingResponseDto>(404, "Cart item not found.");
                }
             await   _cartItemsRepository.DeleteAsync(cartItem);
                cart.UpdatedAt = DateTime.UtcNow;
                await _cartItemsRepository.SaveChangesAsync();
                cart = await _shoppingRepository.GetCartByIdAsync(removeCartItemDTO.CartItemId);
            
                var cartDTO = _mapper.Map<ShoppingResponseDto>(cart);
                return new ApiResponse<ShoppingResponseDto>(200, cartDTO);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ShoppingResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public Task<ApiResponse<ShoppingResponseDto>> UpdateCartItemAsync(UpdateShopingItemDto updateCartItemDTO)
        {
            throw new NotImplementedException();
        }

    }
}
