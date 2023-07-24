using System.Net;
using Ava.SocketTool.Extensions;
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
    public string Ip { get; set; } = NetworkExtension.GetIp().ToString();

    /// <summary>
    /// Port
    /// </summary>
    [Reactive]
    public int Port { get; set; } = 60000;
}