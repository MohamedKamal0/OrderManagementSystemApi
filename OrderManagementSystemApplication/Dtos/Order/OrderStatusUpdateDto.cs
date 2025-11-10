using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderManagementSystemDomain.Enums;

namespace OrderManagementSystemApplication.Dtos.Order
{
    public class OrderStatusUpdateDto
    {
        [Required(ErrorMessage = "OrderId is Required")]
        public int OrderId { get; set; }
        [Required]
        [EnumDataType(typeof(OrderStatus), ErrorMessage = "Invalid Order Status.")]
        public OrderStatus OrderStatus { get; set; }
    }
}
