using Microsoft.AspNetCore.Mvc;

namespace com.initiativec.web.Controllers
{
    public class FAQController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
