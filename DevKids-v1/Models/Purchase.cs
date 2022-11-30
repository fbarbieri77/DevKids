using System.ComponentModel.DataAnnotations;

namespace DevKids_v1.Models
{
    public class Purchase
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public string? PayerId { get; set; }
        public string? OrderId { get; set; }
        public string? PaymentId { get; set; }
        public string? RefundId { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Purchase date")]
        public DateTime DateIn { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Refund date")]
        public DateTime DateOut { get; set; }
       
        [Display(Name = "Confirm Payment?")]
        public bool ConfirmPayment { get; set; }
        
        [Display(Name = "Project")]
        public string ProjectTitle { get; set; } = string.Empty;
        public decimal Amount { get; set; }

        [Display(Name = "Can access resources?")]
        public bool HasAccess { get; set; }
    }
}
