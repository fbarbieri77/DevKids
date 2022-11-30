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

namespace DevKids_v1.Pages.Projects
{
    [Authorize(Policy = "Admin")]
    public class DetailsModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;
        public Project Project { get; set; }

        public DetailsModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context)
        {
            _context = context;
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
    }
}
