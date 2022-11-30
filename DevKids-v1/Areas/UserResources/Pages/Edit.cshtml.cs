using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DevKids_v1.Areas.Identity.Data;
using DevKids_v1.Models;
using Microsoft.AspNetCore.Authorization;

namespace DevKids_v1.Areas.UserResources.Pages
{
    [Authorize(Policy = "Admin")]
    public class EditModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;

        [BindProperty]
        public UserResource UserResource { get; set; } = default!;

        public EditModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.UserResource == null)
            {
                return NotFound();
            }

            var userresource =  await _context.UserResource.FirstOrDefaultAsync(m => m.Id == id);
            if (userresource == null)
            {
                return NotFound();
            }
            UserResource = userresource;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(UserResource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserResourceExists(UserResource.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool UserResourceExists(string id)
        {
          return _context.UserResource.Any(e => e.Id == id);
        }
    }
}
