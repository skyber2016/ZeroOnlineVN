
namespace GameServer
{
    public interface IClient : IDisposable
    {
        Task BeginTransferAsync();
        public string GetClientId();
    }
}
