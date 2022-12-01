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
using System.ComponentModel.DataAnnotations;
using DevKids_v1.MyEnum;

namespace DevKids_v1.Pages.ProjectResources
{
    [Authorize(Policy = "Admin")]
    public class EditModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;

        [BindProperty]
        public ProjectResource ProjectResource { get; set; } = default!;

        [BindProperty]
        [Display(Name = "File type")]
        public FileTypeEnum FileTypeChoice { get; set; }


        public EditModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ProjectResources == null)
            {
                return NotFound();
            }

            var projectresource =  await _context.ProjectResources.FirstOrDefaultAsync(m => m.Id == id);
            if (projectresource == null)
            {
                return NotFound();
            }
            ProjectResource = projectresource;
            FileTypeChoice = (FileTypeEnum) Enum.Parse(typeof(FileTypeEnum), projectresource.FileType);

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

            ProjectResource.FileType = FileTypeChoice.ToString();
            _context.Attach(ProjectResource).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectResourceExists(ProjectResource.Id))
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

        private bool ProjectResourceExists(int id)
        {
          return _context.ProjectResources.Any(e => e.Id == id);
        }
    }
}
