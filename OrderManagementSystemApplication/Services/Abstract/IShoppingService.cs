using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.ShoppingCartDto;

namespace OrderManagementSystemApplication.Services.Abstract
{
    public interface IShoppingService
    {

        Task<ApiResponse<ShoppingResponseDto>> GetCartByCustomerIdAsync(int customerId);
        Task<ApiResponse<string>> AddToCartAsync(AddShoppingDto addToCartDTO);
        Task<ApiResponse<string>> UpdateCartItemAsync(UpdateShopingItemDto updateCartItemDTO);
        Task<ApiResponse<string>> RemoveCartItemAsync(RemoveShoppingItemDto removeCartItemDTO);
        Task<ApiResponse<ConfirmationResponseDto>> ClearCartAsync(int customerId);
    }
}
