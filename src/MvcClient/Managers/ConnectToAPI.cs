using System.Net.Http.Headers;
using System.Text.Json;

namespace MvcClient.Managers
{
    public class ConnectToAPI : IConnectToAPI
    {

        public string address = "https://localhost:6001/identity";
        public IHttpAuthorisedConnector _connector;

        public ConnectToAPI(IHttpAuthorisedConnector connector)
        {
            _connector = connector;
        }

        public async Task<string> Connect(string accessToken)
        {
            return await _connector.Connect(accessToken, address);
        }
    }
}
