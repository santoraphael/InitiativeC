using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace com.initiativec.webpages.Pages
{
    public class EnviarEmailModel : PageModel
    {
        public string Mensagem { get; set; }

        public async Task OnGetAsync(string destino)
        {
            try
            {
                await EnviarEmailAsync(destino);
                Mensagem = "Mensagem enviada com sucesso!";
            }
            catch (Exception ex)
            {
                Mensagem = $"A mensagem não pode ser enviada! =( <br/>Erro: {ex.Message}";
            }
        }

        private async Task EnviarEmailAsync(string destino)
        {
            var mensagem = new MimeMessage();
            mensagem.From.Add(new MailboxAddress("Testando PHPMailer", "noreplay@lehmonbros.fun"));
            mensagem.To.Add(MailboxAddress.Parse(destino));
            mensagem.Subject = "Testando o envio do PHPmailer";

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = "Se você recebeu esta mensagem, o envio via phpmailer está funcional!",
                TextBody = "Se você recebeu esta mensagem, o envio via phpmailer está funcional!"
            };
            mensagem.Body = bodyBuilder.ToMessageBody();

            using (var cliente = new SmtpClient())
            {
                

                try
                {
                    cliente.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    cliente.AuthenticationMechanisms.Remove("XOAUTH2");
                    await cliente.ConnectAsync("kinghost.smtpkl.com.br", 587, SecureSocketOptions.StartTls);
                    await cliente.AuthenticateAsync("c9821a698bd8b45bc54f219ed482a878", "eyJhbGciOiJIUzI1NiJ9.eyJzdWIiOiJjOTgyMWE2OThiZDhiNDViYzU0ZjIxOWVkNDgyYTg3OCIsImF1ZCI6ImNsaWVudGVraW5nMTc5NDg2IiwiaWF0IjoxNzI4NTI0NjQ4LjEwOTk1MiwianRpIjoiZTNiYThiZDI4MzgzOWMyNzQwZGMzODlkMjEwZmIwOTEifQ.-6MVSkB9EipQtmNEgBGHaV9s5DVW5E2ESQbpXTS849A");
                    await cliente.SendAsync(mensagem);
                    await cliente.DisconnectAsync(true);
                }
                catch(Exception ex)
                {
                    var t = ex.Message;
                }
                
            }
        }
    }
}
