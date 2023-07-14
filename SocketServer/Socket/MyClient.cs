using System.Net;
using SuperSocket.Channel;
using SuperSocket.Client;
using SuperSocket.ProtoBase;

namespace SocketServer.Socket;

public class MyClient<TReceivePackage> : EasyClient<TReceivePackage> where TReceivePackage : class
{
    public IPEndPoint RemoteEndPoint { get; set; }

    public MyClient(IPipelineFilter<TReceivePackage> pipelineFilter) : base(pipelineFilter)
    {
    }

    public IChannel GetChannel()
    {
        return Channel;
    }

    protected override ValueTask<TReceivePackage> ReceiveAsync()
    {
        return base.ReceiveAsync();
    }
}