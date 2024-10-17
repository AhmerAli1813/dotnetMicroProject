using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Shopping.Identity.Helper
{
    public class CommonIdentityDetails
    {
       
        public static IEnumerable<IdentityResource> GetIdentityResources =>

             new List<IdentityResource>
                {
                    new IdentityResources.OpenId(),
                    new IdentityResources.Email(),
                    new IdentityResources.Profile(),
                };
        public static IEnumerable<ApiScope> GetApiScopes =>

             new List<ApiScope>
                {
                    new ApiScope(name: "UnitTester",   displayName: "Unit Test."),
                    new ApiScope("ShoppingAPI", "API for Shopping Services"),
                    new ApiScope(name: "read",   displayName: "Read your data."),
                    new ApiScope(name: "write",  displayName: "Write your data."),
                    new ApiScope(name: "delete", displayName: "Delete your data."),

                };
        public static IEnumerable<Client> Clients =>

             new List<Client>
        {
            new Client
            {
                ClientId = "service.client",
                ClientSecrets = { new Secret("secret".Sha256()) },

                AllowedGrantTypes = GrantTypes.ClientCredentials,

            },
            new Client
                                {
                                    ClientId = "UnitTester",

                                    AllowedGrantTypes = GrantTypes.Code,
                                    ClientSecrets = { new Secret("secret".Sha256()) },

                                    RedirectUris =           { "https://localhost:7054/signin-oidc" },
                                    PostLogoutRedirectUris = { "https://localhost:7054/signout-callback-oidc" },


                                    AllowedScopes =
                                    {"UnitTester",
                                        IdentityServerConstants.StandardScopes.OpenId,
                                        IdentityServerConstants.StandardScopes.Profile,
                                        IdentityServerConstants.StandardScopes.Email,

                                    },
                                },
                      new Client
                    {
                        ClientId = "ShoppingAPIClient",
                        AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                        ClientSecrets =
                        {
                            new Secret("secret".Sha256())
                        },
                        AllowedScopes = { "ShoppingAPI", "openid", "profile" },
                        // Other properties as needed
                    }
        };



    }

}
