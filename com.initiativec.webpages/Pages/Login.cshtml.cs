
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace com.initiativec.webpages.Pages
{
    public class LoginModel : PageModel
    {
        public _howtoContentModel HowToContentModel { get; set; }


        public async Task OnGetAsync()
        {
            HowToContentModel = new _howtoContentModel();
            //Users = await _context.Users.ToListAsync();
        }
    }
}
