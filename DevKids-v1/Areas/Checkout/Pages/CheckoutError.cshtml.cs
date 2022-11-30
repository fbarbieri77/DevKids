using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevKids_v1.Areas.Checkout.Pages
{
    [Authorize]
    public class CheckoutErrorModel : PageModel
    {
        public string? ErrorMessage { get; private set; }
        public void OnGet(string message)
        {
            ErrorMessage = message;
        }
    }
}
