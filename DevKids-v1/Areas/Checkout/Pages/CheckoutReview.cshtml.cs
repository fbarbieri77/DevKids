using DevKids_v1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace DevKids_v1.Areas.Checkout.Pages
{
    [Authorize]
    public class CheckoutReviewModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;

        [BindProperty]
        public IList<Purchase> Purchases { get; set; }
        public CheckoutReviewModel(Areas.Identity.Data.RazorPagesAuth context, IConfiguration config)
        {
            _config = config;
            _context = context;
            Purchases = new List<Purchase>();
        }
        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            Purchases = _context.Purchase.Where(m => m.UserId == userId).ToList();

            var PendingPurchases = Purchases.Where(m => m.ConfirmPayment == true
                                                        && m.OrderId != string.Empty).ToList();
            if (PendingPurchases != null && PendingPurchases.Count > 0)
            {
                await ConfirmPayment(PendingPurchases);
            }
        }

        public async Task ConfirmPayment(IList<Purchase> purchases)
        {
            string paypalclientID = _config.GetValue<string>("PayPalClientID");
            string paypalclientSecret = _config.GetValue<string>("PayPalClientSecret");
            PayPalAPI paypal = new PayPalAPI();
            await paypal.GetAccessToken(paypalclientID, paypalclientSecret);
            foreach (Purchase purchase in purchases)
            {
                string data = await paypal.CapturePayment((string)purchase.OrderId);
                JObject jsonData = JObject.Parse(data);
                if (jsonData != null && jsonData.ContainsKey("status") && jsonData["status"] != null && jsonData["id"] != null)
                {
                    if (jsonData["status"].ToString() == "COMPLETED")
                    {
                        string paymentId = jsonData["id"].ToString();
                        purchase.PaymentId = paymentId;
                        purchase.ConfirmPayment = false;
                        purchase.HasAccess = true;
                        _context.Attach(purchase).State = EntityState.Modified;
                        try
                        {
                            await _context.SaveChangesAsync();
                        }
                        catch (DbUpdateConcurrencyException)
                        {
                            throw;
                        }
                    }
                }
            }
        }
    }
}
