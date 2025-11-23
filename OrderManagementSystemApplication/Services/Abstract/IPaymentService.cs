using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Payment;

namespace OrderManagementSystemApplication.Services.Abstract
{
    public interface IPaymentService
    {
        Task<ApiResponse<PaymentResponseDto>> ProcessPaymentAsync(PaymentRequestDto paymentRequest);
        Task<ApiResponse<PaymentResponseDto>> GetPaymentByIdAsync(int paymentId);
        Task<ApiResponse<PaymentResponseDto>> GetPaymentByOrderIdAsync(int orderId);
        Task<ApiResponse<ConfirmationResponseDto>> UpdatePaymentStatusAsync(PaymentStatusUpdateDto statusUpdate);

    }
}
