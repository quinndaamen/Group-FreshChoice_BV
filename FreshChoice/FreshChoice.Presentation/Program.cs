using FreshChoice.Data;
using FreshChoice.Services;
using FreshChoice.Services.Identity.Constants;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FreshChoice.Presentation;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(
                CookieAuthenticationDefaults.AuthenticationScheme,
                options =>
                {
                    options.LoginPath = "/login";
                    options.AccessDeniedPath = "/accessDenied";
                    options.LogoutPath = "/logout";
                });

        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy(DefaultPolicies.AdminPolicy, policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                policyBuilder.RequireRole(DefaultRoles.Admin);
            });

            options.AddPolicy(DefaultPolicies.ManagerPolicy, policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                policyBuilder.RequireRole(DefaultRoles.Manager);
            });

            options.AddPolicy(DefaultPolicies.EmployeePolicy, policyBuilder =>
            {
                policyBuilder.RequireAuthenticatedUser();
                policyBuilder.AddAuthenticationSchemes(CookieAuthenticationDefaults.AuthenticationScheme);
                policyBuilder.RequireRole(DefaultRoles.Employee);
            });
        });
        
        builder.Services.AddRazorPages();
        builder.Services.AddControllersWithViews();
        builder.Services.AddData(builder.Configuration);
        builder.Services.AddServices();

        var app = builder.Build();

        await app.PrepareAsync();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

        await app.PrepareAsync();

        app.UseHttpsRedirection();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapStaticAssets();
        app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
            .WithStaticAssets();
        app.MapRazorPages()
            .WithStaticAssets();

        await app.RunAsync();
    }
}