using System.Net;

namespace SocketServer.EventArg;

public class SessionConnectedEventArgs : BaseEventArgs
{
    public EndPoint RemoteEndPoint { get; set; }
}