using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace com.initiativec.webpages.Controllers
{
    [Route("[controller]")]
    public class SetCultureController : Controller
    {
        [HttpPost]
        public IActionResult SetCulture([FromBody] CultureRequest request)
        {
            if (!string.IsNullOrEmpty(request.Culture))
            {
                var culture = new CultureInfo(request.Culture);
                Response.Cookies.Append(
                    CookieRequestCultureProvider.DefaultCookieName,
                    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
                );
            }

            return Ok();
        }
    }

    public class CultureRequest
    {
        public string Culture { get; set; }
    }
}
