namespace OrderManagementSystemApplication.Helpers
{
    public static class CategoryLogMessages
    {
        // Creation
        public const string CategoryCreated = "Category created successfully. ID: {CategoryId}";
        public const string CategoryNameConflict = "Category creation conflict: {CategoryName}";
        public const string ErrorCreatingCategory = "Error creating category: {CategoryName}";

        // Retrieval
        public const string CategoriesRetrieved = "Retrieved {Count} categories.";
        public const string CategoryRetrieved = "Category retrieved successfully. ID: {CategoryId}";
        public const string ErrorRetrievingCategories = "Error retrieving categories.";
        public const string cachBb = "DB Visited - Categories fetched from database";

    }
}
