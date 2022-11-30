using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using DevKids_v1.Areas.Identity.Data;
using DevKids_v1.Models;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Azure.KeyVault.Models;
using static Humanizer.In;
using System.Globalization;
using Newtonsoft.Json.Linq;
using DevKids_v1.Utilities;

namespace DevKids_v1.Pages.Projects
{
    [Authorize(Policy = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;

        [BindProperty]
        public Project Project { get; set; }

        [BindProperty]
        public ValidationHelper.Project Valid { get; set; }

        public CreateModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }
        
        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            CultureInfo culture = CultureInfo.CurrentCulture;
            Project.Duration = Decimal.Parse(Valid.Duration, culture); 
            Project.Price = Decimal.Parse(Valid.Price, culture);

            if (!ModelState.IsValid || Project == null)
            {
                return Page();
            }

            _context.Project.Add(Project);
            await _context.SaveChangesAsync();

            return RedirectToPage("../ProjectResources/Create/", new { id = Project.Id });
        }
    }
}
