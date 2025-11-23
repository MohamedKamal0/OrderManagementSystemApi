using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystemApplication.Dtos.Payment
{
    public class CODPaymentUpdateDto
    {
        [Required(ErrorMessage = "Order ID is required.")]
        public int OrderId { get; set; }
        [Required(ErrorMessage = "Payment Id is required.")]
        public int PaymentId { get; set; }
    }
}
