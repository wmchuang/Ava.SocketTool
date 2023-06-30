using ReactiveUI.Fody.Helpers;
using SuperSocket;

namespace Ava.SocketTool.Models;

public class ServerStateModel : ModelBase
{
    private ServerState _serverState;

    /// <summary>
    /// ServerState
    /// </summary>
    public ServerState ServerState
    {
        get => _serverState;
        set
        {
            if (value == ServerState.Started)
            {
                IsStart = true;
                StateText = "监听中";
            }
            else
            {
                IsStart = false;
                StateText = "已停止";
            }
            
            _serverState = value;
        }
    }

    [Reactive] public string StateText { get; set; } = "已停止";
    
    [Reactive]
    public bool IsStart { get; set; }
}