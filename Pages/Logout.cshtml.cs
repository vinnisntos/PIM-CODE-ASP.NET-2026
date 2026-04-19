using Microsoft.AspNetCore.Mvc.RazorPages;
using PIM2026.Services;

namespace PIM2026.Pages
{
    public class LogoutModel : PageModel
    {
        public void OnGet()
        {
            AuthHelper.Logout(HttpContext);
        }
    }
}
