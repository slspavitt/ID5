using IdentityModel.Client;

namespace MvcClient.Managers
{
    public interface IConnectToAuth
    {
        Task<TokenResponse?> RequestTokenForREAuth(string code);
    }
}
