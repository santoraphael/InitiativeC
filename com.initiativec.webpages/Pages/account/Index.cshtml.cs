using Blockfrost.Api;
using com.cardano;
using com.database;
using com.database.entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Text;

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
        [EmailAddress(ErrorMessage = "Por favor, insira um e-mail válido.")]
        public string Email { get; set; }

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


            var userExsists = _context.Users.Select(u => u.wallet_address == stk_adress).FirstOrDefault();

            if (userExsists)
            {
                ModelState.AddModelError(string.Empty, "Stake Address ou Wallet Address já cadastrado");
                return Page();
            }

            var generatedCode = GenerateInviteCode();
            while (ValidateInviteCode(generatedCode) == false)
            {
                generatedCode = GenerateInviteCode();
            }


            User user = new User()
            {
                wallet_address = stk_adress,
                name = Name,
                email = string.IsNullOrEmpty(Email) ? null : Email,
                phone_number = "",
                invite_code = generatedCode,
                invited_by = InviteCode,
                status = 0,
                confirmation_code_number = GenerateCodeNumericConfirmation(),
                confirmation_code_alphanumber = GenerateAlphanumericCodeConfirmation(),
                confirmed = false,
                invitations_available = 0,
                expiration_date_invitations = DateTime.UtcNow.AddDays(7)
            };

            _context.Users.Add(user);
            _context.SaveChanges();


            return RedirectToPage("/verify");
        }

        // Método opcional para marcar o código como usado
        private async Task MarkInviteCodeAsUsed(string code)
        {
            // Implemente a lógica para marcar o código como usado no banco de dados
        }

        public string GenerateInviteCode()
        {
            Random random = new Random();
            String source = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdfghijkmnoqrtvwxyz";
            Int32 length = 9;

            StringBuilder builder = new StringBuilder(length);

            while (length-- > 0)
                builder.Append(source[random.Next(source.Length)]);

            return builder.ToString();
        }

        public bool ValidateInviteCode(string inviteCode)
        {
            var inviteCodeReturn = _context.Users.Select(u => u.invite_code == inviteCode).FirstOrDefault();

            if (inviteCodeReturn)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public string GenerateCodeNumericConfirmation()
        {
            Random random = new Random();
            String source = "1234567890";
            Int32 length = 4;

            StringBuilder builder = new StringBuilder(length);

            while (length-- > 0)
                builder.Append(source[random.Next(source.Length)]);

            return builder.ToString();
        }

        public string GenerateAlphanumericCodeConfirmation()
        {
            Random random = new Random();
            String source = "abcdfghijkmnoqrtvwxyz1234567890";
            Int32 length = 30;

            StringBuilder builder = new StringBuilder(length);

            while (length-- > 0)
                builder.Append(source[random.Next(source.Length)]);

            return builder.ToString();
        }
    }
}