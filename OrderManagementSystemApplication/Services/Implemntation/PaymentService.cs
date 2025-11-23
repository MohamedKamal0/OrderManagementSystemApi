using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Payment;
using OrderManagementSystemApplication.Helpers;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Enums;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class PaymentService(IPaymentRepository _paymentRepository,
        IMapper _mapper, IOrderRepository _orderRepository, ILogger<PaymentService> _logger, ResponseHandler _responseHandler) : IPaymentService
    {
        public async Task<ApiResponse<PaymentResponseDto>> ProcessPaymentAsync(PaymentRequestDto paymentRequest)
        {
            using (var transaction = _paymentRepository.BeginTransaction())
            {
                try
                {
                    var order = await _orderRepository.GetOrderWithPaymentAsync(paymentRequest.OrderId, paymentRequest.CustomerId);
                    if (order == null)
                    {
                        _logger.LogWarning(PaymentLogMessages.OrderNotFound, paymentRequest.OrderId, paymentRequest.CustomerId);

                        return _responseHandler.NotFound<PaymentResponseDto>("Order not found.");
                    }
                    if (Math.Round(paymentRequest.Amount, 2) != Math.Round(order.TotalAmount, 2))
                    {
                        _logger.LogWarning(PaymentLogMessages.AmountMismatch, paymentRequest.Amount, order.TotalAmount);

                        return _responseHandler.BadRequest<PaymentResponseDto>("Payment amount does not match the order total.");
                    }
                    Payment payment;
                    if (order.Payment != null)
                    {
                        // Allow retry only if previous payment failed and order status is still Pending
                        if (order.Payment.Status == PaymentStatus.Failed && order.OrderStatus == OrderStatus.Pending)
                        {
                            // Retry: update the existing payment record with new details
                            payment = order.Payment;
                            payment.PaymentMethod = paymentRequest.PaymentMethod;
                            payment.Amount = paymentRequest.Amount;
                            payment.PaymentDate = DateTime.UtcNow;
                            payment.Status = PaymentStatus.Pending;
                            payment.TransactionId = null; // Clear previous transaction id if any
                            await _paymentRepository.UpdateAsync(payment);
                        }
                        else
                        {
                            _logger.LogInformation(PaymentLogMessages.ExistingPaymentFound, order.Id, order.Payment.Status);

                            return _responseHandler.BadRequest<PaymentResponseDto>("Order already has an associated payment.");
                        }
                    }
                    else
                    {
                        // Create a new Payment record if none exists
                        payment = new Payment
                        {
                            OrderId = paymentRequest.OrderId,
                            PaymentMethod = paymentRequest.PaymentMethod,
                            Amount = paymentRequest.Amount,
                            PaymentDate = DateTime.UtcNow,
                            Status = PaymentStatus.Pending
                        };
                        await _paymentRepository.AddAsync(payment);
                    }
                    // For non-COD payments, simulate payment processing
                    if (!IsCashOnDelivery(paymentRequest.PaymentMethod))
                    {
                        var simulatedStatus = await SimulatePaymentGateway();
                        payment.Status = simulatedStatus;
                        if (simulatedStatus == PaymentStatus.Completed)
                        {
                            // Update the Transaction Id on successful payment
                            payment.TransactionId = GenerateTransactionId();
                            // Update order status accordingly
                            order.OrderStatus = OrderStatus.Processing;
                        }
                    }
                    else
                    {
                        // For COD, mark the order status as Processing immediately
                        order.OrderStatus = OrderStatus.Processing;
                    }
                    await _paymentRepository.SaveChangesAsync();
                    await transaction.CommitAsync();

                    var paymentResponse = _mapper.Map<PaymentResponseDto>(payment);
                    _logger.LogInformation(PaymentLogMessages.PaymentCompleted, payment.Id, payment.TransactionId);

                    return _responseHandler.Success(paymentResponse);
                }
                catch (Exception)
                {
                    await transaction.RollbackAsync();
                    _logger.LogWarning(PaymentLogMessages.PaymentFailed);
                    return _responseHandler.InternalServerError<PaymentResponseDto>
                        ("An unexpected error occurred while completing the COD payment.");
                }
            }
        }
        public async Task<ApiResponse<PaymentResponseDto>> GetPaymentByIdAsync(int paymentId)
        {
            try
            {
                var payment = await _paymentRepository
                .GetTableNoTracking()
                .FirstOrDefaultAsync(p => p.Id == paymentId);
                if (payment == null)
                {
                    _logger.LogWarning(PaymentLogMessages.PaymentNotFound, paymentId);
                    return _responseHandler.NotFound<PaymentResponseDto>("Payment not found.");
                }
                var paymentResponse = new PaymentResponseDto
                {
                    PaymentId = payment.Id,
                    OrderId = payment.OrderId,
                    PaymentMethod = payment.PaymentMethod,
                    TransactionId = payment.TransactionId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    Status = payment.Status
                };
                _logger.LogInformation(PaymentLogMessages.PaymentRetrieved, payment.Id);

                return _responseHandler.Success(paymentResponse);
            }
            catch (Exception)
            {
                _logger.LogWarning(PaymentLogMessages.PaymentFailed);
                return _responseHandler.InternalServerError<PaymentResponseDto>
                    ("An unexpected error occurred while completing the COD payment.");
            }
        }
        public async Task<ApiResponse<PaymentResponseDto>> GetPaymentByOrderIdAsync(int orderId)
        {
            try
            {
                var payment = await _paymentRepository.GetTableNoTracking()
                .FirstOrDefaultAsync(p => p.OrderId == orderId);
                if (payment == null)
                {
                    _logger.LogWarning(PaymentLogMessages.PaymentNotFound, orderId);

                    return _responseHandler.NotFound<PaymentResponseDto>("Payment not found for this order.");
                }
                var paymentResponse = new PaymentResponseDto
                {
                    PaymentId = payment.Id,
                    OrderId = payment.OrderId,
                    PaymentMethod = payment.PaymentMethod,
                    TransactionId = payment.TransactionId,
                    Amount = payment.Amount,
                    PaymentDate = payment.PaymentDate,
                    Status = payment.Status
                };
                _logger.LogInformation(PaymentLogMessages.PaymentByOrderRetrieved, orderId, payment.Id);
                return _responseHandler.Success(paymentResponse);
            }
            catch (Exception)
            {
                _logger.LogWarning(PaymentLogMessages.PaymentFailed);
                return _responseHandler.InternalServerError<PaymentResponseDto>
                    ("An unexpected error occurred while completing the COD payment.");
            }
        }

        public async Task<ApiResponse<ConfirmationResponseDto>> UpdatePaymentStatusAsync(PaymentStatusUpdateDto statusUpdate)
        {
            try
            {
                var payment = await _paymentRepository.GetTableAsTracking()
                .Include(p => p.Order)
                .FirstOrDefaultAsync(p => p.Id == statusUpdate.PaymentId);
                if (payment == null)
                {
                    return _responseHandler.NotFound<ConfirmationResponseDto>("Payment not found.");
                }
                payment.Status = statusUpdate.Status;
                // Update order status if payment is now completed and the method is not COD
                if (statusUpdate.Status == PaymentStatus.Completed && !IsCashOnDelivery(payment.PaymentMethod))
                {
                    payment.TransactionId = statusUpdate.TransactionId;
                    payment.Order.OrderStatus = OrderStatus.Processing;
                }
                await _paymentRepository.SaveChangesAsync();
                // Send Order Confirmation Email if Order Status is Processing
                //if (payment.Order.OrderStatus == OrderStatus.Processing)
                //{
                //  await SendOrderConfirmationEmailAsync(payment.Order.Id);
                //}
                var confirmation = new ConfirmationResponseDto
                {
                    Message = $"Payment with ID {payment.Id} updated to status '{payment.Status}'."
                };
                return _responseHandler.Success<ConfirmationResponseDto>(confirmation);
            }
            catch (Exception)
            {
                return _responseHandler.InternalServerError<ConfirmationResponseDto>("An unexpected error occurred while updating the payment status.");
            }
        }
        private async Task<PaymentStatus> SimulatePaymentGateway()
        {
            //Simulate the PG
            await Task.Delay(TimeSpan.FromMilliseconds(1));
            int chance = Random.Shared.Next(1, 101); // 1 to 100
            if (chance <= 60)
                return PaymentStatus.Completed;
            else if (chance <= 90)
                return PaymentStatus.Pending;
            else
                return PaymentStatus.Failed;
        }
        // Generate a unique 12-character transaction ID
        private string GenerateTransactionId()
        {
            return $"TXN-{Guid.NewGuid().ToString("N").ToUpper().Substring(0, 12)}";
        }
        // Determines if the provided payment method indicates Cash on Delivery
        private bool IsCashOnDelivery(string paymentMethod)
        {
            return paymentMethod.Equals("CashOnDelivery", StringComparison.OrdinalIgnoreCase) ||
            paymentMethod.Equals("COD", StringComparison.OrdinalIgnoreCase);
        }


    }
}
