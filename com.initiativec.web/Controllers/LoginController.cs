using Microsoft.AspNetCore.Mvc;

namespace com.initiativec.web.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
