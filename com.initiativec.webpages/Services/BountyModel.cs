using com.database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace com.initiativec.webpages.Services
{
    public class BountyModel : PageModel
    {
        private readonly DatabaseContext _context;

        private readonly IOptions<RequestLocalizationOptions> _locOptions;

        public BountyModel(DatabaseContext context, IOptions<RequestLocalizationOptions> locOptions)
        {
            _context = context;
            _locOptions = locOptions;
        }

        public JsonResult OnGetBounty() // Método de ação
        {
            var tokenPool = _context.TokenPool.FirstOrDefault();

            decimal total = tokenPool.total;
            decimal divisor = tokenPool.divisor;

            var data = new
            {
                amount = CalculoProximaVaga(total, divisor),
                tick_speed = tokenPool.tick_speed,
                timestamp = DateTime.UtcNow
            };

            return new JsonResult(data);
        }

        private decimal CalculoProximaVaga(decimal total, decimal divisor)
        {
            return total / divisor;
        }
    }
}
