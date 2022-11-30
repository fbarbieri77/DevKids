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

namespace DevKids_v1.Areas.UserResources.Pages
{
    [Authorize(Policy = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;

        [BindProperty]
        public UserResource UserResource { get; set; }

        public DeleteModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.UserResource == null)
            {
                return NotFound();
            }

            var userresource = await _context.UserResource.FirstOrDefaultAsync(m => m.Id == id);

            if (userresource == null)
            {
                return NotFound();
            }
            else 
            {
                UserResource = userresource;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null || _context.UserResource == null)
            {
                return NotFound();
            }
            var userresource = await _context.UserResource.FindAsync(id);

            if (userresource != null)
            {
                UserResource = userresource;
                _context.UserResource.Remove(UserResource);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
