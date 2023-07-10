using ReactiveUI.Fody.Helpers;

namespace Ava.SocketTool.Models.Dialog;

public class NodeModel : ModelBase
{
    /// <summary>
    /// NetTypeEnum
    /// </summary>
    public NetTypeEnum TypeEnum { get; set; }

    /// <summary>
    /// Ip
    /// </summary>
    [Reactive]
    public string Ip { get; set; } = "127.0.0.1";

    /// <summary>
    /// Port
    /// </summary>
    [Reactive]
    public int Port { get; set; } = 60000;
}