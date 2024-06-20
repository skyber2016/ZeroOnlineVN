using log4net;
using System.Collections.Concurrent;
using TcpSharp;

namespace ServerForward
{
    public class ProxyServer
    {
        private readonly TcpSharpSocketServer _proxyServer;
        private readonly ILog _logger = LogManager.GetLogger(typeof(ProxyServer));
        private readonly IDictionary<string, ProxyClient> _clients = new ConcurrentDictionary<string, ProxyClient>();
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        public CancellationToken CancellationToken => _cancellationTokenSource.Token;
        private Options _options => Options.Instance;
        public ProxyServer()
        {
            _proxyServer = new TcpSharpSocketServer(_options.LocalPort);
            _proxyServer.OnConnected += _proxyServer_OnConnected;
            _proxyServer.OnConnectionRequest += _proxyServer_OnConnectionRequest;
            _proxyServer.OnDisconnected += _proxyServer_OnDisconnected;
            _proxyServer.OnDataReceived += _proxyServer_OnDataReceived;
            _proxyServer.OnError += _proxyServer_OnError;
            _proxyServer.OnStarted += _proxyServer_OnStarted;
            _proxyServer.OnStopped += _proxyServer_OnStopped;
        }

        public void Start()
        {
            _proxyServer.StartListening();
        }
        public void Stop()
        {
            _proxyServer.StopListening();
            _cancellationTokenSource.Cancel();
        }

        public bool IsListening => !CancellationToken.IsCancellationRequested;

        private void _proxyServer_OnStopped(object sender, OnServerStoppedEventArgs e)
        {
            _logger.Warn($"Server stopping...");
        }

        private void _proxyServer_OnStarted(object sender, OnServerStartedEventArgs e)
        {
            _logger.Info($"Server started on port {_options.LocalPort}");
        }

        private void _proxyServer_OnError(object sender, OnServerErrorEventArgs e)
        {
            _logger.Error("Server error", e.Exception);
        }

        private void _proxyServer_OnDataReceived(object sender, OnServerDataReceivedEventArgs e)
        {
            _logger.Info($"C2S: {e.Data.vnToString()}");
            if (_clients.TryGetValue(e.ConnectionId, out var client))
            {
                e.Data.WriteToFileHex(e.ConnectionId, Channel.C2S);
                client.SendToGameServer(e.Data);
            }
            else
            {
                _logger.Error($"Could not found connection_id={e.ConnectionId}");
            }
        }

        private void _proxyServer_OnDisconnected(object sender, OnServerDisconnectedEventArgs e)
        {
            _logger.Info($"Client reason={e.Reason} connection_id={e.ConnectionId} disconnected");

            if (_clients.TryGetValue(e.ConnectionId, out var client))
            {
                client.Disconnect();
                _clients.Remove(e.ConnectionId);
            }
            else
            {
                _logger.Error($"Could not found connection_id={e.ConnectionId}");
            }
        }

        private void _proxyServer_OnConnectionRequest(object sender, OnServerConnectionRequestEventArgs e)
        {
            _logger.Info("Server Connection Request");
        }

        private void _proxyServer_OnConnected(object sender, OnServerConnectedEventArgs e)
        {
            if (!_clients.TryGetValue(e.ConnectionId, out var client))
            {
                client = new ProxyClient(_proxyServer.GetClient(e.ConnectionId));
                _clients.Add(e.ConnectionId, client);
            }
            else
            {
                _logger.Error($"Connection_id={e.ConnectionId} is existed");
            }
            client.Connect();
            _logger.Info($"Client={e.ConnectionId} {e.IPAddress}:{e.Port} connected. COUNT={_clients.Count()}");

            Task.Run(async () =>
            {
                while (true)
                {
                    var currentClient = _proxyServer.GetClient(e.ConnectionId);
                    if (currentClient == null)
                    {
                        break;
                    }
                    if (!currentClient.Connected || !currentClient.Client.Connected || !currentClient.Client.Client.Connected)
                    {
                        this._proxyServer_OnDisconnected(this, new OnServerDisconnectedEventArgs()
                        {
                            ConnectionId = e.ConnectionId,
                            Reason = DisconnectReason.ServerAborted
                        });
                        break;
                    }
                    await Task.Delay(1000);
                }
            });
        }
    }
}
