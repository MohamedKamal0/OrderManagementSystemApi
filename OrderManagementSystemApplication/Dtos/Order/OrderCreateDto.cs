using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagementSystemApplication.Dtos.Order
{
    public class OrderCreateDto
    {
        [Required(ErrorMessage = "Customer ID is required.")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Billing Address ID is required.")]
        public int BillingAddressId { get; set; }
        [Required(ErrorMessage = "Shipping Address ID is required.")]
        public int ShippingAddressId { get; set; }
        [Required(ErrorMessage = "At least one order item is required.")]
        [MinLength(1, ErrorMessage = "At least one order item is required.")]
        public List<OrderItemCreateDto> OrderItems { get; set; }
    }
}
