using System.ComponentModel.DataAnnotations;

namespace DevKids_v1.Models
{
    public class UserResource
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string ProjectId { get; set; }
        public string ResourceId { get; set; }

        [Required]
        [StringLength(40)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string FileName { get; set; } = string.Empty;

        [StringLength(12)]
        public string FileType { get; set; } = string.Empty;

        [Required]
        [StringLength(40)]
        [Display(Name = "Section Title")]
        public string SectionTitle { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Section Order")]
        public int SectionOrder { get; set; }

        [Required]
        [Display(Name = "Resource order")]
        public int InSectionOrder { get; set; }

        public bool HasCompleted { get; set; }
    }
}
