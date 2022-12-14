using System.ComponentModel.DataAnnotations;

namespace DevKids_v1.Models
{
    public class UserResource
    {
        public int Id { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int ProjectId { get; set; }
        public int ResourceId { get; set; }

        [Required]
        [Display(Name = "Título")]
        [StringLength(40)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string FileName { get; set; } = string.Empty;

        [StringLength(12)]
        public string FileType { get; set; } = string.Empty;

        [Required]
        [StringLength(40)]
        [Display(Name = "Título da Seção")]
        public string SectionTitle { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Ordem da Seção")]
        public int SectionOrder { get; set; }

        [Required]
        [Display(Name = "Ordem do Recurso")]
        public int InSectionOrder { get; set; }

        public bool HasCompleted { get; set; }
    }
}
