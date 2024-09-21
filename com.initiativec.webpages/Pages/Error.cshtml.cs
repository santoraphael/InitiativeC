using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace com.initiativec.webpages.Pages
{
    public class ErrorModel : PageModel
    {
        [TempData]
        public string ErrorTitle { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public void OnGet()
        {
        }
    }
}
