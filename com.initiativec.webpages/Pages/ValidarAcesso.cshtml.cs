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

        
        public IActionResult OnPost([FromBody] TokenRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Token))
            {
                return new JsonResult(new { acessoPermitido = false });
            }

            var StakeAddress = _blockfrostServices.GetStakeAddress(request.Token);

            // Validar o token no banco de dados
            var usuario = _context.Users.FirstOrDefault(u => u.wallet_address == StakeAddress.Result);

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
