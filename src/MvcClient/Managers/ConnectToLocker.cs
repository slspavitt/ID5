using System.Net.Http.Headers;
using System.Text.Json;

namespace MvcClient.Managers
{
    public class ConnectToLocker : IConnectToLocker
    {

        public string address = "https://localhost:6003/passport";
        public IHttpAuthorisedConnector _connector;

        public ConnectToLocker(IHttpAuthorisedConnector connector)
        {
            _connector = connector;
        }

        public async Task<string> Connect(string accessToken)
        {
            var str = await _connector.Connect(accessToken, address);
            return str;
        }
    }
}
