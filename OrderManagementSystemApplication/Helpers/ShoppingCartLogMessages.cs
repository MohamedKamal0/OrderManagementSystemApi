namespace OrderManagementSystemApplication.Helpers
{
    public static class ShoppingCartLogMessages
    {

        public const string AddToCartStarted = "Starting add to cart operation for customer: {CustomerId}, Product: {ProductId}";
        public const string ProductAddedToCart = "Product added to cart successfully. Customer: {CustomerId}, Product: {ProductId}, Quantity: {Quantity}";
        public const string CartItemQuantityUpdated = "Cart item quantity updated. CartItemId: {CartItemId}, NewQuantity: {Quantity}";
        public const string ErrorAddingToCart = "Error adding product to cart. Customer: {CustomerId}, Product: {ProductId}";

        public const string ProductNotFound = "Product not found. ProductId: {ProductId}";
        public const string InsufficientStock = "Insufficient stock. ProductId: {ProductId}, Requested: {Requested}, Available: {Available}";
        public const string StockExceeded = "Adding quantity exceeds available stock. ProductId: {ProductId}, CurrentInCart: {CurrentInCart}, Adding: {Adding}, Available: {Available}";

        public const string CartCreated = "New cart created for customer: {CustomerId}";
        public const string CartRetrieved = "Cart retrieved successfully. CustomerId: {CustomerId}, ItemsCount: {ItemsCount}";
        public const string CartNotFound = "Active cart not found for customer: {CustomerId}";
        public const string EmptyCartReturned = "Empty cart returned for customer: {CustomerId}";

        public const string CartCleared = "Cart cleared successfully. CustomerId: {CustomerId}, ItemsRemoved: {ItemsCount}";
        public const string ErrorClearingCart = "Error clearing cart for customer: {CustomerId}";

        public const string CartItemRemoved = "Cart item removed successfully. CartItemId: {CartItemId}, CustomerId: {CustomerId}";
        public const string CartItemNotFound = "Cart item not found. CartItemId: {CartItemId}";
        public const string ErrorRemovingCartItem = "Error removing cart item. CartItemId: {CartItemId}, CustomerId: {CustomerId}";

        public const string CartItemUpdated = "Cart item updated successfully. CartItemId: {CartItemId}, NewQuantity: {Quantity}";
        public const string ErrorUpdatingCartItem = "Error updating cart item. CartItemId: {CartItemId}";

        public const string StockUpdated = "Product stock updated. ProductId: {ProductId}, QuantityReduced: {Quantity}";
        public const string StockRestored = "Product stock restored. ProductId: {ProductId}, QuantityRestored: {Quantity}";


        public const string TransactionCommitted = "Cart transaction committed successfully for customer: {CustomerId}";
        public const string TransactionRolledBack = "Cart transaction rolled back for customer: {CustomerId}";

        public const string InvalidCustomerId = "Invalid customer ID received: {CustomerId}";
        public const string InvalidProductId = "Invalid product ID received: {ProductId}";
        public const string InvalidQuantity = "Invalid quantity received: {Quantity}";
        public const string InvalidCartData = "Invalid cart data received";
    }
}
