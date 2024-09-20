using com.cardano;
using com.database;
using com.database.entities;
using com.initiativec.webpages.Services;
using com.initiativec.webpages.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace com.initiativec.webpages.Pages.dashboard
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly BlockfrostServices _blockfrostServices;
        private readonly TokenBoutyService _tokenBountyService;


        public IndexModel(DatabaseContext context, BlockfrostServices blockfrostServices, TokenBoutyService tokenBountyService)
        {
            _context = context;
            _blockfrostServices = blockfrostServices;
            _tokenBountyService = tokenBountyService;
        }

        public IList<User> Users { get; set; }
        public string StakeAddress { get; set; }
        public IList<string> WalletAddresses { get; set; }
        public DashboardVM dashboard { get; set; }


        public IActionResult OnGet()
        {
            var token = Request.Cookies["UsuarioToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/verify");
            }

            var StakeAddress = _blockfrostServices.GetStakeAddress(token);
            var stk_adress = StakeAddress.Result;

            var user = _context.Users.FirstOrDefault(u => u.wallet_address == stk_adress);

            DashboardVM dashboardVM = new DashboardVM();
            dashboardVM.id_usuario = user.id;
            dashboardVM.nome_usuario = user.name;
            dashboardVM.qtd_dias_restantes = CalcularDiasRestantes(Convert.ToDateTime(user.expiration_date_invitations));
            dashboardVM.qtd_convites_restantes = (int)user.invitations_available;
            dashboardVM.amount_token_por_convite = _tokenBountyService.ValorReservaPorConvite(user.id);

            dashboard = dashboardVM;

            //Apenas um exemplo
            //_tokenBountyService.ReservarValorInicial(user.id);

            return Page();
        }


        public int CalcularDiasRestantes(DateTime dataAlvo)
        {
            DateTime agora = DateTime.UtcNow;
            TimeSpan diferenca = dataAlvo - agora;
            int diasRestantes = (int)Math.Ceiling(diferenca.TotalDays);
            return diasRestantes < 0 ? 0 : diasRestantes;
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
