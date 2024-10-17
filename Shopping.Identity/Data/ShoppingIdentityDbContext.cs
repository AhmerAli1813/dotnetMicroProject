using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Shopping.Identity.Models;

namespace Shopping.Identity.Data
{
    public class ShoppingIdentityDbContext :IdentityDbContext<ApplicationUser>
    {
        public ShoppingIdentityDbContext(DbContextOptions<ShoppingIdentityDbContext> options ) :base(options)
        {
            
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    }
}
