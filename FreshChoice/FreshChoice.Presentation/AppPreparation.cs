using FreshChoice.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using FreshChoice.Data.Entities;
using System;
using FreshChoice.Services.Identity.Constants;

namespace FreshChoice.Presentation;

public static class AppPreparation
{
    public static async Task PrepareAsync(this IApplicationBuilder app)
    {
        try
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            await dbContext.Database.MigrateAsync();

            if (!await dbContext.Roles.AnyAsync())
            {
                using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                foreach (var role in DefaultRoles.AllRoles)
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>
                    {
                        Name = role,
                    });
                }
            }

            if (!await dbContext.Users.AnyAsync())
            {
                using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var user = new ApplicationUser
                {
                    UserName = InitialAdminCredentials.AdminUsername,
                    Email = InitialAdminCredentials.AdminEmail,
                    EmailConfirmed = true,
                    FirstName = "John",
                    LastName = "Doe",
                };
                var adminCreateResult = await userManager.CreateAsync(user, InitialAdminCredentials.AdminPassword);

                if (adminCreateResult.Succeeded) ;
                {
                    await userManager.AddToRoleAsync(user, DefaultRoles.Admin);
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}