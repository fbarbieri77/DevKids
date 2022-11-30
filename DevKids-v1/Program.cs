using DevKids_v1.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DevKids_v1.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Microsoft.Data.SqlClient;
using Azure.Core;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using DevKids_v1.Services;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Microsoft.Extensions.Configuration;
using QRCoder;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddDbContext<ApplicationDbContext>();
//var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// when using Azure, otherwise %APPDATA%\Microsoft\UserSecrets\
//builder.Configuration.AddAzureKeyVault("https://devkidssecrets.vault.azure.net/");

var identityDB = new SqlConnectionStringBuilder(
    builder.Configuration.GetConnectionString("RazorPagesAuthConnectionLocal"));
//identityDB.UserID = builder.Configuration["AuthenticationDBUserId"]; 
//identityDB.Password = builder.Configuration["AuthenticationDBPassword"];

builder.Services.AddDbContext<RazorPagesAuth>(options =>
    options.UseSqlServer(identityDB.ConnectionString));

builder.Services.AddDefaultIdentity<RazorPagesUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<RazorPagesAuth>();

builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/Projects/Index", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/Projects/Create", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/Projects/Delete", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/Projects/Edit", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/Projects/Details", "Admin"));

builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/ProjectResources/Index", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/ProjectResources/Create", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/ProjectResources/Delete", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/ProjectResources/Edit", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/ProjectResources/Details", "Admin"));

builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/UserResources/Index", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/UserResources/Create", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/UserResources/Delete", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/UserResources/Edit", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/UserResources/Details", "Admin"));

builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/Wallet/Wallet", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/Wallet/Delete", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/Wallet/Edit", "Admin"));
builder.Services.AddRazorPages(options =>
    options.Conventions.AuthorizePage("/Wallet/Details", "Admin"));

builder.Services.AddAuthorization(options =>
    options.AddPolicy("Admin", policy =>
        policy.RequireAuthenticatedUser()
            .RequireClaim("IsAdmin", bool.TrueString)));

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddSingleton(new QRCodeService(new QRCodeGenerator()));

builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

// test this or annotations in file (delete one)
builder.Services.AddRazorPages(options =>
{
    options.Conventions
        .AddPageApplicationModelConvention("/ProjectResources/Create",
            model =>
            {
                model.Filters.Add(
                new RequestFormLimitsAttribute()
                {
                    // Set the limit to 256 MB
                    ValueLengthLimit = 268435456,
                    MultipartBodyLengthLimit = 268435456,
                    MultipartHeadersLengthLimit = 268435456
                });
            });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

CultureInfo defaultCulture = new("pt-BR");
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture(defaultCulture),
    SupportedCultures = new List<CultureInfo> { defaultCulture },
    SupportedUICultures = new List<CultureInfo> { defaultCulture }
};

app.UseRequestLocalization(localizationOptions);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
