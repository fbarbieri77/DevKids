using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DevKids_v1.Utilities
{
    public class ValidationHelper
    {
        public class Project
        {
            [BindProperty]
            [RegularExpression("^(?:-?\\d+|-?\\d{1,3}(?:\\.\\d{3})+)?(?:\\,\\d{1,2})?$",
            ErrorMessage = "Number must have a maximum of 2 digits after a comma separtor")]
            [Display(Name = "Duration")]
            public string Duration { get; set; } = string.Empty;

            [BindProperty]
            [RegularExpression("^(?:-?\\d+|-?\\d{1,3}(?:\\.\\d{3})+)?(?:\\,\\d{1,2})?$",
                ErrorMessage = "Number must have a maximum of 2 digits after a comma separtor")]
            [Display(Name = "Price")]
            public string Price { get; set; } = String.Empty;
        }
    }
}
