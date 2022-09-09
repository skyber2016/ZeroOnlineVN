using FwdAccServer.Properties;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace FwdAccServer
{
    public class Connected
    {
        public TcpClient Client { get; set; }
        public SimpleTcpClient ConnectToServer { get; set; }
    }
    public class Program
    {
        static SimpleTcpServer CurrentServer { get; set; }
        static Dictionary<string, Connected> Clients = new Dictionary<string, Connected>();
        static void Main(string[] args)
        {
            try
            {
                CurrentServer = new SimpleTcpServer();
                CurrentServer.ClientConnected += CurrentServer_ClientConnected;
                CurrentServer.ClientDisconnected += CurrentServer_ClientDisconnected;
                CurrentServer.DataReceived += CurrentServer_DataReceived;
                CurrentServer.Start(Settings.Default.CurrentPort);
                Console.WriteLine($"Server started on port {Settings.Default.CurrentPort}");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Main {ex.GetBaseException().Message}");
                Console.ResetColor();
            }
            Console.ReadLine();
        }

        private static void CurrentServer_DataReceived(object sender, Message e)
        {
            try
            {
                IPEndPoint remoteIpEndPoint = e.TcpClient.Client.RemoteEndPoint as IPEndPoint;
                Console.WriteLine("Client IP Address is: {0} sent data with length {1} bytes", remoteIpEndPoint.Address, e.Data.Length);
                Console.WriteLine($"Message: {e.MessageString}");
                var ip = remoteIpEndPoint.Address.ToString();
                if (Clients.TryGetValue(ip, out var _var))
                {
                    if (e.Data.Length > 136)
                    {
                        Warning($"{remoteIpEndPoint.Address} sent bytes wrong, adding to blacklist");
                        TryWithout(() =>
                        {
                            var blacklist = Settings.Default.Blacklist.Split(',').ToList();
                            blacklist.Add(ip);
                            Settings.Default["Blacklist"] = string.Join(",", blacklist);
                            Settings.Default.Save();
                            Console.WriteLine($"Added {ip} to blacklist: {Settings.Default.Blacklist}");
                        });
                        _var.Client.Client.Disconnect(false);
                    }
                    else
                    {
                        _var.ConnectToServer.Write(e.Data);
                    }
                }
                else
                {
                    Error($"CurrentServer_DataReceived not found {ip}");
                }
                
                
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"CurrentServer_DataReceived {ex.GetBaseException().Message}");
                Console.ResetColor();
            }
        }

        private static void CurrentServer_ClientDisconnected(object sender, TcpClient e)
        {
            
            try
            {
                IPEndPoint remoteIpEndPoint = e.Client.RemoteEndPoint as IPEndPoint;
                Console.WriteLine("Client IP Address is: {0} disconnected", remoteIpEndPoint.Address);
                var ip = remoteIpEndPoint.Address.ToString();
                if(Clients.TryGetValue(ip, out var _var))
                {
                    Console.WriteLine($"CurrentServer_ClientDisconnected remove {ip}");
                    try
                    {
                        if(_var.ConnectToServer.TcpClient.Connected)
                        {
                            Console.WriteLine("Disconnect to server");
                            _var.ConnectToServer.Disconnect();
                            Console.WriteLine("Connect to server is disconnected");
                        }
                    }
                    catch (Exception ex)
                    {
                        Error($"ConnectToServer.Disconnect {ex.GetBaseException().Message}");
                    }
                    Clients.Remove(ip);
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"CurrentServer_ClientDisconnected {ex.GetBaseException().Message}");
                Console.ResetColor();
            }
        }

        private static void CurrentServer_ClientConnected(object sender, TcpClient e)
        {
            try
            {
                IPEndPoint remoteIpEndPoint = e.Client.RemoteEndPoint as IPEndPoint;
                Console.WriteLine("Client IP Address is: {0} connected", remoteIpEndPoint.Address);
                var ip = remoteIpEndPoint.Address.ToString();
                bool isRefuse = false;
                TryWithout(() =>
                {
                    isRefuse = Settings.Default.Blacklist.Split(',').Contains(ip);
                    if (isRefuse)
                    {
                        Warning($"Detected IP {ip} in blacklist, refuse connection");
                        e.Client.Disconnect(false);
                    }
                    
                });
                
                if (Clients.TryGetValue(ip, out var _var))
                {
                    try
                    {
                        _var.ConnectToServer.Disconnect();
                    }
                    catch (Exception ex)
                    {
                        Error($"CurrentServer_ClientConnected {ex.GetBaseException().Message}");
                    }
                    finally
                    {
                        Clients.Remove(ip);
                    }
                }
                if (isRefuse)
                {
                    Console.WriteLine($"End-of-life {ip} because connection refuse");
                    return;
                }
                var clientConnectTo = new SimpleTcpClient();
                clientConnectTo.DataReceived += delegate (object s, Message msg)
                {
                    ClientConnectTo_DataReceived(ip, s, msg);
                };
                clientConnectTo.Connect(Settings.Default.AccServerHost, Settings.Default.AccServerPort);
                Clients.Add(ip, new Connected
                {
                    Client = e,
                    ConnectToServer = clientConnectTo
                });
            }
            catch (Exception ex)
            {
                Error($"CurrentServer_ClientConnected {ex.GetBaseException().Message}");
            }
            
        }

        private static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
        private static void Warning(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(msg);
            Console.ResetColor();
        }

        private static void TryWithout(Action action)
        {
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Error($"TryWithout {ex.GetBaseException().Message}");
            }
        }

        private static void ClientConnectTo_DataReceived(string ip, object sender, Message e)
        {
            try
            {
                Console.WriteLine("ClientConnectTo_DataReceived Client IP Address is: {0} data received", ip);
                if(Clients.TryGetValue(ip, out var _var))
                {
                    if (_var.Client.Connected)
                    {
                        _var.Client.Client.Send(e.Data);
                    }
                    else
                    {
                        Console.WriteLine($"ClientConnectTo_DataReceived TcpClient is disconnected");
                    }

                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"ClientConnectTo_DataReceived cannot found {ip}");

                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"ClientConnectTo_DataReceived {ex.GetBaseException().Message}");
                Console.ResetColor();
            }
             
        }
    }
}
