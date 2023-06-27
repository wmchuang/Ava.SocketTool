using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Ava.SocketTool.Models;

public class NetType : ModelBase
{
    public string Name { get; set; } = string.Empty;
    public NetTypeEnum TypeEnum { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Reactive] 
    public ObservableCollection<NetType> Children { get; set; } = new();
}