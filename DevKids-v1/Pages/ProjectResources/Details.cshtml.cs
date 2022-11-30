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
    public class DetailsModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;
        public ProjectResource ProjectResource { get; set; }

        public DetailsModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string? id)
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
    }
}
