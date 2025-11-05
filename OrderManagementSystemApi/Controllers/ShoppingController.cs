using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<ApiResponse<ShoppingResponseDto>>> GetCartByCustomerId(int customerId)
        {
            var response = await _shoppingService.GetCartByCustomerIdAsync(customerId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Adds an item to the customer's cart.
        [HttpPost("AddToCart")]
        public async Task<ActionResult<ApiResponse<ShoppingResponseDto>>> AddToCart([FromBody] AddShoppingDto addToCartDTO)
        {
            var response = await _shoppingService.AddToCartAsync(addToCartDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        [HttpDelete("RemoveCartItem")]
        public async Task<ActionResult<ApiResponse<ShoppingResponseDto>>> RemoveCartItem([FromBody] RemoveShoppingItemDto removeCartItemDTO)
        {
            var response = await _shoppingService.RemoveCartItemAsync(removeCartItemDTO);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
        // Clears all items from the customer's active cart.
        [HttpDelete("ClearCart")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> ClearCart([FromQuery] int customerId)
        {
            var response = await _shoppingService.ClearCartAsync(customerId);
            if (response.StatusCode != 200)
            {
                return StatusCode(response.StatusCode, response);
            }
            return Ok(response);
        }
    }
}
