using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DevKids_v1.Areas.Checkout.Pages
{
    [Authorize]
    public class CheckoutCancelModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
