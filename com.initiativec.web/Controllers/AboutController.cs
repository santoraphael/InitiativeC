using Microsoft.AspNetCore.Mvc;

namespace com.initiativec.web.Controllers
{
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
