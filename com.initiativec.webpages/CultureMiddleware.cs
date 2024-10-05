using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Threading.Tasks;

namespace com.initiativec.webpages
{
    public class CultureMiddleware
    {
        private readonly RequestDelegate _next;

        public CultureMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // **Ignorar Caminhos de Autenticação**
            if (context.Request.Path.StartsWithSegments("/signin-discord"))
            {
                await _next(context);
                return;
            }

            // **Implementar Lógica de Cultura Personalizada (se necessário)**
            // Exemplo: Definir a cultura com base em um cookie personalizado ou outro critério
            // Este exemplo assume que a cultura já está definida pelo RequestLocalization middleware

            await _next(context);
        }
    }
}
