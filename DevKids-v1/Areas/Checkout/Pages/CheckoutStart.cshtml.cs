using DevKids_v1.Areas.Identity.Data;
using DevKids_v1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DevKids_v1.Areas.Checkout.Pages
{
    [Authorize]
    public class CheckoutStartModel : PageModel
    {
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;
        private readonly IConfiguration _config;
        [BindProperty]
        public Models.Project Project { get; set; } 

        public CheckoutStartModel(DevKids_v1.Areas.Identity.Data.RazorPagesAuth context, IConfiguration config)
        {
            _context = context;
            _config = config;
            Project = new Project();
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return Page();
            }

            var project = await _context.Project.FirstOrDefaultAsync(m => m.Id == id);

            if (project == null)
            {
                return Page();
            }

            Project = project;

            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            Models.Purchase purchase = new()
            {
                UserId = userId,
                ProjectId = Project.Id,
                DateIn = DateTime.UtcNow.Date,
                DateOut = default(DateTime),
                ConfirmPayment = true,
                PaymentId = string.Empty,
                RefundId = string.Empty,
                ProjectTitle = Project.Name,
                Amount = Project.Price,
                HasAccess = false
            };

            if (!ModelState.IsValid)
            {
                return Page();
            }

            string paypalclientID = _config.GetValue<string>("PayPalClientID");
            string paypalclientSecret = _config.GetValue<string>("PayPalClientSecret");
            PayPalAPI paypal = new();
            await paypal.GetAccessToken(paypalclientID, paypalclientSecret);
            string orderId = await paypal.CreateOrder(purchase);
            if (orderId == null)
            {
                RedirectToPage("/Checkout/CheckoutError?message=Failed to create order");
            }

            purchase.OrderId = orderId;

            _context.Purchase.Add(purchase);
            await _context.SaveChangesAsync();

            await AddResourcesToUser(userId);

            return Redirect(paypal.ProcessPayment(orderId));
        }

        public async Task AddResourcesToUser(string userId)
        {
            var resourcesBag = await _context.ProjectResources.Where(m => m.ProjectId == Project.Id).ToListAsync();
            foreach (var content in resourcesBag)
            {
                Models.UserResource resource = new()
                {
                    UserId = userId,
                    ProjectId = content.ProjectId,
                    ResourceId = content.Id,
                    Title = content.Title,
                    FileName = content.FileName,
                    FileType = content.FileType,
                    SectionTitle = content.SectionTitle,
                    SectionOrder = content.SectionOrder,
                    InSectionOrder = content.InSectionOrder,
                    HasCompleted = false
                };

                if (!ModelState.IsValid)
                {
                    return;
                }

                _context.UserResource.Add(resource);
            }
            await _context.SaveChangesAsync();
        }
    }
}
