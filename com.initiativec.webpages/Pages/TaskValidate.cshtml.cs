using com.cardano;
using com.database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.Ocsp;

namespace com.initiativec.webpages.Pages
{
    [IgnoreAntiforgeryToken(Order = 1001)]
    public class TaskValidadeModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly BlockfrostServices _blockfrostServices;

        public TaskValidadeModel(DatabaseContext context, BlockfrostServices blockfrostServices)
        {
            _context = context;
            _blockfrostServices = blockfrostServices;
        }

        [ValidateAntiForgeryToken]
        public IActionResult OnPost([FromBody] TokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Token))
            {
                return new JsonResult(new { acessoPermitido = false });
            }

            // Validar o token no banco de dados
            var usuario = _context.Users.FirstOrDefault(u => u.stake_address == request.Token); 

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

        public async Task<IActionResult> OnPostValidateDiscordAsync([FromBody] TokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Token))
            {
                return new JsonResult(new { acessoPermitido = false, details = "NaoLogado" });
            }

            var StakeAddress = _blockfrostServices.GetStakeAddress(request.Token);

            // Validar o token no banco de dados
            var usuario = _context.Users.FirstOrDefault(u => u.stake_address == StakeAddress.Result);

            if (usuario != null)
            {
                if (usuario.confirmed == true)
                {
                    // Token válido, acesso permitido

                    // Definir o cookie UsuarioToken
                    Response.Cookies.Append("UsuarioToken", request.Token, new CookieOptions
                    {
                        HttpOnly = true,
                        Secure = false, // Defina como true se estiver usando HTTPS
                        Expires = DateTimeOffset.UtcNow.AddMonths(1), // Defina a expiração conforme necessário
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
                // Token inválido, acesso negado
                return new JsonResult(new { acessoPermitido = false, details = "SemConvite" });
            }
        }

        public class TokenRequest
        {
            public string Token { get; set; }
        }
    }
}
