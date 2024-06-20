namespace TcpSharp;

public class OnServerDisconnectedEventArgs : EventArgs
{
    public string ConnectionId { get; set; }
    public DisconnectReason Reason { get; set; }
}
