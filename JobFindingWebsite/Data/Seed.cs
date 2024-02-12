using JobFindingWebsite.Data;
using Microsoft.AspNetCore.Identity;
using JobFindingWebsite.Data.Enum;
using JobFindingWebsite.Models;
using System.Diagnostics;
using System.Net;
using Microsoft.EntityFrameworkCore;

namespace RunGroopWebApp.Data
{
    public class Seed
    {

        public static async Task SeedUsersAndRolesAsync(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

                if (!await roleManager.RoleExistsAsync(Roles.User))
                    await roleManager.CreateAsync(new IdentityRole(Roles.User));
                if (!await roleManager.RoleExistsAsync(Roles.Company))
                    await roleManager.CreateAsync(new IdentityRole(Roles.Company));
                if (!await roleManager.RoleExistsAsync(Roles.Admin))
                    await roleManager.CreateAsync(new IdentityRole(Roles.Admin));

                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<Account>>();

                string adminUserEmail = "admingiorgi@gmail.com";

                var adminUser = await userManager.FindByEmailAsync(adminUserEmail);
                if (adminUser == null)
                {
                    var newAdminUser = new AppUser()
                    {
                        UserName = "GiorgiAdmin",
                        Email = adminUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAdminUser, "Pass123!");
                    await userManager.AddToRoleAsync(newAdminUser, Roles.Admin);
                }

                string appUserEmail = "user1@gmail.com";

                var appUser = await userManager.FindByEmailAsync(appUserEmail);
                if (appUser == null)
                {
                    var newAppUser = new AppUser()
                    {
                        UserName = "user1",
                        Email = appUserEmail,
                        EmailConfirmed = true
                    };
                    await userManager.CreateAsync(newAppUser, "Pass123!");
                    await userManager.AddToRoleAsync(newAppUser, Roles.User);
                }

                string companyEmail = "company1@gmail.com";
                var companyUser = await userManager.FindByEmailAsync(companyEmail);
                if(companyUser == null)
                {
                    var newCompany = new Company()
                    {
                        UserName = "Company 1",
                        Email = companyEmail,
                        EmailConfirmed = true,
                        isCompany = true
                    };
                    await userManager.CreateAsync(newCompany, "Pass123!");
                    await userManager.AddToRoleAsync(newCompany, Roles.Company);
                }
                
            }
        }
    }
}