using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Ava.SocketTool.Models;
using ReactiveUI.Fody.Helpers;
using Ava.SocketTool.Extensions;
using Ava.SocketTool.ViewModels.Dialog;
using Avalonia.Controls;
using ReactiveUI;

namespace Ava.SocketTool.ViewModels;

public class MainViewModel : ViewModelBase
{
    public MainViewModel()
    {
        SocketServer.SocketManager.Instance.PackageHandler += (sender, args) =>
        {
            var str = $"{DateTime.Now:HH:mm:dd}收到数据： {args.Message}{ Environment.NewLine}";
            ReceiveMessage += str;
        };
    }

    [Reactive] public ObservableCollection<NetType> NetTypes { get; set; } = new();

    /// <summary>
    /// 收到的消息
    /// </summary>
    [Reactive]
    public string ReceiveMessage { get; set; }

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

    public void Add(NetTypeEnum typeEnum, NetType netType)
    {
        var netTypeParent = NetTypes.FirstOrDefault(x => x.TypeEnum == typeEnum);
        netTypeParent.Children.Add(netType);
    }

    /// <summary>
    /// 创建
    /// </summary>
    public ReactiveCommand<TreeView, Unit> CreateCommand => CreateCommand<TreeView>(async tree =>
    {
        OverlayExtension.ShowDialog(new CreateServerViewModel(this, NetTypeEnum.TcpServer));
    });
}