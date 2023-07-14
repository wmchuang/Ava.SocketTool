using System.Net;
using SocketServer.EventArg;
using SocketServer.Model;
using SuperSocket;

namespace SocketServer;

public interface ISocketServerManager
{
    /// <summary>
    /// 接收到数据包处理
    /// </summary>
    event EventHandler<PackageHandlerEventArgs> PackageHandler;

    /// <summary>
    /// 客户端连接时处理
    /// </summary>
    event EventHandler<SessionConnectedEventArgs> SessionConnectedHandler;
    
    /// <summary>
    /// 客户端断开时处理
    /// </summary>
    event EventHandler<SessionClosedEventArgs> SessionClosedHandler;

    /// <summary>
    /// 创建Tcp Server
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    Task<bool> CreateTcpServer(SocketModel model);

    /// <summary>
    /// 启动监听
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<bool> StartListen(string key);

    /// <summary>
    /// 停止监听
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task<bool> StopListen(string key);

    /// <summary>
    /// 删除服务
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    Task RemoveServer(string key);

    /// <summary>
    /// 关闭Client连接
    /// </summary>
    /// <param name="remoteIpEndPoint"></param>
    /// <param name="sessionId"></param>
    /// <returns></returns>
    Task<bool> CloseSession(IPEndPoint remoteIpEndPoint,string sessionId);

    /// <summary>
    /// 给Client发送消息
    /// </summary>
    /// <param name="remoteIpEndPoint"></param>
    /// <param name="sessionId"></param>
    /// <param name="message"></param>
    /// <returns></returns>
    Task<bool> SendMessage(IPEndPoint remoteIpEndPoint, string sessionId, string message);
}