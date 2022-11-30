using Amazon.CloudWatchLogs;
using DevKids_v1.Models;
using DevKids_v1.MyEnum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace DevKids_v1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly string _logoFolder;
        private readonly IConfiguration _config;
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;
        public IList<Project> AllProjects { get; set; }
        public IList<Purchase> UserPurchases { get; set; }
        
        public IList<Thumbnail> OtherThumbnails { get; set; }
        public IList<Thumbnail> UserThumbnails { get; set; }

        public IndexModel(ILogger<IndexModel> logger, 
            Areas.Identity.Data.RazorPagesAuth context,
            IConfiguration config)
        {
            _logger = logger;
            _context = context;
            AllProjects = new List<Project>();
            UserPurchases = new List<Purchase>();
            OtherThumbnails = new List<Thumbnail>();
            UserThumbnails = new List<Thumbnail>();
            _logoFolder = config.GetValue<string>("StoredFilesFolder");
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Project != null)
            {
                AllProjects = await _context.Project.Where(project => project.IsActive).ToListAsync();
            }
            if (_context.UserResource != null)
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                UserPurchases = await _context.Purchase.Where(m => m.UserId == userId).ToListAsync();
            }

            if (_context.ProjectResources != null && AllProjects != null)
            {
                foreach (var project in AllProjects)
                {
                    string? logoPath = await GetLogoPath(project.Id);
                    if (logoPath != null)
                    {
                        Thumbnail thumbnail = new()
                        {
                            Title = project.Name,
                            Path = logoPath,
                            ProjectId = project.Id
                        };
                        if (UserPurchases.FirstOrDefault(m => m.ProjectId == project.Id && m.HasAccess == true) != null)
                        {
                            UserThumbnails.Add(thumbnail);
                        }
                        else
                        {
                            OtherThumbnails.Add(thumbnail);

                        }
                    }
                }
            }

            return Page();
        }

        public async Task<string?> GetLogoPath(string id)
        {
            var logoResource = await _context.ProjectResources.
                    FirstOrDefaultAsync(resource => resource.ProjectId == id
                    && resource.FileType == FileTypeEnum.Logo.ToString());
            if (logoResource != null)
            {
                return Path.Combine(_logoFolder, logoResource.FileName);
            }
            else
                return null;
        }
    }

    public class Thumbnail
    {
        public string Title { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        public string ProjectId { get; set; }
    }
}