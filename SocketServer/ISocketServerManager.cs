using SocketServer.EventArg;
using SuperSocket;

namespace SocketServer;

public interface ISocketServerManager
{
    /// <summary>
    /// 数据包处理
    /// </summary>
    event EventHandler<PackageHandlerEventArgs> PackageHandler;

    /// <summary>
    /// Session Connected 
    /// </summary>
    event EventHandler<SessionConnectedEventArgs> SessionConnectedHandler;

    Task<ServerState> CreateTcpServer(SocketModel model);
    Task<ServerState?> EnableServer(string key);
    Task<ServerState?> DisableServer(string key);
}