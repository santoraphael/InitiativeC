using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace com.initiativec.webpages.Services
{
    public class BountyModel : PageModel
    {
        private readonly IOptions<RequestLocalizationOptions> _locOptions;

        public BountyModel(IOptions<RequestLocalizationOptions> locOptions)
        {
            _locOptions = locOptions;
        }

        public JsonResult OnGetBounty() // Método de ação
        {

            long total = 25000000000;
            long divisor = 1000;

            var data = new
            {
                amount = CalculoProximaVaga(total, divisor),
                tick_speed = 10000,
                timestamp = DateTime.UtcNow
            };

            return new JsonResult(data);
        }

        private long CalculoProximaVaga(long total, long divisor)
        {
            return total / divisor;
        }
    }
}
