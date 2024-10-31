using System.Collections.Concurrent;

namespace GameServer
{
    public static class NetworkManager
    {
        public static int ProxyLoginPort = 9958;
        public static int ProxyGamePort = 5816;
        private static IDictionary<string, IClient> _clients = new ConcurrentDictionary<string, IClient>();

        public static IClient Append(IClient client)
        {
            var clientId = client.GetClientId();
            if (_clients.ContainsKey(clientId))
            {
                _clients.Remove(clientId);
            }
            _clients.TryAdd(clientId, client);
            return client;
        }

        public static void Remove(IClient client)
        {
            var clientId = client.GetClientId();
            if (_clients.ContainsKey(clientId))
            {
                _clients.Remove(clientId);
            }
        }
    }
}
