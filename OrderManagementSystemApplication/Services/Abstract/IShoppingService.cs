using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.ShoppingCartDto;

namespace OrderManagementSystemApplication.Services.Abstract
{
    public interface IShoppingService
    {

        Task<ApiResponse<ShoppingResponseDto>> GetCartByCustomerIdAsync(int customerId);
        Task<ApiResponse<ShoppingResponseDto>> AddToCartAsync(AddShoppingDto addToCartDTO);
        Task<ApiResponse<ShoppingResponseDto>> UpdateCartItemAsync(UpdateShopingItemDto updateCartItemDTO);
        Task<ApiResponse<ShoppingResponseDto>> RemoveCartItemAsync(RemoveShoppingItemDto removeCartItemDTO);
        Task<ApiResponse<ConfirmationResponseDto>> ClearCartAsync(int customerId);
    }
}
