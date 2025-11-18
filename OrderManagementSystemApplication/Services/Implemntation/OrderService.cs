using System.Security.Cryptography;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos.Order;
using OrderManagementSystemApplication.Helpers;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Enums;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class OrderService(IOrderRepository _orderRepository,
        ICustomerRepository _customerRepository, IAddressRepository _addressRepository,
        IProductRepository _productRepository, IShoppingRepository _shoppingRepository,
        IMapper _mapper, ResponseHandler _responseHandler, ILogger<OrderService> _logger) : IOrderService
    {

        public async Task<ApiResponse<string>> CreateOrderAsync(OrderCreateDto orderDto)
        {
            try
            {
                var customer = await _customerRepository.GetByIdAsync(orderDto.CustomerId);
                if (customer == null)
                {
                    return _responseHandler.NotFound<string>("Customer does not exist.");
                }
                var billingAddress = await _addressRepository.GetByIdAsync(orderDto.BillingAddressId);
                if (billingAddress == null || billingAddress.CustomerId != orderDto.CustomerId)
                {
                    return _responseHandler.BadRequest<string>("Billing Address is invalid or does not belong to the customer.");
                }
                var shippingAddress = await _addressRepository.GetByIdAsync(orderDto.ShippingAddressId);
                if (shippingAddress == null || shippingAddress.CustomerId != orderDto.CustomerId)
                {
                    return _responseHandler.BadRequest<string>("Shipping Address is invalid or does not belong to the customer.");
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
                        _logger.LogWarning(OrderLogMessages.ProductNotFound, itemDto.ProductId);

                        return _responseHandler.NotFound<string>($"Product with ID {itemDto.ProductId} does not exist.");
                    }
                    if (product.StockQuantity < itemDto.Quantity)
                    {
                        return _responseHandler.BadRequest<string>($"Insufficient stock for product {product.Name}.");
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
                var order = _mapper.Map<Order>(orderDto);
                order.OrderNumber = orderNumber;
                order.TotalAmount = totalAmount;
                order.TotalBaseAmount = totalBaseAmount;
                order.TotalDiscountAmount = totalDiscountAmount;
                order.ShippingCost = shippingCost;
                order.OrderItems = orderItems;
                order.OrderStatus = OrderStatus.Pending;
                order.OrderDate = DateTime.UtcNow;

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
                _logger.LogInformation(OrderLogMessages.OrderCreated, orderNumber, orderDto.CustomerId);

                return _responseHandler.Created($"Order {orderNumber} created successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, OrderLogMessages.ErrorCreatingOrder, orderDto.CustomerId);
                return _responseHandler.InternalServerError<string>(
                    "An error occurred while creating the order.");
            }
        }

        public async Task<ApiResponse<OrderResponseDto>> GetOrderByIdAsync(int orderId)
        {
            try
            {
                var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
                if (order == null)
                {
                    _logger.LogWarning(OrderLogMessages.OrderNotFound, orderId);

                    return _responseHandler.NotFound<OrderResponseDto>("Order not found.");
                }
                var orderResponse = _mapper.Map<OrderResponseDto>(order);
                _logger.LogInformation(OrderLogMessages.OrderRetrieved, orderId);

                return _responseHandler.Success(orderResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, OrderLogMessages.ErrorRetrievingOrder, orderId);
                return _responseHandler.InternalServerError<OrderResponseDto>(
                    "An error occurred while retrieving the order.");
            }
        }
        public async Task<ApiResponse<List<OrderResponseDto>>> GetOrdersByCustomerAsync(int customerId)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerWithOrdersAsync(customerId);
                if (customer == null)
                {
                    _logger.LogWarning(OrderLogMessages.CustomerNotFound, customerId);

                    return _responseHandler.NotFound<List<OrderResponseDto>>("Customer not found.");
                }
                var orders = _mapper.Map<List<OrderResponseDto>>(customer.Orders);

                _logger.LogInformation(OrderLogMessages.OrdersRetrieved, orders.Count, customerId);
                return _responseHandler.Success(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, OrderLogMessages.ErrorRetrievingOrders, customerId);
                return _responseHandler.InternalServerError<List<OrderResponseDto>>(
                    "An error occurred while retrieving orders.");
            }
        }
        public async Task<ApiResponse<string>> UpdateOrderStatusAsync(OrderStatusUpdateDto statusDto)
        {
            try
            {
                var order = await _orderRepository.GetTableNoTracking().FirstOrDefaultAsync(o => o.Id == statusDto.OrderId);
                if (order == null)
                {
                    _logger.LogWarning(OrderLogMessages.OrderNotFound, statusDto.OrderId);

                    return _responseHandler.NotFound<string>("Order not found.");
                }
                var currentStatus = order.OrderStatus;
                var newStatus = statusDto.OrderStatus;
                if (!AllowedStatusTransitions.TryGetValue(currentStatus, out var allowedStatuses))
                {
                    _logger.LogWarning(OrderLogMessages.InvalidStatusTransition,
                            statusDto.OrderId, currentStatus, newStatus);
                    return _responseHandler.BadRequest<string>("Current order status is invalid.");
                }
                if (!allowedStatuses.Contains(newStatus))
                {
                    _logger.LogWarning(OrderLogMessages.InvalidStatusTransition,
                            statusDto.OrderId, currentStatus, newStatus);
                    return _responseHandler.BadRequest<string>($"Cannot change order status from {currentStatus} to {newStatus}.");
                }
                order.OrderStatus = newStatus;
                await _orderRepository.SaveChangesAsync();
                _logger.LogInformation(OrderLogMessages.OrderStatusUpdated,
                  statusDto.OrderId, currentStatus, newStatus);

                return _responseHandler.Updated($"Order status updated to {newStatus} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, OrderLogMessages.ErrorUpdatingOrderStatus, statusDto.OrderId);
                return _responseHandler.InternalServerError<string>(
                    "An error occurred while updating the order status.");
            }
        }


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


        private static readonly Dictionary<OrderStatus, List<OrderStatus>> AllowedStatusTransitions = new()
{
{ OrderStatus.Pending, new List<OrderStatus> { OrderStatus.Processing, OrderStatus.Canceled } },
{ OrderStatus.Processing, new List<OrderStatus> { OrderStatus.Shipped, OrderStatus.Canceled } },
{ OrderStatus.Shipped, new List<OrderStatus> { OrderStatus.Delivered } },
{ OrderStatus.Delivered, new List<OrderStatus>() },
{ OrderStatus.Canceled, new List<OrderStatus>() }
};

    }
}




