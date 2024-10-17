
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shopping.UI.Data;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<ShoppingUIContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ShoppingUIContext") ?? throw new InvalidOperationException("Connection string 'ShoppingUIContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<ShoppingUIContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    // Set the default authentication scheme to cookie-based authentication
    options.DefaultScheme = "Cookies";  // Set cookie as the default scheme
    options.DefaultChallengeScheme = "oidc";
})
.AddCookie("Cookies", options =>
{
    // Optional: Customize cookie options (like expiration)
    options.ExpireTimeSpan = TimeSpan.FromHours(8); // Set cookie expiration time
    options.SlidingExpiration = true; // Enable sliding expiration for the cookie
    
})
.AddOpenIdConnect("oidc", options =>
{
    // Set up OpenID Connect for authentication
    options.Authority = builder.Configuration["URL:Login"]; // Identity server URL
    options.ClientId = builder.Configuration["UnitTester"]; // Client ID
    options.ClientSecret = "secret"; // Client secret
    options.ResponseType = "code"; // Use authorization code flow

    // Claims and token handling
    options.GetClaimsFromUserInfoEndpoint = true; // Retrieve claims from the user info endpoint
    options.SaveTokens = true; // Save tokens in the authentication session
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name", // Map the name claim to User.Identity.Name
        RoleClaimType = "role" // Map the role claim to User.IsInRole
    };

    // Add scopes that will be requested from the identity provider
    options.Scope.Add("UnitTester");
    options.Scope.Add("openid"); // OpenID Connect scope
    options.Scope.Add("profile"); // Profile scope
    options.Scope.Add("email"); // Email scope
});
//builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication(); // Ensure this is before UseAuthorization
app.UseAuthorization();

//app.MapRazorPages().RequireAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
