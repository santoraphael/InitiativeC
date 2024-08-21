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
            var data = new
            {
                amount = 20000000000,
                tick_speed = 10000,
                timestamp = DateTime.UtcNow
            };

            return new JsonResult(data);
        }
    }
}
