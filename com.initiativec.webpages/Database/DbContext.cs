using com.initiativec.webpages.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace com.initiativec.webpages.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
    }
}
