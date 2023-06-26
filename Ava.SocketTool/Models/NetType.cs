using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Ava.SocketTool.Models;

public class NetType : ReactiveObject
{
    public string Name { get; set; } = string.Empty;
    public NetTypeEnum TypeEnum { get; set; }

    [Reactive] public ObservableCollection<NetType> Sons { get; set; } = new();
}