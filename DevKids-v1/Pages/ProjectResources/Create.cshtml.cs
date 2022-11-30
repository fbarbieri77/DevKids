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
using DevKids_v1.Pages.Projects;
using System.ComponentModel.DataAnnotations;
using DevKids_v1.Utilities;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using DevKids_v1.MyEnum;

namespace DevKids_v1.Pages.ProjectResources
{
    [Authorize(Policy = "Admin")]
    [DisableRequestSizeLimit]
    [RequestFormLimits(MultipartBodyLengthLimit = 268435456)]
    public class CreateModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;
        private readonly long _fileSizeLimit;
        private readonly string _targetFilePath;
        private readonly string[] _permittedExtensions = { ".txt", ".py", ".mp4", ".mkv", ".jpg", ".zip" };
        public string? Result { get; private set; }

        [BindProperty]
        public BufferedSingleFileUploadPhysical FileUpload { get; set; }

        [BindProperty]
        public ProjectResource ProjectResource { get; set; }

        [BindProperty]
        [Display(Name = "File type")]
        public FileTypeEnum FileTypeChoice { get; set; }

        [BindProperty]
        public string _projectId { get; set; }

        [BindProperty]
        public IList<ProjectResource> CurrentProjectResources { get; set; } = default!;
        
        public CreateModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context, IConfiguration config)
        {
            _context = context;
            _fileSizeLimit = config.GetValue<long>("FileSizeLimit");
            _targetFilePath = config.GetValue<string>("StoredFilesPath");
            FileTypeChoice = FileTypeEnum.Resource;
        }

        public async Task<IActionResult> OnGet(string? id)
        {
            if (_context.ProjectResources != null && id != null)
            {
                CurrentProjectResources = await _context.ProjectResources.
                 Where(x => x.ProjectId == id).OrderBy(x => x.Title).ToListAsync();
            }
            
            if (id != null)
            {
                _projectId = (string)id;
            }

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (!AreFormDataCorrect())
            {
                return Page();
            }

            var formFileContent =
                await FileHelpers.ProcessFormFile<BufferedSingleFileUploadPhysical>(
                    FileUpload.FormFile, ModelState, _permittedExtensions, _fileSizeLimit);
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form";

                return Page();
            }

            // **WARNING!**
            // In the following example, the file is saved without
            // scanning the file's contents. In most production
            // scenarios, an anti-virus/anti-malware scanner API
            // is used on the file before making the file available
            // for download or for use by other systems. 
            var trustedFileNameForFileStorage = string.Format("{0}.{1}",
                                                                Guid.NewGuid().ToString("N"), 
                                                                Path.GetExtension(FileUpload.FormFile.FileName));
            var filePath = Path.Combine(_targetFilePath, trustedFileNameForFileStorage);
            using (var fileStream = System.IO.File.Create(filePath))
            {
                await fileStream.WriteAsync(formFileContent);
            }
            ProjectResource.FileName = trustedFileNameForFileStorage;
            ProjectResource.ProjectId = _projectId;
            ProjectResource.FileType = FileTypeChoice.ToString();
            _context.ProjectResources.Add(ProjectResource);
            await _context.SaveChangesAsync();

            await ClearModel();

            return Page();
        }

        public bool AreFormDataCorrect()
        {
            if (!ModelState.IsValid)
            {
                Result = "Please correct the form";

                return false;
            }
            else if (FileUpload == null || FileUpload.FormFile == null)
            {
                Result = "Form file is null";

                return false;
            }
            else if (ProjectResource == null)
            {
                Result = "ProjectResource property is null";

                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task ClearModel()
        {
            if (_context.ProjectResources != null)
            {
                CurrentProjectResources = await _context.ProjectResources.
                    Where(x => x.ProjectId == ProjectResource.ProjectId).OrderBy(x => x.Title).ToListAsync();
            }

            ModelState.Clear();
            ProjectResource = new ProjectResource();
            FileTypeChoice = FileTypeEnum.Lecture;
        }
    }

    public class BufferedSingleFileUploadPhysical
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile? FormFile { get; set; }

        [Display(Name = "Note")]
        [StringLength(50, MinimumLength = 0)]
        public string? Note { get; set; }
    }
}