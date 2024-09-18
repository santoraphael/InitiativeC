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
            // Utilize InviteCode conforme necessário
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Processo de registro
            // Associe o código de convite ao novo usuário, se necessário

            // Marque o código como usado, se aplicável
            // await MarkInviteCodeAsUsed(InviteCode);

            return RedirectToPage("/Index");
        }

        // Método opcional para marcar o código como usado
        private async Task MarkInviteCodeAsUsed(string code)
        {
            // Implemente a lógica para marcar o código como usado no banco de dados
        }
    }
}