using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Ava.SocketTool.Models;
using ReactiveUI.Fody.Helpers;
using Ava.SocketTool.Extensions;
using Ava.SocketTool.ViewModels.Dialog;
using Ava.SocketTool.Views.Dialog;
using Avalonia.Controls;
using ReactiveUI;

namespace Ava.SocketTool.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
    }

    [Reactive] public ObservableCollection<NetType> NetTypes { get; set; } = new();

    public void InitData()
    {
        var list = EnumExtension.GetList<NetTypeEnum>();
        foreach (var item in list)
        {
            NetTypes.Add(new NetType()
            {
                Name = item.Description,
                TypeEnum = item.Type,
            });
        }
    }

    public void Add(NetTypeEnum typeEnum,string ipPort)
    {
        var m = NetTypes.FirstOrDefault(x => x.TypeEnum == typeEnum);
        m.Children.Add(new NetType()
        {
            Name = ipPort
        });
    }

    /// <summary>
    /// 创建
    /// </summary>
    public ReactiveCommand<TreeView, Unit> CreateCommand => CreateCommand<TreeView>(async tree =>
    {
        OverlayExtension.ShowDialog(new CreateServerViewModel(this, NetTypeEnum.TcpServer));
    });
      
}   