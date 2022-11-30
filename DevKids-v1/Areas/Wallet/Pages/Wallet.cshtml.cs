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

namespace DevKids_v1.Areas.Wallet.Pages
{
    [Authorize(Policy = "Admin")]
    public class WalletModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;
        public IList<Purchase> Purchase { get;set; } = default!;
        
        public WalletModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            if (_context.Purchase != null)
            {
                Purchase = await _context.Purchase.ToListAsync();
            }
        }
    }
}
