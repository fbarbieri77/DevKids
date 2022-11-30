using DevKids_v1.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using DevKids_v1.Models;

namespace DevKids_v1.Areas.Identity.Data;

public class RazorPagesAuth : IdentityDbContext<RazorPagesUser>
{
    public RazorPagesAuth(DbContextOptions<RazorPagesAuth> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        // Customize the ASP.NET Identity model and override the defaults if needed.
        // For example, you can rename the ASP.NET Identity table names and more.
        // Add your customizations after calling base.OnModelCreating(builder);
    }

    public DbSet<DevKids_v1.Models.Project> Project { get; set; }

    public DbSet<DevKids_v1.Models.ProjectResource> ProjectResources { get; set; }

    public DbSet<DevKids_v1.Models.Purchase> Purchase { get; set; }

    public DbSet<DevKids_v1.Models.UserResource> UserResource { get; set; }
}
