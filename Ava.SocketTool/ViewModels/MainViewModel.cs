using System;
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
        SocketServer.SocketManager.Instance.PackageHandler += (sender, args) =>
        {
            var str = $"{DateTime.Now:HH:mm:dd}收到数据： {args.Message}{Environment.NewLine}";
            ReceiveMessage += str;
        };
    }

    [Reactive] public ObservableCollection<TreeDataModel> TreeDataList { get; set; } = new();

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
            TreeDataList.Add(new SocketModel(item.Description)
            {
                TypeEnum = item.Type,
            });
        }
    }

    public void Add(NetTypeEnum typeEnum, SocketModel netType)
    {
        var treeDataParent = TreeDataList.FirstOrDefault(x => x.TypeEnum == typeEnum);
        treeDataParent.Children.Add(netType);
    }

    /// <summary>
    /// 创建
    /// </summary>
    public ReactiveCommand<TreeView, Unit> CreateCommand => CreateCommand<TreeView>(async tree =>
    {
        if (tree.SelectedItem is TreeDataModel treeDataModel)
        {
            OverlayExtension.ShowDialog(new CreateServerViewModel(this, treeDataModel.TypeEnum));
        }
        else
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("请选择类型"));
        }

    });
    
    /// <summary>
    /// 启动
    /// </summary>
    public ReactiveCommand<TreeView, Unit> EnableCommand => CreateCommand<TreeView>(async tree =>
    {
        if (tree.SelectedItem is SocketModel socketModel)
        {
            await SocketServer.SocketManager.Instance.EnableServer(socketModel.Ip, Convert.ToInt32(socketModel.Port));
        }
        else
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("请选择Server"));
        }
    });
}