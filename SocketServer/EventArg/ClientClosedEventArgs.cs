using System.Net;

namespace SocketServer.EventArg;

public class ClientClosedEventArgs
{
    public EndPoint LocalEndPoint { get; set; }
}