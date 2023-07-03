using Microsoft.Extensions.Options;
using SuperSocket;
using SuperSocket.Channel;
using SuperSocket.ProtoBase;
using SuperSocket.Server;

namespace SocketServer.Server;

public class MyService : SuperSocketService<TextPackageInfo>
{
    private readonly List<IAppSession> _appSessions;
    private readonly CancellationTokenSource _tokenSource;

    public MyService(IServiceProvider serviceProvider, IOptions<ServerOptions> serverOptions) : base(serviceProvider,
        serverOptions)
    {
        _appSessions = new List<IAppSession>();
        _tokenSource = new CancellationTokenSource();
    }

    protected override ValueTask OnStopAsync()
    {
        var sessionContainer = this.GetSessionContainer().GetSessions();
        foreach (var session in sessionContainer)
        {
            session.CloseAsync(CloseReason.LocalClosing);
        }

        return base.OnStopAsync();
    }
}