﻿using System.Collections.Concurrent;
using System.Net;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocketServer.EventArg;
using SocketServer.Server;
using SuperSocket;
using SuperSocket.Channel;
using SuperSocket.Client;
using SuperSocket.ProtoBase;
using SuperSocket.Server;

namespace SocketServer;

public class SocketServerManager : ISocketServerManager
{
    private static ConcurrentDictionary<string, IServer> _tcpServer = new ConcurrentDictionary<string, IServer>();
    private static SocketServerManager _instance;

    public static SocketServerManager Instance => _instance ??= new SocketServerManager();

    private static CancellationTokenSource _cts = new();

    /// <summary>
    /// 数据包处理
    /// </summary>
    public event EventHandler<PackageHandlerEventArgs> PackageHandler;

    /// <summary>
    /// Session Connected 
    /// </summary>
    public event EventHandler<SessionConnectedEventArgs> SessionConnectedHandler;

    public async Task<ServerState> CreateTcpServer(SocketModel model)
    {
        //创建宿主：用Package的类型和PipelineFilter的类型创建SuperSocket宿主。
        var server = SuperSocketHostBuilder.Create<TextPackageInfo, MyPipelineFilter>()
            .UseHostedService<MyService>()
            .UseSession<MySession>()
            //注册用于处理连接、关闭的Session处理器
            .UseSessionHandler(async (session) =>
            {
                SessionConnectedHandler?.Invoke(this, new SessionConnectedEventArgs()
                {
                    ServerId = session.Server.Name,
                    SessionID = session.SessionID,
                    RemoteEndPoint = session.RemoteEndPoint
                });
                await Task.Delay(0);
            }, async (session, reason) =>
            {
                Console.WriteLine($"{DateTime.Now} [SessionHandler] Session {session.RemoteEndPoint} closed: {reason}");
                await Task.Delay(0);
            })

            //注册用于处理接收到的数据的包处理器
            .UsePackageHandler((session, package) =>
            {
                PackageHandler?.Invoke(this, new PackageHandlerEventArgs()
                {
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
                        Ip = model.Ip,
                        Port = model.Port
                    }
                }.ToList();
            })
            .UseInProcSessionContainer()
            .BuildAsServer();

        _tcpServer.TryAdd(model.Key, server);
        await server.StartAsync();

        var service = server.ServiceProvider.GetService<MyService>();
        return service.State;
    }

    public async Task<ServerState?> EnableServer(string key)
    {
        if (_tcpServer.TryGetValue(key, out var server))
        {
            _cts = new CancellationTokenSource();
            var service = server.ServiceProvider.GetService<MyService>();
            await service.StartAsync(_cts.Token);
            return service.State;
        }

        return null;
    }

    public async Task<ServerState?> DisableServer(string key)
    {
        if (_tcpServer.TryGetValue(key, out var server))
        {
            _cts = new CancellationTokenSource();
            var service = server.ServiceProvider.GetService<MyService>();
            await service.StopAsync(_cts.Token);
            return service.State;
        }

        return null;
    }
}