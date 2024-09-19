using Blockfrost.Api;
using com.cardano;
using com.database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace com.initiativec.webpages.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class ValidarAcessoModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly BlockfrostServices _blockfrostServices;

        public ValidarAcessoModel(DatabaseContext context, BlockfrostServices blockfrostServices)
        {
            _context = context;
            _blockfrostServices = blockfrostServices;
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPost([FromBody] TokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Token))
            {
                return new JsonResult(new { acessoPermitido = false, details = "NaoLogado" });
            }

            var StakeAddress = _blockfrostServices.GetStakeAddress(request.Token);

            // Validar o token no banco de dados
            var usuario = _context.Users.FirstOrDefault(u => u.wallet_address == StakeAddress.Result);

            if (usuario != null)
            {
                if(usuario.confirmed == true)
                {
                    // Token v�lido, acesso permitido

                    // Definir o cookie UsuarioToken
                    Response.Cookies.Append("UsuarioToken", request.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = true, // Defina como true se estiver usando HTTPS
                        Expires = DateTimeOffset.UtcNow.AddHours(1), // Defina a expira��o conforme necess�rio
                        SameSite = SameSiteMode.Strict
                    });

                    return new JsonResult(new { acessoPermitido = true, details = "" });
                }
                else
                {
                    return new JsonResult(new { acessoPermitido = false, details = "NaoAprovado" });
                }
            }
            else
            {
                // Token inv�lido, acesso negado
                return new JsonResult(new { acessoPermitido = false, details = "SemConvite" });
            }
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPostLogout()
        {
            // Remove o cookie 'UsuarioToken'
            Response.Cookies.Delete("UsuarioToken");

            // Retorna uma resposta JSON indicando sucesso
            return new JsonResult(new { success = true });
        }

        public class TokenRequest
        {
            public string Token { get; set; }
        }
    }
}
