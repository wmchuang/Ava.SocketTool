using System.Collections.Concurrent;
using System.Net;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocketServer.EventArg;
using SocketServer.Model;
using SocketServer.Socket;
using SuperSocket;
using SuperSocket.Channel;
using SuperSocket.Client;
using SuperSocket.ProtoBase;
using SuperSocket.Server;

namespace SocketServer;

public class SocketServerManager : ISocketServerManager
{
    private static ConcurrentDictionary<string, IServer> _tcpServer = new();
    private static CancellationTokenSource _cts = new();

    /// <summary>
    /// 数据包处理
    /// </summary>
    public event EventHandler<PackageHandlerEventArgs> PackageHandler;

    /// <summary>
    /// Session Connected 
    /// </summary>
    public event EventHandler<SessionConnectedEventArgs> SessionConnectedHandler;

    /// <summary>
    /// Session Close 
    /// </summary>
    public event EventHandler<SessionClosedEventArgs> SessionClosedHandler;

    public async Task<bool> CreateServer(SocketModel model, bool isTcpServer)
    {
        //创建宿主：用Package的类型和PipelineFilter的类型创建SuperSocket宿主。
        var build = SuperSocketHostBuilder.Create<TextPackageInfo, MyPipelineFilter>()
            .UseHostedService<MyService>()
            .UseSession<MySession>()
            //注册用于处理连接、关闭的Session处理器
            .UseSessionHandler(async (session) =>
            {
                SessionConnectedHandler?.Invoke(this, new SessionConnectedEventArgs()
                {
                    IsTcpServer = isTcpServer,
                    ServerId = session.Server.Name,
                    SessionID = session.SessionID,
                    RemoteEndPoint = session.RemoteEndPoint,
                    LocalEndPoint = session.LocalEndPoint
                });
                await Task.Delay(0);
            }, async (session, reason) =>
            {
                SessionClosedHandler?.Invoke(this, new SessionClosedEventArgs()
                {
                    IsTcpServer = isTcpServer,
                    ServerId = session.Server.Name,
                    SessionID = session.SessionID,
                    RemoteEndPoint = session.RemoteEndPoint,
                    Reason = reason.Reason
                });
                await Task.Delay(0);
            })

            //注册用于处理接收到的数据的包处理器
            .UsePackageHandler((session, package) =>
            {
                PackageHandler?.Invoke(this, new PackageHandlerEventArgs()
                {
                    IsTcpServer = isTcpServer,
                    ServerId = session.Server.Name,
                    SessionID = session.SessionID,
                    Message = package.Text
                });
                return ValueTask.CompletedTask;
            })
            //配置服务器如服务器名和监听端口等基本信息
            .ConfigureSuperSocket(options =>
            {
                options.Name = model.Id;
                options.Listeners = new[]
                {
                    new ListenOptions
                    {
                        Ip = model.LocalEndPoint.Address.ToString(),
                        Port = model.LocalEndPoint.Port
                    }
                }.ToList();
            })
            .UseInProcSessionContainer();
        if (!isTcpServer)
            build.UseUdp();
        var server = build.BuildAsServer();

        _tcpServer.TryAdd(model.Key, server);
        await server.StartAsync();

        var service = server.ServiceProvider.GetService<MyService>();
        return service.State == ServerState.Started;
    }

    public async Task<bool> StartListen(string key)
    {
        if (_tcpServer.TryGetValue(key, out var server))
        {
            _cts = new CancellationTokenSource();
            var service = server.ServiceProvider.GetService<MyService>();
            await service.StartAsync(_cts.Token);
            return service.State == ServerState.Started;
        }

        return false;
    }

    public async Task<bool> StopListen(string key)
    {
        if (_tcpServer.TryGetValue(key, out var server))
        {
            _cts = new CancellationTokenSource();
            var service = server.ServiceProvider.GetService<MyService>();
            await service.StopAsync(_cts.Token);
            return service.State == ServerState.Stopped;
        }

        return false;
    }

    public async Task<bool> CloseSession(IPEndPoint remoteIpEndPoint, string sessionId)
    {
        var matchData = _tcpServer.FirstOrDefault(x => x.Key.Contains(remoteIpEndPoint.ToString()));
        if (!string.IsNullOrEmpty(matchData.Key))
        {
            var service = matchData.Value.ServiceProvider.GetService<MyService>();
            await service.CloseSessionAsync(sessionId);
            return true;
        }

        return false;
    }

    public async Task<bool> SendMessage(IPEndPoint remoteIpEndPoint, string message, string sessionId)
    {
        var matchData = _tcpServer.FirstOrDefault(x => x.Key.Contains(remoteIpEndPoint.ToString()));
        if (!string.IsNullOrEmpty(matchData.Key))
        {
            var service = matchData.Value.ServiceProvider.GetService<MyService>();
            await service.SendMessageAsync(message, sessionId);
            return true;
        }

        return false;
    }

    public async Task RemoveServer(string key)
    {
        if (_tcpServer.TryRemove(key, out var server))
        {
            _cts = new CancellationTokenSource();
            var service = server.ServiceProvider.GetService<MyService>();
            await service.StopAsync(_cts.Token);
        }
    }
}