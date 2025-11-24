using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.ShoppingCartDto;
using OrderManagementSystemApplication.Services.Abstract;

namespace OrderManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingController(IShoppingService _shoppingService) : ControllerBase
    {
        [HttpGet("GetCart/{customerId}")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ShoppingResponseDto>>> GetCartByCustomerId(int customerId)
        {
            var response = await _shoppingService.GetCartByCustomerIdAsync(customerId);

            return Ok(response);
        }
        [HttpPost("AddToCart")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ShoppingResponseDto>>> AddToCart([FromBody] AddShoppingDto addToCartDTO)
        {
            var response = await _shoppingService.AddToCartAsync(addToCartDTO);

            return Ok(response);
        }
        [HttpDelete("RemoveCartItem")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ShoppingResponseDto>>> RemoveCartItem([FromBody] RemoveShoppingItemDto removeCartItemDTO)
        {
            var response = await _shoppingService.RemoveCartItemAsync(removeCartItemDTO);

            return Ok(response);
        }
        [HttpDelete("ClearCart")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> ClearCart([FromQuery] int customerId)
        {
            var response = await _shoppingService.ClearCartAsync(customerId);

            return Ok(response);
        }
    }
}
