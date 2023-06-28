using System.Text;
using Microsoft.Extensions.Hosting;
using SuperSocket;
using SuperSocket.ProtoBase;

namespace SocketServer;

public class SocketManager
{
    private static SocketManager _instance;

    public static SocketManager Instance => _instance ??= new SocketManager();
    
    public event EventHandler<PackageHandlerEventArgs> PackageHandler;

    private SocketManager()
    {
    }

    public async Task CreateTcpServer(string ip, int port)
    {
        //创建宿主：用Package的类型和PipelineFilter的类型创建SuperSocket宿主。
        var host = SuperSocketHostBuilder.Create<TextPackageInfo, LinePipelineFilter>()
            //注册用于处理接收到的数据的包处理器
            .UsePackageHandler(async (session, package) =>
            {
                PackageHandler?.Invoke(this,new PackageHandlerEventArgs()
                {
                    Message = package.Text
                });
                var result = 0;
                //发送消息给客户端
                await session.SendAsync(Encoding.UTF8.GetBytes(result.ToString() + "\r\n"));
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
                options.Name = $"{ip}:{port}";
                options.Listeners = new[]
                {
                    new ListenOptions
                    {
                        Ip = ip,
                        Port = port
                    }
                }.ToList();
            })
            .Build();
        await host.StartAsync();
    }
}