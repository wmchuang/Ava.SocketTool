namespace SocketServer;

public interface ISocketClientManager
{
    Task CreateTcpClient(SocketModel model);
    Task SendMessage(string key,string message);
}