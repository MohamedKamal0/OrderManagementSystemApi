using System.ComponentModel.DataAnnotations.Schema;
using OrderManagementSystemDomain.Enums;

namespace OrderManagementSystemDomain.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        public int BillingAddressId { get; set; }
        [ForeignKey("BillingAddressId")]
        public Address BillingAddress { get; set; }
        public int ShippingAddressId { get; set; }
        [ForeignKey("ShippingAddressId")]
        public Address ShippingAddress { get; set; }
        public decimal TotalBaseAmount { get; set; }
        public decimal TotalDiscountAmount { get; set; }
        public decimal ShippingCost { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
        public Payment Payment { get; set; } //Linked with Associated Payment
        public Cancellation Cancellation { get; set; } //Linked with Associated Cancellation
    }
}
