using Blockfrost.Api;
using com.cardano;
using com.database;
using com.database.entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace com.initiativec.webpages.Pages.account
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

        [BindProperty(SupportsGet = true)]
        public string InviteCode { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Name { get; set; }

        [BindProperty]
        public string WalletAddress { get; set; }

        public async Task OnGetAsync()
        {
            var dasd = InviteCode;
            //Users = await _context.Users.ToListAsync();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrEmpty(WalletAddress))
            {
                ModelState.AddModelError(string.Empty, "Endereço da carteira não encontrado.");
                return Page();
            }

            var StakeAddress = _blockfrostServices.GetStakeAddress(WalletAddress);
            var stk_adress = "";

            if (StakeAddress != null)
            {
                stk_adress = StakeAddress.Result;
            }
            else
            {
                stk_adress = WalletAddress;
            }


            User user = new User()
            {
                wallet_address = stk_adress,
                name = Name,
                email = "", // Ou null, se permitido
                phone_number = "", // Ou null
                invite_code = "", // Ou null
                invited_by = "", // Ou null
                status = 0, // Ou outro valor padrão, ou null se permitido
                confirmation_code_number = "", //GenerateConfirmationNumber(),
                confirmation_code_alphanumber = "", // GenerateConfirmationCode(),
                confirmed = false,
                invitations_available = 0, // Ou outro valor padrão
                expiration_date_invitations = DateTime.UtcNow.AddDays(7) // Ou DateTime.UtcNow.AddDays(7), por exemplo
            };

            _context.Users.Add(user);
            _context.SaveChanges();


            return RedirectToPage("/dashboard");
        }

        // Método opcional para marcar o código como usado
        private async Task MarkInviteCodeAsUsed(string code)
        {
            // Implemente a lógica para marcar o código como usado no banco de dados
        }
    }
}