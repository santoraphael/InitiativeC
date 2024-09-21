using com.cardano;
using com.database;
using com.database.entities;
using com.initiativec.webpages.Services;
using com.initiativec.webpages.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace com.initiativec.webpages.Pages.dashboard
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly BlockfrostServices _blockfrostServices;
        private readonly TokenBoutyService _tokenBountyService;
        private readonly TwitterService _twitterService;
        private readonly TelegramService _telegramService;

        public IndexModel(DatabaseContext context
                            ,BlockfrostServices blockfrostServices
                            ,TokenBoutyService tokenBountyService
                            ,TwitterService twitterService
                            ,TelegramService telegramService)
        {
            _context = context;
            _blockfrostServices = blockfrostServices;
            _tokenBountyService = tokenBountyService;
            _twitterService = twitterService;
            _telegramService = telegramService;
        }

        public IList<User> Users { get; set; }
        public string StakeAddress { get; set; }
        public IList<string> WalletAddresses { get; set; }
        public DashboardVM dashboard { get; set; }
        public CardVerificaConvite cardVerificaConvite { get; set; }    
        public CardEnviarConvite enviarConvite { get; set; }
        public CardSaldoAtual cardSaldoAtual { get; set; }

        public FollowStatusCard FollowStatus { get; set; }
        public bool IsUserInGroup { get; set; }

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

            DashboardVM dashboardVM = new DashboardVM();
            dashboardVM.id_usuario = user.id;
            dashboardVM.nome_usuario = user.name;
            dashboardVM.qtd_dias_restantes = CalcularDiasRestantes(Convert.ToDateTime(user.expiration_date_invitations));
            dashboardVM.qtd_convites_restantes = (int)user.invitations_available;
            dashboardVM.amount_token_por_convite = _tokenBountyService.ValorReservaPorConvite(user.id);

            dashboard = dashboardVM;
            cardVerificaConvite = GetCardVerificaConvite(user.invite_code, (int)user.invitations_available);
            enviarConvite = GetCardEnviarConvite(user.invite_code, _tokenBountyService.ValorReservaConviteTotal(user.id), (int)user.invitations_available);
            cardSaldoAtual = GetCardSaldoAtual(user.id);


            //Apenas um exemplo
            //_tokenBountyService.ReservarValorInicial(user.id);

            return Page();
        }


        private int CalcularDiasRestantes(DateTime dataAlvo)
        {
            DateTime agora = DateTime.UtcNow;
            TimeSpan diferenca = dataAlvo - agora;
            int diasRestantes = (int)Math.Ceiling(diferenca.TotalDays);
            return diasRestantes < 0 ? 0 : diasRestantes;
        }

        private CardVerificaConvite GetCardVerificaConvite(string invite_code, int invitations_available)
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
                cardVerificaConvite.status = true;
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

        private CardSaldoAtual GetCardSaldoAtual(int id_usuario)
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

            
            cardSaldoAtual.percentualAtual = 70;
            cardSaldoAtual.percentualReservado = 30;

            cardSaldoAtual.status = true;

            return cardSaldoAtual;
        }


        //ACESSAR https://developer.twitter.com/ PARA IMPLEMENTAR
        private void GetCardFriedShipTwitter()
        {
            string targetUsername = "SeuUsername";
            string sourceUsername = "UsernameDoUsuario";

            // Obter os IDs dos usuários
            long targetUserId = (long)_twitterService.GetUserIdByUsernameAsync(targetUsername).Result;
            long sourceUserId = (long)_twitterService.GetUserIdByUsernameAsync(sourceUsername).Result;


            if (targetUserId != null && sourceUserId != null)
            {
                bool isFollowing = _twitterService.IsFollowingAsync(sourceUserId, targetUserId).Result;
                FollowStatus = new FollowStatusCard
                {
                    SourceUsername = sourceUsername,
                    TargetUsername = targetUsername,
                    IsFollowing = isFollowing
                };
            }
            else
            {
                // Trate o caso onde um dos usuários não foi encontrado
                FollowStatus = new FollowStatusCard
                {
                    SourceUsername = sourceUsername,
                    TargetUsername = targetUsername,
                    IsFollowing = false,
                    ErrorMessage = "Um dos usuários não foi encontrado."
                };
            }
        }


        private void GetCardParticipaGrupoTelegram()
        {
            long userTelegramId = 123456789; // Substitua pelo ID real do usuário

            IsUserInGroup = _telegramService.IsUserInGroupAsync(userTelegramId).Result;
        }


        //public async Task OnGetAsync()
        //{

        //    string wallet = "dasdas";

        //    _context.Users.Select(u => u.wallet_address == wallet);


        //    string walletAddress = "addr1q9p2annkw94a9nrs2gduxu8rhe4v0vvhpnlwtuzttqngtz5wsfgya999406dl2wthe3adeux7c0jv8kfrfdyv4zp4zxshcsqru";

        //    StakeAddress = await _blockfrostServices.GetStakeAddress(walletAddress);

        //    WalletAddresses = await _blockfrostServices.GetAllAddressesByStakeAddress(StakeAddress);

        //    //StakeAddress = await _blockfrostServices.GetStakeAddressFromWalletAddress(walletAddress);

        //    //WalletAddresses = await _blockfrostServices.GetWalletAddressesFromStakeAddress(StakeAddress, 1, 1);

        //}
    }
}
