using IdentityModel;
using IdentityModel.Client;

namespace MvcClient.Managers
{
    public class ConnectToAuth : IConnectToAuth
    {
        public async Task<TokenResponse?> RequestTokenForREAuth(string code)
        {
            var client = new HttpClient();
            var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
            var request = new AuthorizationCodeTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = "mvc",
                ClientSecret = "secret",
                RedirectUri = "https://localhost:5002/Home/Callback",
                Code = code,
                GrantType = OidcConstants.GrantTypes.AuthorizationCode,
            };
            var response = await client.RequestAuthorizationCodeTokenAsync(request);
            return response;
        }
    }
}
