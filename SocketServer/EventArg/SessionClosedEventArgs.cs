using System.Net;
using SuperSocket.Channel;

namespace SocketServer.EventArg;

public class SessionClosedEventArgs : BaseEventArgs
{
    public EndPoint RemoteEndPoint { get; set; }
    
    public CloseReason Reason { get; set; }
}