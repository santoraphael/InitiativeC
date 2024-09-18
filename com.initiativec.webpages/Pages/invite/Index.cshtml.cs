using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace com.initiativec.webpages.Pages.invite
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string InviteCode { get; set; }
        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(InviteCode))
            {
                return RedirectToPage("/Error");
            }

            if (IsValidInviteCode(InviteCode))
            {
                return RedirectToPage("/account/Index", new { inviteCode = InviteCode });
            }
            else
            {
                TempData["ErrorMessage"] = "Código de convite inválido ou expirado.";
                return RedirectToPage("/Error");
            }
        }

        private bool IsValidInviteCode(string code)
        {
            var validCodes = new List<string> { "ABC123", "DEF456", "GHI789" };
            return validCodes.Contains(code);
        }
    }
}