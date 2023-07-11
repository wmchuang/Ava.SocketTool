using SuperSocket.Channel;
using SuperSocket.Client;
using SuperSocket.ProtoBase;

namespace SocketServer.Socket;

public class MyClient<TReceivePackage> : EasyClient<TReceivePackage> where TReceivePackage : class
{
    public MyClient(IPipelineFilter<TReceivePackage> pipelineFilter) : base(pipelineFilter)
    {
    }

    public IChannel GetChannel()
    {
        return Channel;
    }

}