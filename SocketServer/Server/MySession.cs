using SuperSocket.Channel;
using SuperSocket.Server;

namespace SocketServer.Server;

public class MySession : AppSession
{
    protected override ValueTask OnSessionConnectedAsync()
    {
        Console.WriteLine($"{DateTime.Now} [SessionHandler] [{ this.Server.Name}] Session connected: {RemoteEndPoint}");
       
        return base.OnSessionConnectedAsync();
    }

    protected override ValueTask OnSessionClosedAsync(CloseEventArgs e)
    {
        Console.WriteLine($"{DateTime.Now} [SessionHandler] [{ this.Server.Name}] Session {RemoteEndPoint} closed: {e.Reason}");
        return base.OnSessionClosedAsync(e);
    }
}