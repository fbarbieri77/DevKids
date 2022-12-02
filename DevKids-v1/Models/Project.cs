using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace DevKids_v1.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Nome")]
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Descição")]
        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Duração (hrs)")]
        [Column(TypeName = "decimal(4,1)")]
        //[RegularExpression(@"^(\d+)(,\d{2}|\.\d{2})?$")]
        public decimal Duration { get; set; }

        [Required]
        [Display(Name = "Linguagem")]
        [StringLength(15)]
        public string CodeLanguage { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Língua de ensino")]
        [StringLength(15)]
        public string TeachLanguage { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Preço")]
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(6,2)")]
        public decimal Price { get; set; }

        [Required]
        public bool IsOpenToVote { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
