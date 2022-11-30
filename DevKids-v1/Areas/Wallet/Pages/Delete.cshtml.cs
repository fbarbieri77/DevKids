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

namespace DevKids_v1.Areas.Wallet.Pages
{
    [Authorize(Policy = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;

        [BindProperty]
        public Purchase Purchase { get; set; }

        public DeleteModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context)
        {
            _context = context;
        }
        
        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.Purchase == null)
            {
                return NotFound();
            }

            var purchase = await _context.Purchase.FirstOrDefaultAsync(m => m.Id == id);

            if (purchase == null)
            {
                return NotFound();
            }
            else 
            {
                Purchase = purchase;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null || _context.Purchase == null)
            {
                return NotFound();
            }
            var purchase = await _context.Purchase.FindAsync(id);

            if (purchase != null)
            {
                Purchase = purchase;
                _context.Purchase.Remove(Purchase);

                var userresources = await _context.UserResource.
                    Where(m => m.ProjectId == purchase.ProjectId).ToListAsync();
                if (userresources != null)
                {
                    foreach(var resource in userresources)
                    {
                        _context.UserResource.Remove(resource);
                    }
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Wallet");
        }
    }
}
