using Blockfrost.Api;
using com.cardano;
using com.database;
using com.database.entities;
using com.initiativec.webpages.Interfaces;
using com.initiativec.webpages.Services;
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
        private readonly TokenBoutyService _tokenBountyService;

        private readonly EmailService _emailService;

        public IndexModel(DatabaseContext context
                            ,BlockfrostServices blockfrostServices
                            ,IEmailSender emailSender
                            ,TokenBoutyService tokenBountyService
                            ,EmailService emailService)
        {
            _context = context;
            _blockfrostServices = blockfrostServices;
            _tokenBountyService = tokenBountyService;

            _emailService = emailService;
        }

        [BindProperty(SupportsGet = true)]
        public string InviteCode { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Name { get; set; }

        [BindProperty]
        [EmailAddress(ErrorMessage = "Por favor, insira um e-mail válido.")]
        public string? Email { get; set; }

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
            var stk_adress = StakeAddress.Result;

            var userExsists = _context.Users.FirstOrDefault(u => u.stake_address == stk_adress);

            if(userExsists != null)
            {
                if ((bool)userExsists.confirmed)
                {
                    ModelState.AddModelError(string.Empty, "Stake Address ou Wallet Address já cadastrado");
                    return Page();
                }
            }
            

            var generatedCode = GenerateInviteCode();
            while (ValidateInviteCode(generatedCode) == false)
            {
                generatedCode = GenerateInviteCode();
            }


            var confirmedMaster = false;

            if(InviteCode == "MASTER")
            {
                var userMaster = _context.Users.FirstOrDefault(u => u.invite_code == "MASTER");
                if(userMaster.invitations_available > 0)
                {
                    confirmedMaster = true;

                    userMaster.invitations_available = userMaster.invitations_available - 1;
                    _context.Update(userMaster);
                }
            }


            User user = new User();

            if (userExsists != null)
            {
                userExsists.name = Name;
                userExsists.email = string.IsNullOrEmpty(Email) ? null : Email;
                userExsists.invited_by = InviteCode;
                userExsists.confirmed = confirmedMaster;
                userExsists.expiration_date_invitations = DateTime.UtcNow.AddDays(14);
                userExsists.currentCulture = CultureInfo.CurrentUICulture.Name;

                _context.Users.Update(userExsists);
            }
            else
            {
                
                user.stake_address = stk_adress;
                user.wallet_address = WalletAddress;
                user.name = Name;
                user.email = string.IsNullOrEmpty(Email) ? null : Email;
                user.phone_number = "";
                user.invite_code = generatedCode;
                user.invited_by = InviteCode;
                //user.status = 0;
                user.confirmation_code_number = GenerateCodeNumericConfirmation();
                user.confirmation_code_alphanumber = GenerateAlphanumericCodeConfirmation();
                user.confirmed = confirmedMaster;
                user.invitations_available = 5;
                user.expiration_date_invitations = DateTime.UtcNow.AddDays(14);
                user.currentCulture = CultureInfo.CurrentUICulture.Name;

                _context.Users.Add(user);
            }

            _context.SaveChanges();



            if(confirmedMaster)
            {
                _tokenBountyService.ReservarValorInicial(user.id);
            }
            

            string subject = "Confirmação de Registro";
            string message = $"Olá {Name},<br/>Obrigado por se registrar!";

            
            _emailService.SendLocalizedEmailAsync(Email, subject, message, user.currentCulture);

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