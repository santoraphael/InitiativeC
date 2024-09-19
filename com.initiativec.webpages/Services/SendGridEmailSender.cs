using com.initiativec.webpages.Interfaces;
using RestSharp.Authenticators;
using RestSharp;

namespace com.initiativec.webpages.Services
{
    public class SendGridEmailSender : IEmailSender
    {
        private readonly string _domain;
        private readonly string _apiKey;

        public SendGridEmailSender(IConfiguration configuration)
        {
            //_apiKey = configuration["SendGrid:ApiKey"];
            _domain = configuration["Mailgun:Domain"];
            _apiKey = configuration["Mailgun:ApiKey"];
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            // Configurar as opções do RestClient com autenticação
            var options = new RestClientOptions($"https://api.mailgun.net/v3/{_domain}")
            {
                Authenticator = new HttpBasicAuthenticator("api", _apiKey)
            };


            // Criar o RestClient com as opções
            var client = new RestClient(options);

            var request = new RestRequest("messages", Method.Post);
            request.AddParameter("from", $"Seu Nome <no-reply@{_domain}>");
            request.AddParameter("to", email);
            request.AddParameter("subject", subject);
            request.AddParameter("text", htmlMessage);

            var response = await client.ExecuteAsync(request);

            if (!response.IsSuccessful)
            {
                throw new Exception($"Falha ao enviar e-mail: {response.ErrorMessage}");
            }




            //var client = new SendGridClient(_apiKey);
            //var from = new EmailAddress("seuemail@seudominio.com", "Seu Nome");
            //var to = new EmailAddress(email);

            //var msg = MailHelper.CreateSingleEmail(from, to, subject, "",htmlMessage);

            //var response = await client.SendEmailAsync(msg);

            //// Opcional: Verificar a resposta e tratar erros
            //if (response.StatusCode >= System.Net.HttpStatusCode.BadRequest)
            //{
            //    // Lógica de tratamento de erros
            //    throw new Exception($"Erro ao enviar e-mail: {response.StatusCode}");
            //}
        }
    }
}
