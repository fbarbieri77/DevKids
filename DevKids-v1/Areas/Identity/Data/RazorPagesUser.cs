using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DevKids_v1.Models;
using Microsoft.AspNetCore.Identity;

namespace DevKids_v1.Areas.Identity.Data;

// Add profile data for application users by adding properties to the RazorPagesUser class
public class RazorPagesUser : IdentityUser
{
    [Required]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;
}

