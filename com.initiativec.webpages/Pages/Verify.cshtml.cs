using com.database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace com.initiativec.webpages.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class VerifyModel : PageModel
    {
        private readonly DatabaseContext _context;

        public VerifyModel(DatabaseContext context)
        {
            _context = context;
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPost([FromBody] TokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Token))
            {
                return new JsonResult(new { acessoPermitido = false });
            }

            // Validar o token no banco de dados
            var usuario = _context.Users.FirstOrDefault(u => u.wallet_address == request.Token);

            if (usuario != null)
            {
                // Token válido, acesso permitido
                return new JsonResult(new { acessoPermitido = true });
            }
            else
            {
                // Token inválido, acesso negado
                return new JsonResult(new { acessoPermitido = false });
            }
        }

        public class TokenRequest
        {
            public string Token { get; set; }
        }
    }
}
