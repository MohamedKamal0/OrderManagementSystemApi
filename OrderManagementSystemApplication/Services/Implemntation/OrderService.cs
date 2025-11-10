using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos;
using OrderManagementSystemApplication.Dtos.Order;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Enums;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class OrderService(IOrderRepository _orderRepository,
        ICustomerRepository _customerRepository, IAddressRepository _addressRepository,
        IProductRepository _productRepository, IShoppingRepository _shoppingRepository) : IOrderService
    {
        private static readonly Dictionary<OrderStatus, List<OrderStatus>> AllowedStatusTransitions = new()
{
{ OrderStatus.Pending, new List<OrderStatus> { OrderStatus.Processing, OrderStatus.Canceled } },
{ OrderStatus.Processing, new List<OrderStatus> { OrderStatus.Shipped, OrderStatus.Canceled } },
{ OrderStatus.Shipped, new List<OrderStatus> { OrderStatus.Delivered } },
{ OrderStatus.Delivered, new List<OrderStatus>() }, 
{ OrderStatus.Canceled, new List<OrderStatus>() } 
};
        public async Task<ApiResponse<OrderResponseDto>> CreateOrderAsync(OrderCreateDto orderDto)
        {
            var customer = await _customerRepository.GetByIdAsync(orderDto.CustomerId);
            if (customer == null)
            {
                return new ApiResponse<OrderResponseDto>(404, "Customer does not exist.");
            }
            var billingAddress = await _addressRepository.GetByIdAsync(orderDto.BillingAddressId);
            if (billingAddress == null || billingAddress.CustomerId != orderDto.CustomerId)
            {
                return new ApiResponse<OrderResponseDto>(400, "Billing Address is invalid or does not belong to the customer.");
            }
            var shippingAddress = await _addressRepository.GetByIdAsync(orderDto.ShippingAddressId);
            if (shippingAddress == null || shippingAddress.CustomerId != orderDto.CustomerId)
            {
                return new ApiResponse<OrderResponseDto>(400, "Shipping Address is invalid or does not belong to the customer.");
            }
            decimal totalBaseAmount = 0;
            decimal totalDiscountAmount = 0;
            decimal shippingCost = 10.00m; 
            decimal totalAmount = 0;
            string orderNumber = GenerateOrderNumber();
            var orderItems = new List<OrderItem>();
            foreach (var itemDto in orderDto.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                {
                    return new ApiResponse<OrderResponseDto>(404, $"Product with ID {itemDto.ProductId} does not exist.");
                }
                if (product.StockQuantity < itemDto.Quantity)
                {
                    return new ApiResponse<OrderResponseDto>(400, $"Insufficient stock for product {product.Name}.");
                }
                decimal basePrice = itemDto.Quantity * product.Price;
                decimal discount = (product.DiscountPercentage / 100.0m) * basePrice;
                decimal totalPrice = basePrice - discount;
                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price,
                    Discount = discount,
                    TotalPrice = totalPrice
                };
                orderItems.Add(orderItem);
                totalBaseAmount += basePrice;
                totalDiscountAmount += discount;
                product.StockQuantity -= itemDto.Quantity;
                await _productRepository.UpdateAsync(product);
            }
            totalAmount = totalBaseAmount - totalDiscountAmount + shippingCost;
            var order = new Order
            {
                OrderNumber = orderNumber,
                CustomerId = orderDto.CustomerId,
                OrderDate = DateTime.UtcNow,
                BillingAddressId = orderDto.BillingAddressId,
                ShippingAddressId = orderDto.ShippingAddressId,
                TotalBaseAmount = totalBaseAmount,
                TotalDiscountAmount = totalDiscountAmount,
                ShippingCost = shippingCost,
                TotalAmount = totalAmount,
                OrderStatus = OrderStatus.Pending,
                OrderItems = orderItems
            };
            await _orderRepository.AddAsync(order);
            var cart = await _shoppingRepository.GetTableAsTracking().
                FirstOrDefaultAsync(c => c.CustomerId == orderDto.CustomerId && !c.IsCheckedOut);
            if (cart != null)
            {
                cart.IsCheckedOut = true;
                cart.UpdatedAt = DateTime.UtcNow;
                await _shoppingRepository.UpdateAsync(cart);
            }
            await _orderRepository.SaveChangesAsync();
            var orderResponse = MapOrderToDTO(order, customer, billingAddress, shippingAddress);
            return new ApiResponse<OrderResponseDto>(200, orderResponse);
        }

        public async Task<ApiResponse<OrderResponseDto>> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
                if (order == null)
                {
                    return new ApiResponse<OrderResponseDto>(404, "Order not found.");
                }
                var orderResponse = MapOrderToDTO(order, order.Customer, order.BillingAddress, order.ShippingAddress);
                return new ApiResponse<OrderResponseDto>(200, orderResponse);
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");

            }
        }
        public async Task<ApiResponse<List<OrderResponseDto>>> GetOrdersByCustomerAsync(int customerId)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerWithOrdersAsync(customerId);
                if (customer == null)
                {
                    return new ApiResponse<List<OrderResponseDto>>(404, "Customer not found.");
                }
                var orders = customer.Orders.Select(o => MapOrderToDTO(o, customer, o.BillingAddress, o.ShippingAddress)).ToList();
                return new ApiResponse<List<OrderResponseDto>>(200, orders);
            }
            catch (Exception ex)
            {
                return new ApiResponse<List<OrderResponseDto>>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ConfirmationResponseDto>> UpdateOrderStatusAsync(OrderStatusUpdateDto statusDto)
        {
            try
            {
                var order = await _orderRepository.GetTableNoTracking().FirstOrDefaultAsync(o => o.Id == statusDto.OrderId);
                if (order == null)
                {
                    return new ApiResponse<ConfirmationResponseDto>(404, "Order not found.");
                }
                var currentStatus = order.OrderStatus;
                var newStatus = statusDto.OrderStatus;
                if (!AllowedStatusTransitions.TryGetValue(currentStatus, out var allowedStatuses))
                {
                    return new ApiResponse<ConfirmationResponseDto>(500, "Current order status is invalid.");
                }
                if (!allowedStatuses.Contains(newStatus))
                {
                    return new ApiResponse<ConfirmationResponseDto>(400, $"Cannot change order status from {currentStatus} to {newStatus}.");
                }
                order.OrderStatus = newStatus;
                await _orderRepository.SaveChangesAsync();
                var confirmation = new ConfirmationResponseDto
                {
                    Message = $"Order Status with Id {statusDto.OrderId} updated successfully."
                };
                return new ApiResponse<ConfirmationResponseDto>(200, confirmation);
            }
            catch (Exception ex)
            {
                return new ApiResponse<ConfirmationResponseDto>(500, $"An unexpected error occurred while processing your request, Error: {ex.Message}");
            }
        }
        
        private OrderResponseDto MapOrderToDTO(Order order, Customer customer, Address billingAddress, Address shippingAddress)
        {
            var orderItemsDto = order.OrderItems.Select(oi => new OrderItemResponseDto
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                Discount = oi.Discount,
                TotalPrice = oi.TotalPrice
            }).ToList();
            return new OrderResponseDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                BillingAddressId = order.BillingAddressId,
                ShippingAddressId = order.ShippingAddressId,
                TotalBaseAmount = order.TotalBaseAmount,
                TotalDiscountAmount = order.TotalDiscountAmount,
                ShippingCost = order.ShippingCost,
                TotalAmount = Math.Round(order.TotalAmount, 2),
                OrderStatus = order.OrderStatus,
                OrderItems = orderItemsDto
            };
        }
        // Generates a unique order number using the current UTC date/time and a random number.
        // Format: ORD-yyyyMMdd-HHmmss-XXXX
        private string GenerateOrderNumber()
        {
            return $"ORD-{DateTime.UtcNow.ToString("yyyyMMdd-HHmmss")}-{RandomNumber(1000, 9999)}";
        }
        // Generates a random number between min and max.
        private int RandomNumber(int min, int max)
        {
            using (var rng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[4];
                rng.GetBytes(bytes);
                return Math.Abs(BitConverter.ToInt32(bytes, 0) % (max - min + 1)) + min;
            }
        }

       
    }
}

            
   

