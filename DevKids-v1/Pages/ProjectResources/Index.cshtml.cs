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
    public class IndexModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;
        public IList<ProjectResource> ProjectResource { get; set; } = default!;

        [BindProperty]
        public int GoToProjectId { get; set; }

        public IndexModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.ProjectResources != null)
            {
                ProjectResource = await _context.ProjectResources.
                    OrderBy(x => x.ProjectId).ToListAsync();
            }
            return Page();
        }
    }
}
