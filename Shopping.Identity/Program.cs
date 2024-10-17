using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shopping.Identity.Data;
using Shopping.Identity.DataSeeding;
using Shopping.Identity.Helper;
using Shopping.Identity.Models;
using Duende.IdentityServer;
using Shopping.Identity.Services;
using Duende.IdentityServer.Services;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllersWithViews();

        // Configure EF Core with SQL Server
        builder.Services.AddDbContext<ShoppingIdentityDbContext>(options =>
        {
            options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
        });

        // Configure Identity with EF Core stores and token providers
        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ShoppingIdentityDbContext>()
            .AddDefaultTokenProviders();

        // Configure IdentityServer
        builder.Services.AddIdentityServer(options =>
        {
            options.Events.RaiseSuccessEvents = true;
            options.Events.RaiseErrorEvents = true;
            options.Events.RaiseInformationEvents = true;
            options.Events.RaiseFailureEvents = true;
            options.EmitStaticAudienceClaim = true;
        })
        .AddInMemoryIdentityResources(CommonIdentityDetails.GetIdentityResources) // Add identity resources
        .AddInMemoryApiScopes(CommonIdentityDetails.GetApiScopes) // Add API scopes
        .AddInMemoryClients(CommonIdentityDetails.Clients) // Add clients
        .AddAspNetIdentity<ApplicationUser>() // Integrate ASP.NET Identity
        .AddDeveloperSigningCredential().AddProfileService<ProfileService>(); // Use a dev signing credential (for development)
        builder.Services.AddScoped<IProfileService, ProfileService>();
        // Register database initializer
        builder.Services.AddScoped<IDbInitializer, DbInitializer>();

        // Add CORS policy to allow requests from the UI project
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigins",
                builder =>
                {
                    builder.WithOrigins("https://localhost:7054") // Client app URL
                           .AllowAnyHeader()
                           .AllowAnyMethod();
                });
        });

        // Add Razor Pages for Identity UI
        builder.Services.AddRazorPages();

        var app = builder.Build();

        // Apply CORS policy
        app.UseCors("AllowSpecificOrigins");

        // Configure the HTTP request pipeline
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts(); // Enable HSTS in production
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        // Use IdentityServer for authentication and authorization
        app.UseIdentityServer();

        // Apply authentication and authorization middleware
        app.UseAuthentication();
        app.UseAuthorization();

        // Map Razor Pages for the Identity UI
        app.MapRazorPages().RequireAuthorization();

        // Configure routing for MVC controllers
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");

        // Run the application
        app.Run();
    }
}
