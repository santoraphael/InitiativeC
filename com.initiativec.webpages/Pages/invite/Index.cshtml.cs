using com.cardano;
using com.database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace com.initiativec.webpages.Pages.invite
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseContext _context;

        public IndexModel(DatabaseContext context)
        {
            _context = context;
        }

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
            var validCode = _context.Users.Select(u => u.invite_code == code).FirstOrDefault();
            return validCode;
        }
    }
}