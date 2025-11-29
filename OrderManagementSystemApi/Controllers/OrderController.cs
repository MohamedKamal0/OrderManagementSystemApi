using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Order;
using OrderManagementSystemApplication.Services.Abstract;

namespace OrderManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IOrderService _orderService) : ControllerBase
    {
        [HttpPost("CreateOrder")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<OrderResponseDto>>> CreateOrder([FromBody] OrderCreateDto orderDto)
        {
            var response = await _orderService.CreateOrderAsync(orderDto);

            return Ok(response);
        }
        [HttpGet("GetOrderById/{id}")]
        [EnableRateLimiting("DefaultPolicy")]
        public async Task<ActionResult<ApiResponse<OrderResponseDto>>> GetOrderById(int id)
        {
            var response = await _orderService.GetOrderByIdAsync(id);

            return Ok(response);
        }
        [HttpGet("GetOrdersByCustomer/{customerId}")]
        [EnableRateLimiting("DefaultPolicy")]
        public async Task<ActionResult<ApiResponse<List<OrderResponseDto>>>> GetOrdersByCustomer(int customerId)
        {
            var response = await _orderService.GetOrdersByCustomerAsync(customerId);

            return Ok(response);
        }
        [HttpPut("UpdateOrderStatus")]
        [Authorize]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> UpdateOrderStatus([FromBody] OrderStatusUpdateDto statusDto)
        {
            var response = await _orderService.UpdateOrderStatusAsync(statusDto);

            return Ok(response);
        }
    }
}
