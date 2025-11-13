using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystemApplication.Helpers
{
    public static class AddressLogMessages
    {
        public const string AddressNotFound = "Address with Id {AddressId} not found.";
        public const string AddressCreated = "Address with Id {AddressId} created for Customer {CustomerId}.";
        public const string AddressUpdated = "Address with Id {AddressId} updated for Customer {CustomerId}.";
        public const string AddressDeleted = "Address with Id {AddressId} deleted for Customer {CustomerId}.";
        public const string CustomerNotFound = "Customer with Id {CustomerId} not found.";
        public const string AddressesRetrieved = "{Count} addresses retrieved for Customer {CustomerId}.";
        public const string ErrorCreatingAddress = "Error creating address for Customer {CustomerId}.";
        public const string ErrorUpdatingAddress = "Error updating address with Id {AddressId} for Customer {CustomerId}.";
        public const string ErrorDeletingAddress = "Error deleting address with Id {AddressId} for Customer {CustomerId}.";
        public const string ErrorRetrievingAddress = "Error occurred while retrieving Address {CustomerId}.";

    }
}
