using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocketServer.PipelineFilter;
using SuperSocket;
using SuperSocket.ProtoBase;
using SuperSocket.Server;

namespace SocketServer;

public class SocketManager
{

    private static ConcurrentDictionary<string, IHost> _tcpServer = new ConcurrentDictionary<string, IHost>();
    private static SocketManager _instance;

    public static SocketManager Instance => _instance ??= new SocketManager();
    
    /// <summary>
    /// 数据包处理
    /// </summary>
    public event EventHandler<PackageHandlerEventArgs> PackageHandler;
    
  

    private SocketManager()
    {
    }

    public async Task<ServerState> CreateTcpServer(SocketModel model)
    {
        //创建宿主：用Package的类型和PipelineFilter的类型创建SuperSocket宿主。
        var host = SuperSocketHostBuilder.Create<TextPackageInfo, MyPipelineFilter>()
            //注册用于处理接收到的数据的包处理器
            .UsePackageHandler(async (session, package) =>
            {
                PackageHandler?.Invoke(this,new PackageHandlerEventArgs()
                {
                    Message = package.Text
                });
            })
            //注册用于处理连接、关闭的Session处理器
            .UseSessionHandler(async (session) =>
            {
                Console.WriteLine($"{DateTime.Now} [SessionHandler] Session connected: {session.RemoteEndPoint}");
                await Task.Delay(0);
            }, async (session, reason) =>
            {
                Console.WriteLine($"{DateTime.Now} [SessionHandler] Session {session.RemoteEndPoint} closed: {reason}");
                await Task.Delay(0);
            })
            //配置服务器如服务器名和监听端口等基本信息
            .ConfigureSuperSocket(options =>
            {
                options.Name = model.Name;
                options.Listeners = new[]
                {
                    new ListenOptions
                    {
                        Ip = model.Ip,
                        Port = model.Port
                    }
                }.ToList();
            }).Build();

        _tcpServer.TryAdd(model.Key, host);
       await host.StartAsync();
       
       var service = host.Services.GetService<SuperSocketService<TextPackageInfo>>();
       return service.State;

    }

    public async Task<ServerState?> EnableServer(string key)
    {
        if (_tcpServer.TryGetValue(key, out var host))
        {
            var service = host.Services.GetService<SuperSocketService<TextPackageInfo>>();
            return service.State;
        }

        return null;
    }

    public async Task<ServerState?> DisableServer(string key)
    {
        if (_tcpServer.TryGetValue(key, out var host))
        {
            await host.StopAsync();
            var service = host.Services.GetService<SuperSocketService<TextPackageInfo>>();
            return service.State;
        }

        return null;
    }
}