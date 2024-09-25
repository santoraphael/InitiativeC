using com.cardano;
using com.database;
using Microsoft.AspNetCore.Mvc;

namespace com.initiativec.webpages.Controllers
{
    public class MemberController : Controller
    {
        private readonly DatabaseContext _context;
        private readonly BlockfrostServices _blockfrostServices;

        public MemberController(DatabaseContext context
            ,BlockfrostServices blockfrostServices)
        {
            _context = context;
            _blockfrostServices = blockfrostServices;
        }

        [HttpPost]
        public ActionResult AprovarMembro(int Id)
        {

            var token = Request.Cookies["UsuarioToken"];

            var StakeAddress = _blockfrostServices.GetStakeAddress(token);
            var stk_adress = StakeAddress.Result;

            var UsuarioLogado = _context.Users.FirstOrDefault(u => u.stake_address == stk_adress);


            //Verifica se usuário ainda tem convites disponiveis
            if (UsuarioLogado.invitations_available > 0)
            {
                //Busca dados do usuário convidado
                var usuarioConvidado = _context.Users.FirstOrDefault(u => u.id == Id);

                if (usuarioConvidado.confirmed != true)
                {
                    //Cria a carteira virtual do novo membro e faz a reserva do valor.
                    //StartBankPrincipal.ReservarValor(usuarioConvidado);

                    ////Faz a efetivação do valor já reservado na carteira do membro logado no site
                    //StartBankPrincipal.EfetivarValorReservado(UsuarioLogado);

                    ////Altera o Status do novo membro para 1 = Ativo.
                    //usuarioConvidado.Status = 1;
                    ////Faz a aprovação do membro gravando os dados no Banco de dados.
                    //Usuario.AprovarUsuario(usuarioConvidado, GetFirstname(UsuarioLogado.Nome));

                    ////Diminui o numero dos convites disponíveis do usuário logado para menos 1.
                    //UsuarioLogado.ConvitesDisponiveis -= 1;
                    ////Grava os dados alterados do usuário logado
                    //Usuario.AlterarUsuario(UsuarioLogado);
                }
            }
            else
            {
                ViewBag.ConviteAprovadoReprovado = "Você não tem mais convites para aprovar usuários. Aguarde as próximas ações que serão liberadas.";
            }


            return RedirectToAction("Dashboard");
        }

        [HttpPost]
        public ActionResult ReprovarMembro(string Id)
        {
            //var usuario = Usuario.retornaUsuarioByID(Id);

            //usuario.Status = 0;
            //Usuario.AlterarUsuario(usuario);

            return RedirectToAction("Dashboard");
        }
    }
}
