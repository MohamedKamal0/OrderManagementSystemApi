using System.ComponentModel.DataAnnotations.Schema;
using OrderManagementSystemDomain.Enums;
namespace OrderManagementSystemDomain.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public string PaymentMethod { get; set; } // e.g., "DebitCard", "CreditCard", "PayPal", "COD"
        public string? TransactionId { get; set; } // From payment gateway
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; } // "Completed", "Pending", "Failed", "Refunded"
        public Refund Refund { get; set; }
    }
}
