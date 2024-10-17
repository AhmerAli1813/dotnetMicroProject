using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.IdentityModel.Logging;

var builder = WebApplication.CreateBuilder(args);

// Enable PII logging for debugging
IdentityModelEventSource.ShowPII = true;

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure authentication with cookie and OIDC
builder.Services.AddAuthentication(options =>
{
    // Cookie-based authentication as the default scheme
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    // OpenID Connect (OIDC) will be used for challenges when authentication is needed
    options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme) // Adds cookie support
.AddOpenIdConnect(OpenIdConnectDefaults.AuthenticationScheme, options =>
{
    // Set IdentityServer as the authority
    options.Authority = "https://localhost:7088"; // URL of your Duende IdentityServer

    // Client ID configured in IdentityServer (from CommonIdentityDetails class)
    options.ClientId = "UnitTester"; // Replace with the actual client ID

    // Client secret configured in IdentityServer (from CommonIdentityDetails class)
    options.ClientSecret = "secret"; // Replace with the actual client secret

    // Use authorization code flow (recommended)
    options.ResponseType = "code";

    // Configure scopes to be requested from IdentityServer
    options.Scope.Add("UnitTester");  // Custom API scope
    options.Scope.Add("openid");      // Standard OpenID Connect scopes
    options.Scope.Add("profile");
    options.Scope.Add("email");

    // Save the tokens received from IdentityServer (access token, ID token, etc.)
    options.SaveTokens = true;

    // Configure token validation parameters
    options.TokenValidationParameters = new TokenValidationParameters
    {
        NameClaimType = "name",
        RoleClaimType = "role"
    };

    // Handle redirect URIs (must match with what is configured in IdentityServer)
    options.Events.OnRedirectToIdentityProvider = context =>
    {
        context.ProtocolMessage.RedirectUri = "https://localhost:7054/signin-oidc"; // Redirect URI for your client app
        return Task.CompletedTask;
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middleware configuration
app.UseHttpsRedirection();  // Redirect HTTP requests to HTTPS
app.UseStaticFiles();        // Serve static files

app.UseRouting();            // Enable routing

// Enable authentication and authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// Map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// Run the app
app.Run();
