using Microsoft.AspNetCore.Mvc;

namespace com.initiativec.web.Controllers
{
    public class TimelineController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
