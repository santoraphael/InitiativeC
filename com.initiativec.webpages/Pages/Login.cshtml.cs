using com.initiativec.webpages.Database;
using com.initiativec.webpages.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace com.initiativec.webpages.Pages
{
    public class LoginModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        

        public LoginModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<User> Users { get; set; }

        public async Task OnGetAsync()
        {
            Users = await _context.Users.ToListAsync();
        }
    }
}
