using System.ComponentModel.DataAnnotations;

namespace DevKids_v1.Models
{
    public class Purchase
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int ProjectId { get; set; } 
        public string? PayerId { get; set; }
        public string? OrderId { get; set; }
        public string? PaymentId { get; set; }
        public string? RefundId { get; set; }


        [DataType(DataType.Date)]
        [Display(Name = "Data da Compra")]
        public DateTime DateIn { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Data do Reembolso")]
        public DateTime DateOut { get; set; }
       
        [Display(Name = "Confirmar Pagamento?")]
        public bool ConfirmPayment { get; set; }
        
        [Display(Name = "Projeto")]
        public string ProjectTitle { get; set; } = string.Empty;
        
        [Display(Name = "Valor")]
        public decimal Amount { get; set; }

        [Display(Name = "Pode Acessar Aulas?")]
        public bool HasAccess { get; set; }
    }
}
