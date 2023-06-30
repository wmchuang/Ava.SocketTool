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
using SocketServer;

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

    [Reactive] public ObservableCollection<SocketTreeModel> TreeDataList { get; set; } = new();

    /// <summary>
    /// 当前选择的对象
    /// </summary>
    [Reactive]
    public SocketTreeModel CurrentSelectModel { get; set; }

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
            TreeDataList.Add(new SocketTreeModel(item.Description)
            {
                TypeEnum = item.Type,
            });
        }
    }

    public void Add(NetTypeEnum typeEnum, SocketTreeModel netType)
    {
        var treeDataParent = TreeDataList.FirstOrDefault(x => x.TypeEnum == typeEnum);
        treeDataParent.Children.Add(netType);
    }

    /// <summary>
    /// 选中切换
    /// </summary>
    public ReactiveCommand<TreeView, Unit> SelectionChangedCommand => CreateCommand<TreeView>(async tree =>
    {
        if (tree.SelectedItem != null)
        {
            CurrentSelectModel = tree.SelectedItem as SocketTreeModel;
        }
    });

    /// <summary>
    /// 创建
    /// </summary>
    public ReactiveCommand<TreeView, Unit> CreateCommand => CreateCommand<TreeView>(async tree =>
    {
        if (tree.SelectedItem is SocketTreeModel treeDataModel)
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
    public ReactiveCommand<Unit, Unit> EnableCommand => CreateCommand<Unit>(async tree =>
    {
        var state = await SocketManager.Instance.EnableServer(CurrentSelectModel.Key);
        if (state == null)
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("操作失败"));
        }
        else
        {
            CurrentSelectModel.ServerStateModel.ServerState = state.Value;
        }
    });

    /// <summary>
    /// 启动
    /// </summary>
    public ReactiveCommand<Unit, Unit> DisableCommand => CreateCommand<Unit>(async tree =>
    {
        var state = await SocketManager.Instance.DisableServer(CurrentSelectModel.Key);
        if (state == null)
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("操作失败"));
        }
        else
        {
            CurrentSelectModel.ServerStateModel.ServerState = state.Value;
        }
    });
}