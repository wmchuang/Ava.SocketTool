using SocketServer.Model;

namespace SocketServer;

public interface ISocketClientManager
{
    Task<SocketModel> CreateTcpClient(SocketModel model);
    Task SendMessage(string key,string message);
}