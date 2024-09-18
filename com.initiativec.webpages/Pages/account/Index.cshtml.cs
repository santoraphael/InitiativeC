using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace com.initiativec.webpages.Pages.account
{
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string InviteCode { get; set; }

        public void OnGet()
        {
            // Utilize InviteCode conforme necess�rio
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Processo de registro
            // Associe o c�digo de convite ao novo usu�rio, se necess�rio

            // Marque o c�digo como usado, se aplic�vel
            // await MarkInviteCodeAsUsed(InviteCode);

            return RedirectToPage("/Index");
        }

        // M�todo opcional para marcar o c�digo como usado
        private async Task MarkInviteCodeAsUsed(string code)
        {
            // Implemente a l�gica para marcar o c�digo como usado no banco de dados
        }
    }
}