using Microsoft.Extensions.Options;
using SuperSocket;
using SuperSocket.Channel;
using SuperSocket.ProtoBase;
using SuperSocket.Server;

namespace SocketServer.Socket;

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

    protected override async ValueTask OnStopAsync()
    {
        var sessionContainer = this.GetSessionContainer().GetSessions();
        foreach (var session in sessionContainer)
        {
            await session.CloseAsync(CloseReason.LocalClosing);
        }

        await base.OnStopAsync();
    }

    public async Task CloseSessionAsync(string sessionId)
    {
        var sessionContainer = this.GetSessionContainer().GetSessions();

        var session = sessionContainer.FirstOrDefault(x => x.SessionID == sessionId);
        if (session != null)
        {
            await session.CloseAsync(CloseReason.LocalClosing);
        }
    }
}