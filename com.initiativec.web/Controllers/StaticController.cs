using Microsoft.AspNetCore.Mvc;

namespace com.initiativec.web.Controllers
{
    public class StaticController : Controller
    {
        public JsonResult Bounty()
        {
            var data = new
            {
                amount = 10000000,
                tick_speed = 400,
                timestamp = DateTime.UtcNow.ToString("ddd, dd MMM yyyy HH:mm:ss 'GMT'")
            };

            return Json(data);
        }
    }
}
