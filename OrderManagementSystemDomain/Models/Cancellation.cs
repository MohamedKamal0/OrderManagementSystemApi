using System.ComponentModel.DataAnnotations.Schema;
using OrderManagementSystemDomain.Enums;

namespace OrderManagementSystemDomain.Models
{
    public class Cancellation
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public Order Order { get; set; }
        public string Reason { get; set; }
        public CancellationStatus Status { get; set; }
        public DateTime RequestedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }
        public int? ProcessedBy { get; set; } // Could link to an Admin entity if available
        public decimal OrderAmount { get; set; }
        public decimal? CancellationCharges { get; set; } = 0.00m;
        public string? Remarks { get; set; }
        public Refund Refund { get; set; }
    }
}
