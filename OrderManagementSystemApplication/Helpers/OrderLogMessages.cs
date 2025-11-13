using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystemApplication.Helpers
{
    public static class OrderLogMessages
    {
        // Order Creation
        public const string OrderCreated = "Order created successfully. OrderNumber: {OrderNumber}, CustomerID: {CustomerId}";
        public const string ErrorCreatingOrder = "Error creating order for customer: {CustomerId}";
        public const string OrderCreationStarted = "Starting order creation for customer: {CustomerId}";

        // Customer Validation
        public const string CustomerNotFound = "Customer not found. ID: {CustomerId}";
        public const string CustomerValidated = "Customer validated successfully. ID: {CustomerId}";

        // Address Validation
        public const string BillingAddressNotFound = "Billing address not found. AddressID: {AddressId}, CustomerID: {CustomerId}";
        public const string ShippingAddressNotFound = "Shipping address not found. AddressID: {AddressId}, CustomerID: {CustomerId}";
        public const string BillingAddressInvalid = "Invalid billing address. AddressID: {AddressId}, CustomerID: {CustomerId}";
        public const string ShippingAddressInvalid = "Invalid shipping address. AddressID: {AddressId}, CustomerID: {CustomerId}";
        public const string AddressesValidated = "Addresses validated successfully for customer: {CustomerId}";

        // Product Validation
        public const string ProductNotFound = "Product not found. ProductID: {ProductId}";
        public const string InsufficientStock = "Insufficient stock for product. ProductID: {ProductId}, Required: {Required}, Available: {Available}";
        public const string ProductValidated = "Product validated successfully. ProductID: {ProductId}";

        // Order Items Processing
        public const string OrderItemsProcessed = "Processed {Count} order items for order: {OrderNumber}";
        public const string StockUpdated = "Stock updated for product. ProductID: {ProductId}, Quantity: {Quantity}";

        // Shopping Cart
        public const string ShoppingCartCheckedOut = "Shopping cart checked out for customer: {CustomerId}";
        public const string ShoppingCartNotFound = "Shopping cart not found for customer: {CustomerId}";

        // Order Retrieval
        public const string OrderRetrieved = "Order retrieved successfully. OrderID: {OrderId}";
        public const string OrderNotFound = "Order not found. OrderID: {OrderId}";
        public const string ErrorRetrievingOrder = "Error retrieving order. OrderID: {OrderId}";

        // Orders by Customer
        public const string OrdersRetrieved = "Retrieved {Count} orders for customer: {CustomerId}";
        public const string ErrorRetrievingOrders = "Error retrieving orders for customer: {CustomerId}";

        // Order Status Update
        public const string OrderStatusUpdated = "Order status updated successfully. OrderID: {OrderId}, OldStatus: {OldStatus}, NewStatus: {NewStatus}";
        public const string InvalidStatusTransition = "Invalid status transition. OrderID: {OrderId}, CurrentStatus: {CurrentStatus}, NewStatus: {NewStatus}";
        public const string ErrorUpdatingOrderStatus = "Error updating order status. OrderID: {OrderId}";

        // Validation Messages
        public const string InvalidOrderData = "Invalid order data received. OrderDTO is null";
        public const string InvalidOrderId = "Invalid order ID received: {OrderId}";
        public const string InvalidCustomerId = "Invalid customer ID received: {CustomerId}";
        public const string EmptyOrderItems = "Order items cannot be empty for customer: {CustomerId}";
    }
}
