using System.Net.Http.Headers;
using System.Text.Json;

namespace MvcClient.Managers
{
    public interface IConnectToAPI
    {
        Task<string> Connect(string accessToken);
    }
}
