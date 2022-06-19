using Etut.Data;
using Etut.Models.DataModels;
using Etut.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Etut.Models
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetService<UserManager<ApplicationUser>>();
            RoleManager<IdentityRole> roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

            ApplicationDbContext dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

            try
            {
                if(dbContext.Database.GetPendingMigrations().Count()>0)
                {
                    dbContext.Database.Migrate();
                }
            }
            catch(Exception ex)
            {

            }

            if (dbContext.Roles.Any(x => x.Name == Helper.Admin)) return;

            roleManager.CreateAsync(new IdentityRole(Helper.Admin)).GetAwaiter().GetResult();
            roleManager.CreateAsync(new IdentityRole(Helper.Student)).GetAwaiter().GetResult();

            string currentTime = DateTime.Now.ToString("hh:mm:ss");
            string currentDate = DateTime.Now.ToString("dd/MM/yy");

            userManager.CreateAsync(new ApplicationUser {
                FirstName = "Super",
                LastName = "Admin",
                Email = "super.admin@gmail.com",
                UserName = "super.admin@gmail.com",
                UserCreationDate = currentDate,
                UserCreationTime = currentTime,
                IsAdminApproved = true,

            }, ConfigurationHelper.config["gs-etut-super-admin-password"]).GetAwaiter().GetResult();

            ApplicationUser adminUser = dbContext.Users.FirstOrDefault(u => u.Email == "super.admin@gmail.com");
            userManager.AddToRoleAsync(adminUser, Helper.Admin).GetAwaiter().GetResult();
        }
    }
}
