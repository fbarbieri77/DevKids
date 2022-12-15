using DevKids_v1.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevKids_v1.Models;
using Microsoft.AspNetCore.Components.Web;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Text;

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
        public string ResourceTitle { get; set; }

        [BindProperty]
        public Models.Project Project { get; set; }

        [BindProperty]
        public IList<ProjectResource> ProjectResources { get; set; }

        public IndexModel(RazorPagesAuth context, IConfiguration config)
        {
            _context = context;
            _targetFilePath = config.GetValue<string>("StoredFilesPath");
            Project = new Project();
        }

        public async Task<IActionResult> OnGet(int? id)
        {
            if (id == null || _context.ProjectResources == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return Page();
            }

            Project = project;

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
         /*   if (!ModelState.IsValid)
            {
                return Page();
            }
         */
            return RedirectToPage("./Video/", new { fileName = Action, title = ResourceTitle});
        }

        public IActionResult OnPostDownload()
        {
            var filePath = Path.Combine(_targetFilePath, Action);
            if (System.IO.File.Exists(filePath))
            {
                var projectresource = ProjectResources.FirstOrDefault(m => m.FileName == Action);
                byte[] bytes = System.IO.File.ReadAllBytes(filePath);
                StringBuilder fileName = new StringBuilder();
                fileName.Append(ResourceTitle);
                fileName.Append(Action.Substring(Action.IndexOf("."), Action.Length - Action.IndexOf(".")));
                return File(bytes, "application/octet-stream", fileName.ToString());
            }
            else
            {
                Result = "File not found";
                return Page();
            }
        }
    }
}