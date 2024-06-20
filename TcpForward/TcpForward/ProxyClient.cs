using log4net;
using Newtonsoft.Json;
using ServerForward.Data;
using System.Runtime.InteropServices;
using TcpSharp;

namespace ServerForward
{
    public class ProxyClient
    {
        private readonly ConnectedClient _game;
        private readonly TcpSharpSocketClient _serverGame;
        private readonly ILog _logger = LogManager.GetLogger(typeof(ProxyClient));
        private Options _options => Options.Instance;
        public ProxyClient(ConnectedClient client)
        {
            this._game = client;
            _serverGame = new TcpSharpSocketClient(_options.RemoteHost, _options.RemotePort);
            _serverGame.OnConnected += _serverGame_OnConnected;
            _serverGame.OnDataReceived += _serverGame_OnDataReceived;
            //_serverGame.OnDisconnected += _serverGame_OnDisconnected;
        }

        private void _serverGame_OnDisconnected(object sender, OnClientDisconnectedEventArgs e)
        {
            this.Disconnect();
        }

        private void _serverGame_OnDataReceived(object sender, OnClientDataReceivedEventArgs e)
        {
            _logger.Info($"S2C->{_game.ConnectionId}: {e.Data.vnToString()}");
            e.Data.WriteToFileHex(_game.ConnectionId, Channel.S2C);
            if (e.Data.Length == 0x34)
            {
                LoginResponseData loginData;
                byte[] serverData = e.Data;
                unsafe
                {
                    fixed (byte* buffer = serverData)
                    {
                        loginData = Marshal.PtrToStructure<LoginResponseData>((IntPtr)buffer);
                        _logger.Info($"Decompiled data {JsonConvert.SerializeObject(loginData)}");
                    }
                }
                var isLoginSuccess = loginData.AccountId != 0;
                if (isLoginSuccess)
                {
                    var gameIP = _game.Client.Client.RemoteEndPoint.ToString().Split(":").FirstOrDefault();
                    var canLogin = CanLogin(gameIP);
                    canLogin.Wait();
                    if (canLogin.Result)
                    {
                        _logger.Info($"IP={gameIP} login success");
                        _game.SendBytes(e.Data);
                    }
                    else
                    {
                        _logger.Error($"IP={gameIP} is max connect client");
                        this.Disconnect();
                    }
                }
                else
                {
                    _logger.Error("Login failed");
                    _game.SendBytes(e.Data);
                }
            }
            else
            {
                _game.SendBytes(e.Data);
            }
        }

        private void _serverGame_OnConnected(object sender, OnClientConnectedEventArgs e)
        {
            _logger.Info($"Proxy client connected to server game {e.ServerIPAddress}");
        }

        public void Connect()
        {
            _serverGame.Connect();
        }

        public void SendToGameServer(byte[] bytes)
        {
            this._serverGame.SendBytes(bytes);
        }

        public void Disconnect()
        {
            if (_serverGame.Connected)
            {
                _logger.Info("Disconnecting server game...");
                _serverGame.Disconnect();
            }
            if (_game.Connected)
            {
                _logger.Info("Disconnecting client game...");
                _game.Client.Dispose();
            }

        }

        private async Task<bool> CanLogin(string ipAddress)
        {
            using var http = new HttpClient();
            var res = await http.GetAsync($"{_options.HostConnection}?ip={ipAddress}&port={_options.RemotePort}");
            if (res.IsSuccessStatusCode)
            {
                var content = await res.Content.ReadAsStringAsync();
                var jsonText = content;
                _logger.Info($"Can login: {jsonText}");
                var responseData = JsonConvert.DeserializeObject<ConnectionData>(jsonText);
                return responseData.Count < _options.MaxConnect;
            }
            return true;
        }
    }
}
