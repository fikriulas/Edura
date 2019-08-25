using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.IdentityCore
{
    public static class SeedIdentity
    {
        public static async Task CreateIdentityUsers(IServiceProvider serviceProvider,IConfiguration configuration)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var username = configuration["Data:AdminUser:username"];
            var email = configuration["Data:AdminUser:email"];
            var password = configuration["Data:AdminUser:password"];
            var role = configuration["Data:AdminUser:role"];

            if(await userManager.FindByNameAsync(username)==null) // kullanıcı daha önceden var mı? 
            {
                if(await roleManager.FindByNameAsync(role)==null) // rol daha önceden var mı?
                {
                    await roleManager.CreateAsync(new IdentityRole(role)); // role oluştu
                }
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = username,
                    Email = email,
                    Name ="Ulaş",
                    SurName ="Osmanağaoğlu"
               
                };
                IdentityResult result = await userManager.CreateAsync(user, password); // kullanıcı oluştur.
                if(result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, role); // role ata.
                }
            }
        }
    }
}
