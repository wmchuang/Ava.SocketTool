using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
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
        SocketManager.Instance.PackageHandler += (sender, args) =>
        {
            var tcpServer = TreeDataList.FirstOrDefault(x => x.TypeEnum == NetTypeEnum.TcpServer);
            var server = tcpServer.Children.FirstOrDefault(x => x.Id == args.ServerId);
            
            var client = server.Children.FirstOrDefault(x => x.Id == args.SessionID);
            var str = $"{DateTime.Now:HH:mm:dd}收到数据： {args.Message}{Environment.NewLine}";
            client.ReceiveMessage += str;
        };
        
        SocketManager.Instance.SessionConnectedHandler += (sender, args) =>
        {
            var tcpServer = TreeDataList.FirstOrDefault(x => x.TypeEnum == NetTypeEnum.TcpServer);
            var server = tcpServer.Children.FirstOrDefault(x => x.Id == args.ServerId);


            var sp = args.RemoteEndPoint.ToString().Split(':');
            server.Children.Add(new SocketTreeModel(sp[0],System.Convert.ToInt32(sp[1]))
            {
                Id = args.SessionID,
                TypeEnum = NetTypeEnum.TcpClient,
                IsConnect = true
            });
        };
    }

    [Reactive] public ObservableCollection<SocketTreeModel> TreeDataList { get; set; } = new();

    /// <summary>
    /// 当前选择的对象
    /// </summary>
    [Reactive]
    public SocketTreeModel CurrentSelectModel { get; set; } = new SocketTreeModel();

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
            OverlayExtension.ShowDialog(new CreateNodeViewModel(this, treeDataModel.TypeEnum));
        }
        else
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("请选择类型"));
        }
    });
    
    
    /// <summary>
    /// 删除
    /// </summary>
    public ReactiveCommand<Unit, Unit> DeleteCommand => CreateCommand<Unit>(async _ =>
    {
        foreach (var item in TreeDataList)
        {
             var model = item.Children.FirstOrDefault(x => x.Id == CurrentSelectModel.Id);
             if (model != null)
             {
                 StopListenCommand.Execute();
                 item.Children.Remove(model);
                 CurrentSelectModel = new();
                 break;
             }
        }
    });

    /// <summary>
    /// 启动
    /// </summary>
    public ReactiveCommand<Unit, Unit> StartListenCommand => CreateCommand<Unit>(async tree =>
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
    public ReactiveCommand<Unit, Unit> StopListenCommand => CreateCommand<Unit>(async tree =>
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