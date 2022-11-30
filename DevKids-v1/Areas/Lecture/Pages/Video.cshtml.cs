using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace DevKids_v1.Areas.Lecture.Pages
{
    [Authorize]
    public class VideoModel : PageModel
    {
        private readonly string _targetFilePath;
        public string FilePath { get; set; } = string.Empty;

        public VideoModel(IConfiguration config)
        {
            _targetFilePath = config.GetValue<string>("StoredFilesFolder");
        }

        public IActionResult OnGet(string? fileName)
        {
            if (fileName == null)
            {
                return NotFound();
            }

            FilePath = Path.Combine(_targetFilePath, fileName);

            return Page();
        }
    }
}
