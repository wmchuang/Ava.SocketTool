using System.Net;

namespace SocketServer.EventArg;

public class SessionConnectedEventArgs : BaseEventArgs
{
    public EndPoint LocalEndPoint { get; set; }
    public EndPoint RemoteEndPoint { get; set; }
}