using DevKids_v1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace DevKids_v1.Areas.Checkout.Pages
{
    [Authorize]
    public class CheckoutCompleteModel : PageModel
    {
        private readonly IConfiguration _config;
        private readonly DevKids_v1.Areas.Identity.Data.RazorPagesAuth _context;
        
        public Purchase Purchase { get; set; }
        public string Status;
        public CheckoutCompleteModel(IConfiguration config, Areas.Identity.Data.RazorPagesAuth context)
        {
            _config = config;
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync(string token, string PayerId)
        {
            //paypal returns token and PayerID
            var purchase = await _context.Purchase.FirstOrDefaultAsync(m => m.OrderId == token);
            if (purchase == null)
            {
                return NotFound();
            }

            string paypalclientID = _config.GetValue<string>("PayPalClientID");
            string paypalclientSecret = _config.GetValue<string>("PayPalClientSecret");
            PayPalAPI paypal = new PayPalAPI();
            await paypal.GetAccessToken(paypalclientID, paypalclientSecret);
            string data = await paypal.CapturePayment(token);
            JObject jsonData = JObject.Parse(data);
            if (jsonData != null && jsonData.ContainsKey("status") && jsonData["status"] != null && jsonData["id"] != null)
            {
                if (jsonData["status"].ToString() == "COMPLETED")
                {
                    string paymentId = jsonData["id"].ToString();
                    purchase.PaymentId = paymentId;
                    purchase.PayerId = PayerId;
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

            Purchase = purchase;
            if (Purchase.HasAccess)
            {
                Status = "Aprovado";
            }
            else
            {
                Status = "Não Aprovado.";
            }

            return Page();
        }
    }
}
