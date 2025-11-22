namespace OrderManagementSystemApplication.Helpers
{
    public static class ProductLogMessages
    {

        public const string ProductNotFound = "Product with Id {ProductId} not found.";
        public const string ProductCreated = "Product with Id {ProductId} created successfully.";
        public const string ProductDeleted = "Product with Id {ProductId} deleted (soft delete).";
        public const string ProductUpdated = "Product with Id {ProductId} updated successfully.";
        public const string ProductStatusUpdated = "Product status updated for Id {ProductId}.";
        public const string ProductsRetrieved = "{Count} products retrieved.";
        public const string ProductsByCategoryRetrieved = "{Count} products retrieved for Category {CategoryId}.";
        public const string CategoryNotFound = "Category with Id {CategoryId} not found.";
        public const string ProductNameConflict = "Product name '{ProductName}' already exists.";
        public const string ErrorCreatingProduct = "Error creating product {ProductName}.";
        public const string ErrorUpdatingProduct = "Error updating product with Id {ProductId}.";
        public const string ErrorDeletingProduct = "Error deleting product with Id {ProductId}.";
        public const string ErrorRetrievingProducts = "Error retrieving products.";
        public const string cachBb = "DB Visited - product fetched from database";

    }
}

