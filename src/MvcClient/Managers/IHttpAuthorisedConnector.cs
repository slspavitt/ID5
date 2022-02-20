
namespace MvcClient.Managers
{
    public interface IHttpAuthorisedConnector
    {
        Task<string> Connect(string accessToken, string address);
    }
}