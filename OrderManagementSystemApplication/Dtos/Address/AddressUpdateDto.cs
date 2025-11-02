using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystemApplication.Dtos.Address
{
    public class AddressUpdateDto
    {
        [Required(ErrorMessage = "AddressId is required.")]
        public int AddressId { get; set; }
        [Required(ErrorMessage = "CustomerId is required.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Address Line 1 is required.")]
        [StringLength(100, ErrorMessage = "Address Line 1 cannot exceed 100 characters.")]
        public string AddressLine { get; set; }
        [Required(ErrorMessage = "City is required.")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
        public string City { get; set; }
        [Required(ErrorMessage = "State is required.")]
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters.")]
        public string State { get; set; }
        [Required(ErrorMessage = "Postal Code is required.")]
        [RegularExpression(@"^\d{4,6}$", ErrorMessage = "Invalid Postal Code.")]
        public string PostalCode { get; set; }
    }
}
