
namespace MvcClient.Managers
{
    public interface IConnectToLocker
    {
        Task<string> Connect(string accessToken);
    }
}