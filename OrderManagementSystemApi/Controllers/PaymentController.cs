using Microsoft.AspNetCore.Mvc;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Payment;
using OrderManagementSystemApplication.Services.Abstract;

namespace OrderManagementSystemApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController(IPaymentService _payment) : ControllerBase
    {

        [HttpPost("ProcessPayment")]
        public async Task<ActionResult<ApiResponse<PaymentResponseDto>>> ProcessPayment([FromBody] PaymentRequestDto paymentRequest)
        {
            var response = await _payment.ProcessPaymentAsync(paymentRequest);

            return Ok(response);
        }
        [HttpGet("GetPaymentById/{paymentId}")]
        public async Task<ActionResult<ApiResponse<PaymentResponseDto>>> GetPaymentById(int paymentId)
        {
            var response = await _payment.GetPaymentByIdAsync(paymentId);

            return Ok(response);
        }
        [HttpGet("GetPaymentByOrderId/{orderId}")]
        public async Task<ActionResult<ApiResponse<PaymentResponseDto>>> GetPaymentByOrderId(int orderId)
        {
            var response = await _payment.GetPaymentByOrderIdAsync(orderId);

            return Ok(response);
        }
        [HttpPut("UpdatePaymentStatus")]
        public async Task<ActionResult<ApiResponse<ConfirmationResponseDto>>> UpdatePaymentStatus([FromBody] PaymentStatusUpdateDto statusUpdate)
        {
            var response = await _payment.UpdatePaymentStatusAsync(statusUpdate);

            return Ok(response);
        }
    }
}
