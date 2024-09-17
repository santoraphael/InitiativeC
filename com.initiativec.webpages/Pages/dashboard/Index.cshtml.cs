using com.cardano;
using com.database;
using com.database.entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace com.initiativec.webpages.Pages.dashboard
{
    public class IndexModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly BlockfrostServices _blockfrostServices;


        public IndexModel(DatabaseContext context, BlockfrostServices blockfrostServices)
        {
            _context = context;
            _blockfrostServices = blockfrostServices;
        }

        public IList<User> Users { get; set; }
        public string StakeAddress { get; set; }
        public IList<string> WalletAddresses { get; set; }

        public IActionResult OnGet()
        {
            var token = Request.Cookies["UsuarioToken"];
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToPage("/Verify");
            }

            return Page();
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
