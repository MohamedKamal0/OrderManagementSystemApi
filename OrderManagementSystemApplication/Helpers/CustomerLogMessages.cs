using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystemApplication.Helpers
{
    public static class CustomerLogMessages
    {
        public const string CustomerNotFound = "Customer with Id {CustomerId} not found.";
        public const string CustomerCreated = "Customer with Id {CustomerId} created successfully.";
        public const string CustomerUpdated = "Customer with Id {CustomerId} updated successfully.";
        public const string CustomerDeleted = "Customer with Id {CustomerId} deleted (soft delete).";
        public const string CustomerRetrieved = "Customer with Id {CustomerId} retrieved successfully.";
        public const string CustomersRetrieved = "{Count} customers retrieved successfully.";

        public const string EmailConflict = "Email {Email} is already in use.";
        public const string ErrorCreatingCustomer = "Error occurred while creating customer {CustomerId}.";
        public const string ErrorUpdatingCustomer = "Error occurred while updating customer {CustomerId}.";
        public const string ErrorDeletingCustomer = "Error occurred while deleting customer {CustomerId}.";
        public const string ErrorRetrievingCustomer = "Error occurred while retrieving customer {CustomerId}.";

    }
}
