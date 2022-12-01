using DevKids_v1.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevKids_v1.Models;
using Microsoft.AspNetCore.Components.Web;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace DevKids_v1.Areas.Lecture.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly string _targetFilePath;
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;
        public string? Result { get; private set; }
        public string FilePath { get; set; }

        [BindProperty]
        public string Action { get; set; }

        [BindProperty]
        public IList<ProjectResource> ProjectResources { get; set; }

        public IndexModel(RazorPagesAuth context, IConfiguration config)
        {
            _context = context;
            _targetFilePath = config.GetValue<string>("StoredFilesPath");
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null || _context.ProjectResources == null)
            {
                return NotFound();
            }

            var projectresources = await _context.ProjectResources.
                                            Where(m => m.ProjectId == id).
                                            OrderBy(m => m.SectionOrder).
                                            ThenBy(m => m.InSectionOrder).ToListAsync();
            if (projectresources == null)
            {
                return NotFound();
            }
            else
            {
                ProjectResources = projectresources;
            }

            return Page();
        }

        public IActionResult OnPostButton()
        {
            if (ModelState.IsValid == false)
            {
                return Page();
            }

            return RedirectToPage("./Video/", new { fileName = Action });
        }

        public IActionResult OnPostDownload()
        {
            var filePath = Path.Combine(_targetFilePath, Action);
            if (System.IO.File.Exists(filePath))
            {
                var projectresource = ProjectResources.FirstOrDefault(m => m.FileName == Action);
                byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                return File(bytes, "application/octet-stream", Action);
            }
            else
            {
                Result = "File not found";
                return Page();
            }
        }
    }
}
