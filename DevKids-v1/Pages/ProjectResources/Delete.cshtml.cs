using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using DevKids_v1.Areas.Identity.Data;
using DevKids_v1.Models;
using Microsoft.AspNetCore.Authorization;

namespace DevKids_v1.Pages.ProjectResources
{
    [Authorize(Policy = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;
        private readonly string _targetFilePath;

        [BindProperty]
        public ProjectResource ProjectResource { get; set; }

        public DeleteModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context, IConfiguration config)
        {
            _context = context;
            _targetFilePath = config.GetValue<string>("StoredFilesPath");
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ProjectResources == null)
            {
                return NotFound();

            }

            var projectresource = await _context.ProjectResources.FirstOrDefaultAsync(m => m.Id == id);

            if (projectresource == null)
            {
                return NotFound();
            }
            else 
            {
                ProjectResource = projectresource;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.ProjectResources == null)
            {
                return NotFound();
            }
            var projectresource = await _context.ProjectResources.FindAsync(id);

            if (projectresource != null)
            {
                ProjectResource = projectresource;
                _context.ProjectResources.Remove(ProjectResource);
                var filePath = Path.Combine(_targetFilePath, projectresource.FileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
