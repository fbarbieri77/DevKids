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
using NuGet.Versioning;

namespace DevKids_v1.Pages.Projects
{
    [Authorize(Policy = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;
        private readonly string _targetFilePath;

        [BindProperty]
        public Project Project { get; set; }

        public DeleteModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context, IConfiguration config)
        {
            _context = context;
            _targetFilePath = config.GetValue<string>("StoredFilesPath");
        }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.Project == null)
            {
                return NotFound();
            }

            var project = await _context.Project.FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return NotFound();
            }
            else 
            {
                Project = project;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null || _context.Project == null)
            {
                return NotFound();
            }
            var project = await _context.Project.FindAsync(id);

            if (project != null)
            {
                Project = project;
                _context.Project.Remove(Project);
                
                var projectresource = await _context.ProjectResources.
                    Where(m => m.ProjectId == id).ToListAsync();
                if (projectresource != null)
                {
                    foreach (var resource in projectresource)
                    {
                        var filePath = Path.Combine(_targetFilePath, resource.FileName);
                        if (System.IO.File.Exists(filePath))
                        {
                            System.IO.File.Delete(filePath);
                        }
                        _context.ProjectResources.Remove(resource);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
