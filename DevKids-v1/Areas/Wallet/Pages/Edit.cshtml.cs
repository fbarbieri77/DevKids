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
using DevKids_v1.Utilities;
using System.Globalization;

namespace DevKids_v1.Areas.Wallet.Pages
{
    [Authorize(Policy = "Admin")]
    public class EditModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;
        
        [BindProperty]
        public Purchase Purchase { get; set; } = default!;

        [BindProperty]
        public ValidationHelper.Project Valid { get; set; }

        public EditModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context)
        {
            _context = context;
            Valid = new ValidationHelper.Project();
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Purchase == null)
            {
                return NotFound();
            }

            var purchase =  await _context.Purchase.FirstOrDefaultAsync(m => m.Id == id);
            if (purchase == null)
            {
                return NotFound();
            }
            Purchase = purchase;
            Valid.Price = Purchase.Amount.ToString();

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            Purchase.Amount = Decimal.Parse(Valid.Price, culture);

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(Purchase.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Wallet");
        }

        private bool PurchaseExists(int id)
        {
          return _context.Purchase.Any(e => e.Id == id);
        }
    }
}
