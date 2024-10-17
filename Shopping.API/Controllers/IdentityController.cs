using Azure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shopping.API.Entities;
using System.ComponentModel.DataAnnotations;

namespace Shopping.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public IdentityController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        [HttpPost("Token")]
        public async Task<IActionResult> Token(UserCredential model)
        {
            var tokenEndpoint = _configuration.GetSection("URL")["Login"] + "connect/token";

            var tokenRequestParams = new Dictionary<string, string>
        {
            { "grant_type", "password" },
            { "client_id", "ShoppingAPIClient" },          // Client ID registered with IdentityServer
            { "client_secret", "secret" },  // The plain client secret
            { "username", model.userName },  // User's username
            { "password", model.password },  // User's password
            { "scope", "ShoppingAPI openid profile" }  // Required scopes
        };

            var requestContent = new FormUrlEncodedContent(tokenRequestParams);

            // Create HttpClient instance from factory
            var client = _httpClientFactory.CreateClient();

            HttpResponseMessage response = await client.PostAsync(tokenEndpoint, requestContent);

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = await response.Content.ReadAsStringAsync();
                return Ok(tokenResponse); // Return the token response
            }
            else
            {
                return BadRequest($"Error retrieving token: {response.StatusCode}");
            }
        }


       
    }
}
