using com.cardano;
using com.database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace com.initiativec.webpages.Pages.invite
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly IStringLocalizer<IndexModel> _localizer;

        public IndexModel(DatabaseContext context, IStringLocalizer<IndexModel> localizer)
        {
            _context = context;
            _localizer = localizer;
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

                TempData["ErrorTitle"] = _localizer["ErrorTitleInvalidInvite"].Value;
                TempData["ErrorMessage"] = _localizer["ErrorMessageInvalidInvite"].Value;
                return RedirectToPage("/Error");
            }
        }

        private bool IsValidInviteCode(string code)
        {
            var validCode = _context.Users.Any(u => u.invite_code == code && u.invitations_available > 0);
            return validCode;
        }
    }
}