using System.Collections.Concurrent;
using System.Net;
using System.Text;
using SocketServer.Model;
using SocketServer.Socket;
using SuperSocket.Client;
using SuperSocket.ProtoBase;

namespace SocketServer;

public class SocketClientManager : ISocketClientManager
{
    private static ConcurrentDictionary<string, IEasyClient<TextPackageInfo>> _tcpClients = new();
    
    private static CancellationTokenSource _cts = new();

    public async Task<SocketModel> CreateTcpClient(SocketModel model)
    {
        var filter = new MyPipelineFilter();
        var easyClient = new MyClient<TextPackageInfo>(filter);
       
        var client = easyClient.AsClient();
        // 解析 IP 地址
        var ipAddress = IPAddress.Parse(model.Ip);
        // 创建 IPEndPoint
        var ipEndPoint = new IPEndPoint(ipAddress, model.Port);
        
        await client.ConnectAsync(ipEndPoint);
        
        client.StartReceive();


        var channel = easyClient.GetChannel();
       if(channel.LocalEndPoint is IPEndPoint point)
       {
           model.ClientModel = new SocketModel()
           {
               Ip = point.Address.ToString(),
               Port = point.Port
           };
       }

        _tcpClients.TryAdd(model.Key, client);
       
        return model;
    }
    
    
    public async Task SendMessage(string key,string message)
    {
        if (_tcpClients.TryGetValue(key, out var client))
        {
            var bytes = Encoding.GetEncoding("GBK").GetBytes(message);
            await client.SendAsync(bytes);
        }

     
    }
}