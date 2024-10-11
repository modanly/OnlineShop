using Microsoft.AspNetCore.Identity;
using Microsoft.Identity.Client;
using OnlineShop.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Db
{
    public class IdentityInitializer
    {
        public static async Task Initialize(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminEmail = "admin@gmail.com";
            var password = "Hello123mister!";
            if (await roleManager.FindByNameAsync(Constans.AdminRoleName) == null)
            {
               await roleManager.CreateAsync(new IdentityRole(Constans.AdminRoleName));
            }
            if (await roleManager.FindByNameAsync(Constans.UserRoleName) == null)
            {
               await roleManager.CreateAsync(new IdentityRole(Constans.UserRoleName));
            }
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                var admin = new User { Email = adminEmail, UserName = adminEmail };
                var result = await userManager.CreateAsync(admin, password);
                if(result.Succeeded)
                {
                   await userManager.AddToRoleAsync(admin, Constans.AdminRoleName);
                }
            }

        }
    }
}
