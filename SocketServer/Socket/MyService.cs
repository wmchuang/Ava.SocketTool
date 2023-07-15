using System.Text;
using Microsoft.Extensions.Options;
using SocketServer.Encoder;
using SuperSocket;
using SuperSocket.Channel;
using SuperSocket.ProtoBase;
using SuperSocket.Server;

namespace SocketServer.Socket;

public class MyService : SuperSocketService<TextPackageInfo>
{
    public MyService(IServiceProvider serviceProvider, IOptions<ServerOptions> serverOptions) : base(serviceProvider,
        serverOptions)
    {
    }

    /// <summary>
    /// Server 停止的时候,关闭所有Session连接
    /// </summary>
    protected override async ValueTask OnStopAsync()
    {
        var sessionContainer = this.GetSessionContainer().GetSessions();
        foreach (var session in sessionContainer)
        {
            await session.CloseAsync(CloseReason.LocalClosing);
        }

        await base.OnStopAsync();
    }

    /// <summary>
    /// 关闭Session
    /// </summary>
    /// <param name="sessionId"></param>
    public async Task CloseSessionAsync(string sessionId)
    {
        var sessionContainer = this.GetSessionContainer().GetSessions();

        var session = sessionContainer.FirstOrDefault(x => x.SessionID == sessionId);
        if (session != null)
        {
            await session.CloseAsync(CloseReason.LocalClosing);
        }
    }
    
    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="sessionId"></param>
    /// <param name="message"></param>
    public async Task SendMessageAsync(string sessionId,string message)
    {
        var sessionContainer = this.GetSessionContainer().GetSessions();

        var session = sessionContainer.FirstOrDefault(x => x.SessionID == sessionId);
        if (session != null)
        {
            await session.SendAsync(DefaultEncoder.Encoding.GetBytes(message));
        }
    }
}