using System.Net;
using SocketServer.EventArg;
using SocketServer.Model;

namespace SocketServer;

public interface ISocketClientManager
{
    /// <summary>
    /// 接收到数据包处理
    /// </summary>
    event EventHandler<PackageHandlerEventArgs> PackageHandler;

    /// <summary>
    /// 创建Tcp Client
    /// </summary>
    /// <param name="model"></param>
    void CreateTcpClient(SocketModel model);

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="key"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendMessage(string key, string message);

    /// <summary>
    /// 连接
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<IPEndPoint?> ConnectAsync(string key);

    /// <summary>
    /// 关闭
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task CloseAsync(string key);
}