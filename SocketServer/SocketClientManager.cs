using System.Collections.Concurrent;
using System.Net;
using System.Text;
using SocketServer.Encoder;
using SocketServer.EventArg;
using SocketServer.Model;
using SocketServer.Socket;
using SuperSocket.Client;
using SuperSocket.ProtoBase;

namespace SocketServer;

public class SocketClientManager : ISocketClientManager
{
    private static ConcurrentDictionary<string, MyClient<TextPackageInfo>> _tcpClients = new();

    private static CancellationTokenSource _cts = new();
    
    /// <summary>
    /// 接收到数据包处理
    /// </summary>
    public event EventHandler<PackageHandlerEventArgs> PackageHandler;

    public event EventHandler<ClientClosedEventArgs>? ClosedHandler;

    public void CreateClient(SocketModel model)
    {
        var filter = new MyPipelineFilter();
        var myClient = new MyClient<TextPackageInfo>(filter)
        {
            RemoteEndPoint = model.LocalEndPoint
        };
        _tcpClients.TryAdd(model.Key, myClient);

    }

    public async Task SendMessage(string key, string message)
    {
        if (_tcpClients.TryGetValue(key, out var myClient))
        {
            var bytes = DefaultEncoder.Encoding.GetBytes(message);
            var client = myClient.AsClient();
            await client.SendAsync(bytes);
        }
    }
    
    public async Task<IPEndPoint?> AsUdpAsync(string key)
    {
        if (_tcpClients.TryGetValue(key, out var myClient))
        {
            myClient.AsUdp(myClient.RemoteEndPoint);
            var client = myClient.AsClient();
       
            var channel = myClient.GetChannel();
            if (channel == null) return null;
            
            client.PackageHandler += (sender, package) =>
            {
                PackageHandler?.Invoke(channel.LocalEndPoint,new PackageHandlerEventArgs
                {
                    Message = package.Text
                });
                return default;
            };
            client.StartReceive();
            if (channel.LocalEndPoint is IPEndPoint point)
            {
                return point;
            }
        }

        return null;
    }

    public async Task<IPEndPoint?> ConnectAsync(string key)
    {
        if (_tcpClients.TryGetValue(key, out var myClient))
        {
            var client = myClient.AsClient();
            await client.ConnectAsync(myClient.RemoteEndPoint);
       
            var channel = myClient.GetChannel();
            if (channel == null || channel.IsClosed) return null;
            
            client.PackageHandler += (sender, package) =>
            {
                PackageHandler?.Invoke(channel.LocalEndPoint,new PackageHandlerEventArgs
                {
                    IsTcpServer = true,
                    Message = package.Text
                });
                return default;
            };

            client.Closed += (sender, args) =>
            {
               ClosedHandler.Invoke(this,new ClientClosedEventArgs()
               {
                   LocalEndPoint = ((MyClient<TextPackageInfo>)sender).GetChannel().LocalEndPoint
               });
            };
            client.StartReceive();
            if (channel.LocalEndPoint is IPEndPoint point)
            {
                return point;
            }
        }

        return null;
    }
    
    public async Task CloseAsync(string key)
    {
        if (_tcpClients.TryGetValue(key, out var myClient))
        {
            var client = myClient.AsClient();
            await client.CloseAsync();
        }
    }
}