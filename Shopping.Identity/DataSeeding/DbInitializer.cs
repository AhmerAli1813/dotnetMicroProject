using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping.Identity.Data;
using Shopping.Identity.Helper;
using Shopping.Identity.Models;
using static Shopping.Identity.Enum.Enums;

namespace Shopping.Identity.DataSeeding
{
    public class DbInitializer : IDbInitializer
    {
        private UserManager<ApplicationUser> _userManager;
        private RoleManager<IdentityRole> _roleManger;
        private ShoppingIdentityDbContext _context;

        public DbInitializer(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManger, ShoppingIdentityDbContext context)
        {
            _userManager = userManager;
            _roleManger = roleManger;
            _context = context;
        }

        public void Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {


                    _context.Database.Migrate();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            if (!_roleManger.RoleExistsAsync(userRole.superAdmin.ToString()).GetAwaiter().GetResult())
            {
                _roleManger.CreateAsync(new IdentityRole(userRole.superAdmin.ToString() )).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(userRole.admin.ToString())).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(userRole.manager.ToString())).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(userRole.customer.ToString())).GetAwaiter().GetResult();
                _roleManger.CreateAsync(new IdentityRole(userRole.appUser.ToString())).GetAwaiter().GetResult();
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@outlook.com",
                    PhoneNumber = "1234567890",
                    EmailConfirmed = true,
                    FatherName = "Afzal",
                    

                }, "Admin@123").GetAwaiter().GetResult();
                ApplicationUser user = _context.ApplicationUsers.Where(x => x.Email == "admin@outlook.com").FirstOrDefault();
                _userManager.AddToRoleAsync(user, userRole.admin.ToString()).GetAwaiter().GetResult();
            }
        }
    }
}
