using com.initiativec.webpages.ViewModel;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using Microsoft.Extensions.Localization;
using System.Globalization;

namespace com.initiativec.webpages.Services
{
    public class EmailService
    {
        private readonly IStringLocalizer<EmailService> _localizer;
        private readonly IHtmlLocalizer<EmailService> htmlLocalizer;
        private readonly IOptions<RequestLocalizationOptions> LocOptions;

        private readonly EmailApiSettings _emailApiSettings;
        
        private readonly HttpClient _httpClient;

        public EmailService(IOptions<EmailApiSettings> emailApiSettings, IStringLocalizer<EmailService> localizer, HttpClient httpClient)
        {
            _emailApiSettings = emailApiSettings.Value;
            _localizer = localizer;
            _httpClient = httpClient;
        }

        public async Task SendLocalizedEmailAsync(string toEmail, string subject, string body, string culture)
        {
            
            // Define a cultura para as strings localizadas
            //CultureInfo.CurrentCulture = new CultureInfo(culture);
            //CultureInfo.CurrentUICulture = new CultureInfo(culture);

            // Recupera as strings localizadas do arquivo .resx
            string _subject = _localizer[subject];
            string _body = _localizer[body];

            // Envia o e-mail via API
            await SendEmailViaApi(toEmail, _subject, _body);
        }

        private async Task SendEmailViaApi(string toEmail, string subject, string body)
        {
            // Cria o objeto para o corpo da requisição
            var emailData = new
            {
                from = _emailApiSettings.FromEmail,
                to = toEmail,
                subject = subject,
                body = body
            };

            var content = new StringContent(JsonConvert.SerializeObject(emailData), Encoding.UTF8, "application/json");

            // Configura os cabeçalhos e autenticação com o token da API
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("x-auth-token", _emailApiSettings.ApiToken);

            // Envia a requisição POST para a API
            var response = await _httpClient.PostAsync(_emailApiSettings.ApiUrl, content);

            // Tratamento de resposta
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("E-mail enviado com sucesso.");
            }
            else
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Erro ao enviar e-mail: {errorResponse}");
            }
        }
    }
}
