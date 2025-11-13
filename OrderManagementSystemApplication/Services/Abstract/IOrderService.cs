using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Order;

namespace OrderManagementSystemApplication.Services.Abstract
{
    public interface IOrderService
    {
        Task<ApiResponse<string>> CreateOrderAsync(OrderCreateDto orderDto);
         Task<ApiResponse<OrderResponseDto>> GetOrderByIdAsync(int orderId);
        Task<ApiResponse<List<OrderResponseDto>>> GetOrdersByCustomerAsync(int customerId);
         Task<ApiResponse<string>> UpdateOrderStatusAsync(OrderStatusUpdateDto statusDto);
    }
}
