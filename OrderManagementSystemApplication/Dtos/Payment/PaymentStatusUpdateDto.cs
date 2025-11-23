using System.ComponentModel.DataAnnotations;
using OrderManagementSystemDomain.Enums;

namespace OrderManagementSystemApplication.Dtos.Payment
{
    public class PaymentStatusUpdateDto
    {
        [Required(ErrorMessage = "Payment ID is required.")]
        public int PaymentId { get; set; }
        public string? TransactionId { get; set; }
        [Required(ErrorMessage = "Status is required.")]
        public PaymentStatus Status { get; set; } // e.g., "Completed", "Failed"
    }
}
