using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        IProductRepository _productRepository,ICartItemsRepository _cartItemsRepository) : IShoppingService
    {
        public async Task<ApiResponse<ShoppingResponseDto>> AddToCartAsync(AddShoppingDto addToCartDTO)
        {
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
                // Retrieve an active cart for the customer (include related CartItems and Products).
                var cart = await _shoppingRepository.GetActiveCartByCustomerAsync(addToCartDTO.CustomerId);
                //_context.Carts
                // .Include(c => c.CartItems)
                //.ThenInclude(ci => ci.Product)
                //.FirstOrDefaultAsync(c => c.CustomerId == addToCartDTO.CustomerId && !c.IsCheckedOut);
                // If no active cart exists, create a new cart.
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
                // Update the cart's last updated timestamp.
                cart.UpdatedAt = DateTime.UtcNow;
               await _shoppingRepository.UpdateAsync(cart);
                await _shoppingRepository.SaveChangesAsync();
                // Reload the cart with the latest details (including related CartItems and Products).
                cart = await _shoppingRepository.GetCartByIdAsync(cart.Id);
                // Map the cart entity to the DTO, which includes price calculations.
                var cartDTO = MapCartToDTO(cart);
                return new ApiResponse<ShoppingResponseDto>(200, cartDTO);
            }
            catch (Exception ex)
            {
                // Return error response in case of exceptions.
                return new ApiResponse<ShoppingResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDto>> ClearCartAsync(int customerId)
        {
            try
            {
                // Retrieve the active cart along with its items.
                var cart = await _shoppingRepository.GetCartByIdAsync(customerId);
                    //_context.Carts
                //.Include(c => c.CartItems)
                //.FirstOrDefaultAsync(c => c.CustomerId == customerId && !c.IsCheckedOut);
                // Return 404 if no active cart is found.
                if (cart == null)
                {
                    return new ApiResponse<ConfirmationResponseDto>(404, "Active cart not found.");
                }
                // If there are any items in the cart, remove them.
                if (cart.CartItems.Any())
                {
                   await _cartItemsRepository.DeleteRangeAsync(cart.CartItems);
                    cart.UpdatedAt = DateTime.UtcNow;
                    await _cartItemsRepository.SaveChangesAsync();
                }
                // Create a confirmation response DTO.
                var confirmation = new ConfirmationResponseDto
                {
                    Message = "Cart has been cleared successfully."
                };
                return new ApiResponse<ConfirmationResponseDto>(200, confirmation);
            }
            catch (Exception ex)
            {
                // Return error response if an exception occurs.
                return new ApiResponse<ConfirmationResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public async Task<ApiResponse<ShoppingResponseDto>> GetCartByCustomerIdAsync(int customerId)
        {
            try
            {
                // Query the database for a cart that belongs to the specified customer and is not checked out.
                var cart = await _shoppingRepository.GetActiveCartByCustomerAsync(customerId);
                //_context.Carts
                //.Include(c => c.CartItems) // Include the cart items in the query
                //.ThenInclude(ci => ci.Product) // Also include the product details for each cart item
                //.FirstOrDefaultAsync(c => c.CustomerId == customerId && !c.IsCheckedOut);
                // If no active cart is found, create an empty DTO with default values.
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
                    // Return the empty cart wrapped in an ApiResponse with status code 200 (OK).
                    return new ApiResponse<ShoppingResponseDto>(200, emptyCartDTO);
                }
                // Map the cart entity to its corresponding DTO (includes price calculations).
                var cartDTO = MapCartToDTO(cart);
                return new ApiResponse<ShoppingResponseDto>(200, cartDTO);
            }
            catch (Exception ex)
            {
                // In case of an exception, return a 500 status code with an error message.
                return new ApiResponse<ShoppingResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ShoppingResponseDto>> RemoveCartItemAsync(RemoveShoppingItemDto removeCartItemDTO)
        {
            try
            {
                // Retrieve the active cart along with its items and product details.
                var cart = await _shoppingRepository.GetActiveCartByCustomerAsync(removeCartItemDTO.CustomerId);
                    //_context.Carts
                //.Include(c => c.CartItems)
                //.ThenInclude(ci => ci.Product)
                //.FirstOrDefaultAsync(c => c.CustomerId == removeCartItemDTO.CustomerId && !c.IsCheckedOut);
                // Return 404 if no active cart is found.
                if (cart == null)
                {
                    return new ApiResponse<ShoppingResponseDto>(404, "Active cart not found.");
                }
                // Find the cart item to remove.
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.Id == removeCartItemDTO.CartItemId);
                if (cartItem == null)
                {
                    return new ApiResponse<ShoppingResponseDto>(404, "Cart item not found.");
                }
                // Remove the cart item from the context.
             await   _cartItemsRepository.DeleteAsync(cartItem);
                cart.UpdatedAt = DateTime.UtcNow;
                await _cartItemsRepository.SaveChangesAsync();
                // Reload the updated cart after removal.
                cart = await _shoppingRepository.GetCartByIdAsync(removeCartItemDTO.CartItemId);
                    //_context.Carts
                //.Include(c => c.CartItems)
                //.ThenInclude(ci => ci.Product)
                //.FirstOrDefaultAsync(c => c.Id == cart.Id) ?? new Cart();
                // Map the updated cart to the DTO.
                var cartDTO = MapCartToDTO(cart);
                return new ApiResponse<ShoppingResponseDto>(200, cartDTO);
            }
            catch (Exception ex)
            {
                // Return error response if an exception occurs.
                return new ApiResponse<ShoppingResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }

        public Task<ApiResponse<ShoppingResponseDto>> UpdateCartItemAsync(UpdateShopingItemDto updateCartItemDTO)
        {
            throw new NotImplementedException();
        }

        private ShoppingResponseDto MapCartToDTO(Cart cart)
        {
            // Map each CartItem entity to its corresponding CartItemResponseDTO.a
            var cartItemsDto = cart.CartItems?.Select(ci => new ShoppingItemResponseDto
            {
                Id = ci.Id,
                ProductId = ci.ProductId,
                ProductName = ci.Product?.Name,
                Quantity = ci.Quantity,
                UnitPrice = ci.UnitPrice,
                Discount = ci.Discount,
                TotalPrice = ci.TotalPrice
            }).ToList() ?? new List<ShoppingItemResponseDto>();
            // Initialize totals for base price, discount, and amount after discount.
            decimal totalBasePrice = 0;
            decimal totalDiscount = 0;
            decimal totalAmount = 0;
            // Iterate through each cart item DTO to accumulate the totals.
            foreach (var item in cartItemsDto)
            {
                totalBasePrice += item.UnitPrice * item.Quantity;       // Sum of base prices (without discount)
                totalDiscount += item.Discount * item.Quantity;         // Sum of discounts applied per unit
                totalAmount += item.TotalPrice;                         // Sum of final prices after discount
            }
            // Create and return the final CartResponseDTO with all details and calculated totals.
            return new ShoppingResponseDto
            {
                Id = cart.Id,
                CustomerId = cart.CustomerId,
                IsCheckedOut = cart.IsCheckedOut,
                CreatedAt = cart.CreatedAt,
                UpdatedAt = cart.UpdatedAt,
                CartItems = cartItemsDto,
                TotalBasePrice = totalBasePrice,
                TotalDiscount = totalDiscount,
                TotalAmount = totalAmount
            };
        }
    }
}
