using com.cardano;
using com.database;
using com.database.entities;
using com.initiativec.webpages.Interfaces;
using com.initiativec.webpages.Services;
using com.initiativec.webpages.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using Telegram.Bot.Types;

namespace com.initiativec.webpages.Pages.dashboard
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly BlockfrostServices _blockfrostServices;
        private readonly TokenBoutyService _tokenBountyService;

        public IndexModel(DatabaseContext context
                            ,BlockfrostServices blockfrostServices
                            ,TokenBoutyService tokenBountyService)
        {
            _context = context;
            _blockfrostServices = blockfrostServices;
            _tokenBountyService = tokenBountyService;
        }

        public IList<database.entities.User> Users { get; set; }
        public string StakeAddress { get; set; }
        public IList<string> WalletAddresses { get; set; }
        public DashboardVM dashboard { get; set; }
        public CardVerificaConvite cardVerificaConvite { get; set; }    
        public CardEnviarConvite enviarConvite { get; set; }
        public CardSaldoAtual cardSaldoAtual { get; set; }

        public FollowStatusCard FollowStatus { get; set; }
        public bool IsUserInGroup { get; set; }
        
        [BindProperty]
        public int UserId { get; set; }

        [BindProperty]
        public bool? IsUserInChannel { get; set; }

        public IActionResult OnGet()
        {
            var token = Request.Cookies["UsuarioToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/verify");
            }

            var StakeAddress = _blockfrostServices.GetStakeAddress(token);
            var stk_adress = StakeAddress.Result;

            var user = _context.Users.FirstOrDefault(u => u.stake_address == stk_adress);

            if (user == null)
            {
                return RedirectToPage("/verify");
            }

            DashboardVM dashboardVM = new DashboardVM();
            dashboardVM.id_usuario = user.id;
            dashboardVM.nome_usuario = user.name;
            dashboardVM.qtd_dias_restantes = CalcularDiasRestantes(Convert.ToDateTime(user.expiration_date_invitations));
            dashboardVM.qtd_convites_restantes = (int)user.invitations_available;
            dashboardVM.amount_token_por_convite = _tokenBountyService.ValorReservaPorConvite(user.id);

            dashboard = dashboardVM;
            cardVerificaConvite = GetCardVerificaConvite(user.invite_code, (int)user.invitations_available, dashboardVM.qtd_dias_restantes);
            enviarConvite = GetCardEnviarConvite(user.invite_code, _tokenBountyService.ValorReservaConviteTotal(user.id), (int)user.invitations_available);

            int percentualTarefa = 0;

            //Atividades:
            //Cadastrar 10 % -1.125.000
            //Convidar 5 Amigos: 40 %     -- CARD ENVIAR CONVITE
            //Seguir Twitter: 10 %        -- CARD SEGUIR TWITTER
            //Entrar no Discord: 10 %     -- CARD DISCORD
            //Entrar no Telegram: 10 %      
            //Adicionar Email: 10 %
            //Fazer a primeira contribui��o m�nima para a Pool 10 %

            if (cardVerificaConvite.status)
            {
                percentualTarefa = 40;
                
            }
            else
            {
                percentualTarefa = 10;
            }


            cardSaldoAtual = GetCardSaldoAtual(user.id, percentualTarefa);


            return Page();
        }

        private int CalcularDiasRestantes(DateTime dataAlvo)
        {
            DateTime agora = DateTime.UtcNow;
            TimeSpan diferenca = dataAlvo - agora;
            int diasRestantes = (int)Math.Ceiling(diferenca.TotalDays);
            return diasRestantes < 0 ? 0 : diasRestantes;
        }

        private CardVerificaConvite GetCardVerificaConvite(string invite_code, int invitations_available, int qtd_dias_restantes)
        {
            CardVerificaConvite cardVerificaConvite = new CardVerificaConvite();
            List<UsuarioAcao> usuarioAcoes = new List<UsuarioAcao>();


            var users = _context.Users
                    .Where(u => u.invited_by == invite_code)
                    .ToList();

            if(users != null)
            {
                foreach (var user in users.Where(u => u.confirmed == false))
                {
                    UsuarioAcao usuarioAcao = new UsuarioAcao();
                    usuarioAcao.Id = user.id;
                    usuarioAcao.Nome = user.name;

                    usuarioAcoes.Add(usuarioAcao);
                }

                cardVerificaConvite.Usuarios = usuarioAcoes;
                cardVerificaConvite.ConvitesParaRevisar = usuarioAcoes.Count();
                cardVerificaConvite.Aprovados = users.Where(u => u.confirmed == true).Count();
                cardVerificaConvite.ConvitesDisponiveis = invitations_available;

                if(invitations_available > 0 && qtd_dias_restantes > 0)
                {
                    cardVerificaConvite.status = true;
                }
                else
                {
                    cardVerificaConvite.status = false;
                }
                
            }

            return cardVerificaConvite;
        }

        private CardEnviarConvite GetCardEnviarConvite(string invite_code, decimal valorConviteTotal, int convitesRestantes)
        {
            CardEnviarConvite cardEnviarConvite = new CardEnviarConvite();
            cardEnviarConvite.valorConviteTotal = valorConviteTotal;
            cardEnviarConvite.invite_code = invite_code;
            
            if(convitesRestantes>0)
            {
                cardEnviarConvite.status = true;
            }
            else
            {
                cardEnviarConvite.status = false;
            }

            return cardEnviarConvite;
        }

        private CardSaldoAtual GetCardSaldoAtual(int id_usuario, int percentualTarefa)
        {
            CardSaldoAtual cardSaldoAtual = new CardSaldoAtual();
            var tokens = _context.TokenBounties.FirstOrDefault(t => t.id_usuario == id_usuario);

            if(tokens != null)
            {
                cardSaldoAtual.valorSaldo = tokens.valor_reservado;
                cardSaldoAtual.valorSaldoTotal = tokens.valor_reserva_total;
            }
            else
            {
                cardSaldoAtual.valorSaldo = 0;
                cardSaldoAtual.valorSaldoTotal = 0;
            }

            var cardSaldoAtualAtualizado = _tokenBountyService.ObterReservaPercentuais(id_usuario, percentualTarefa, cardSaldoAtual);

            //cardSaldoAtual.percentualAtual = 70;
            //cardSaldoAtual.percentualReservado = 30;

            cardSaldoAtualAtualizado.status = true;

            return cardSaldoAtualAtualizado;
        }



        // Handler para Aprovar
        public IActionResult OnPostApprove()
        {
            var token = Request.Cookies["UsuarioToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/verify");
            }

            var StakeAddress = _blockfrostServices.GetStakeAddress(token);
            var stk_adress = StakeAddress.Result;

            var user = _context.Users.FirstOrDefault(u => u.stake_address == stk_adress);

            // Valide o UserId conforme necess�rio
            if (UserId > 0)
            {
                // L�gica para aprovar o usu�rio
                AprovarUsuario(UserId);

                var amount_token_por_convite = _tokenBountyService.ValorReservaPorConvite(user.id);
                _tokenBountyService.ReservarValorTarefa(user.id, amount_token_por_convite);
                
                
                TempData["Message"] = "Usu�rio aprovado com sucesso.";
            }
            else
            {
                TempData["Error"] = "ID de usu�rio inv�lido.";
            }

            return RedirectToPage();
        }

        // Handler para Reprovar
        public IActionResult OnPostReject()
        {
            // Valide o UserId conforme necess�rio
            if (UserId > 0)
            {
                // L�gica para reprovar o usu�rio
                ReprovarUsuario(UserId);
                TempData["Message"] = "Usu�rio reprovado com sucesso.";
            }
            else
            {
                TempData["Error"] = "ID de usu�rio inv�lido.";
            }

            return RedirectToPage();
        }

        private void AprovarUsuario(int userId)
        {
            var token = Request.Cookies["UsuarioToken"];

            var StakeAddress = _blockfrostServices.GetStakeAddress(token);
            var stk_adress = StakeAddress.Result;

            var UsuarioLogado = _context.Users.FirstOrDefault(u => u.stake_address == stk_adress);


            //Verifica se usu�rio ainda tem convites disponiveis
            if (UsuarioLogado.invitations_available > 0)
            {
                //Busca dados do usu�rio convidado
                var usuarioConvidado = _context.Users.FirstOrDefault(u => u.id == userId);

                if (usuarioConvidado.confirmed != true)
                {
                    //Cria a carteira virtual do novo membro e faz a reserva do valor.
                    _tokenBountyService.ReservarValorInicial(userId);

                    ////Altera o Status do novo membro para 1 = Ativo.
                    usuarioConvidado.confirmed = true;
                    ////Faz a aprova��o do membro gravando os dados no Banco de dados.
                    _context.Users.Update(usuarioConvidado);

                    UsuarioLogado.invitations_available = UsuarioLogado.invitations_available - 1;
                    _context.Update(UsuarioLogado);
                }
            }
            else
            {
                
            }

            _context.SaveChanges();

        }

        private void ReprovarUsuario(int userId)
        {
            var token = Request.Cookies["UsuarioToken"];
            var StakeAddress = _blockfrostServices.GetStakeAddress(token);
            var stk_adress = StakeAddress.Result;
            var UsuarioLogado = _context.Users.FirstOrDefault(u => u.stake_address == stk_adress);

            var usuarioConvidado = _context.Users.FirstOrDefault(u => u.id == userId);

            if(usuarioConvidado.invited_by == UsuarioLogado.invite_code)
            {
                _context.Users.Remove(usuarioConvidado);

                _context.SaveChanges();
            }
        }
    }
}
