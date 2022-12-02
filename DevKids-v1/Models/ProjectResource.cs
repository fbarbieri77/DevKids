//using Microsoft.Build.Framework;
using Microsoft.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace DevKids_v1.Models
{
    public class ProjectResource
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }

        [Required]
        [StringLength(40)]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        public string FileName { get; set; } = string.Empty;

        [StringLength(12)]
        public string FileType { get; set; } = string.Empty;

        [Required]
        [StringLength(40)]
        public string SectionTitle { get; set; } = string.Empty;

        [Required]
        public int SectionOrder { get; set; }

        [Required]
        public int InSectionOrder { get; set; }
    }
}
