using System.Collections.Concurrent;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SocketServer.Server;
using SuperSocket;
using SuperSocket.ProtoBase;
using SuperSocket.Server;

namespace SocketServer;

public class SocketManager
{

    private static ConcurrentDictionary<string, IHost> _tcpServer = new ConcurrentDictionary<string, IHost>();
    private static SocketManager _instance;

    public static SocketManager Instance => _instance ??= new SocketManager();

    private static CancellationTokenSource _cts = new CancellationTokenSource();
    
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
            .UseHostedService<MyService>()
            .UseSession<MySession>()
          
            //注册用于处理接收到的数据的包处理器
            .UsePackageHandler(async (session, package) =>
            {
                PackageHandler?.Invoke(this,new PackageHandlerEventArgs()
                {
                    Message = package.Text
                });
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
            })
            .UseInProcSessionContainer()
            .Build();

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
            await service.StartAsync(_cts.Token);
            return service.State;
        }

        return null;
    }

    public async Task<ServerState?> DisableServer(string key)
    {
        if (_tcpServer.TryGetValue(key, out var host))
        {
            var service = host.Services.GetService<SuperSocketService<TextPackageInfo>>();
            await service.StopAsync(_cts.Token);
            return service.State;
        }

        return null;
    }
}