using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using IdentityModel;

namespace IdentityServer;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        { 
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            new IdentityResource()
            {
                Name = "verification",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.Email,
                    JwtClaimTypes.EmailVerified
                }
            },
            new IdentityResource()
            {
                Name = "phone",
                UserClaims = new List<string>
                {
                    JwtClaimTypes.PhoneNumber,
                }
            }

        };

    public static IEnumerable<ApiScope> ApiScopes =>
    new List<ApiScope>
    {
        new ApiScope(name: "api1", displayName: "MyAPI")
    };

    public static IEnumerable<Client> Clients =>
    new List<Client>
    {
        new Client
        {
            ClientId = "client",

            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            // secret for authentication
            ClientSecrets =
            {
                new Secret("secret".Sha256())
            },

            // scopes that client has access to
            AllowedScopes = { "api1" }
        },
        new Client
        {
            ClientId = "mvc",
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedGrantTypes = GrantTypes.Code,
            
            // where to redirect to after login
            RedirectUris = { "https://localhost:5002/signin-oidc","https://localhost:5002/Home/Callback" },

            // where to redirect to after logout
            PostLogoutRedirectUris = { "https://localhost:5002/signout-callback-oidc" },

            AllowedScopes = new List<string>
            {
                IdentityServerConstants.StandardScopes.OpenId,
                IdentityServerConstants.StandardScopes.Profile,
                "api1",
                "verification",
                "phone"
            },
            RequirePkce = false,
            AllowOfflineAccess = true,
            AlwaysIncludeUserClaimsInIdToken = true,
            AllowAccessTokensViaBrowser = false,

        }
    };
}