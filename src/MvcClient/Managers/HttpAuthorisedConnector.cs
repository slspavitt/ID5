using System.Net.Http.Headers;
using System.Text.Json;

namespace MvcClient.Managers
{
    public class HttpAuthorisedConnector : IHttpAuthorisedConnector
    {
        public async Task<string> Connect(string accessToken, string address)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync(address);

            var parsed = JsonDocument.Parse(content);
            var formatted = JsonSerializer.Serialize(parsed, new JsonSerializerOptions { WriteIndented = true });
            return formatted;
        }
    }
}
