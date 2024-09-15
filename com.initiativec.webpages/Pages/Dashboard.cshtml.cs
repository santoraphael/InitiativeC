using com.cardano;
using com.database;
using com.database.entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace com.initiativec.webpages.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly DatabaseContext _context;
        private readonly BlockfrostService _blockfrostService;

        public DashboardModel(DatabaseContext context)
        {
            _context = context;
            _blockfrostService = new BlockfrostService();
        }

        public IList<User> Users { get; set; }
        public string StakeAddress { get; set; }
        public IList<string> WalletAddresses { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();

            string walletAddress = "addr1qykejxf3y7zdmpz46lujz3y8wjvqcrgxexhg8pjnktthlrt572xe6wu67dqq88ra7lpwkdgr43xugeykwy4d9vuzsa6qtynn2y";

            StakeAddress = await _blockfrostService.GetStakeAddressFromWalletAddress(walletAddress);

            WalletAddresses = await _blockfrostService.GetWalletAddressesFromStakeAddress(StakeAddress, 1, 1);

        }
    }
}
