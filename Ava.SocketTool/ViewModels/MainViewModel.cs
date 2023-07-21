using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Reactive;
using Ava.SocketTool.Models;
using ReactiveUI.Fody.Helpers;
using Ava.SocketTool.Extensions;
using Ava.SocketTool.ViewModels.Dialog;
using Ava.SocketTool.ViewModels.Page;
using Ava.SocketTool.Views.Dialog;
using Avalonia.Controls;
using ReactiveUI;
using SocketServer;
using SocketServer.Socket;
using SuperSocket.ProtoBase;

namespace Ava.SocketTool.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly ISocketServerManager _serverManager;
    private readonly ISocketClientManager _clientManager;

    public MainViewModel()
    {
    }

    public MainViewModel(ISocketServerManager serverManager, ISocketClientManager clientManager)
    {
        _serverManager = serverManager;
        _clientManager = clientManager;
        HandEvent();
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
            TreeDataList.Add(new SocketTreeModel(item.Type, item.Description));
        }
    }

    public void Add(NetTypeEnum typeEnum, SocketTreeModel netType)
    {
        var treeDataParent = TreeDataList.FirstOrDefault(x => x.TypeEnum == typeEnum);
        treeDataParent.Children.Add(netType);
    }

    #region Command

    /// <summary>
    /// 选中切换
    /// </summary>
    public ReactiveCommand<TreeView, Unit> SelectionChangedCommand => CreateCommand<TreeView>(async tree =>
    {
        if (tree.SelectedItem != null)
        {
            CurrentSelectModel = tree.SelectedItem as SocketTreeModel;

            var handleViewMode = Bootstrapper.GetService<HandleViewModel>();
            handleViewMode.CurrentSelectModel = CurrentSelectModel;
        }
    });

    /// <summary>
    /// 创建
    /// </summary>
    public ReactiveCommand<TreeView, Unit> CreateCommand => CreateCommand<TreeView>(async tree =>
    {
        if (tree.SelectedItem is SocketTreeModel treeDataModel)
        {
            var viewMode = Bootstrapper.GetService<CreateNodeViewModel>();
            viewMode.Init(this, treeDataModel.TypeEnum);
            OverlayExtension.ShowDialog(viewMode);
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
                await _serverManager.RemoveServer(model.Key);
                item.Children.Remove(model);
                CurrentSelectModel = new();
                break;
            }
        }
    });

    #endregion

    public void HandEvent()
    {
        _serverManager.PackageHandler += (sender, args) =>
        {
            if (args.IsTcpServer)
            {
                var serverRoot = TreeDataList.FirstOrDefault(x => x.TypeEnum == NetTypeEnum.TcpServer);
                var server = serverRoot?.Children.FirstOrDefault(x => x.Id == args.ServerId);

                var node = server?.Children.FirstOrDefault(x => x.Id == args.SessionID);
                var str = $"{DateTime.Now:HH:mm:dd}收到数据：{args.Message}{Environment.NewLine}";
                if (node != null) node.ReceiveMessage += str;
            }
            else
            {
                var serverRoot = TreeDataList.FirstOrDefault(x => x.TypeEnum == NetTypeEnum.UdpServer);
                var node = serverRoot?.Children.FirstOrDefault(x => x.Id == args.ServerId);
                var str = $"{DateTime.Now:HH:mm:dd}收到[{args.SessionID}]数据：{args.Message}{Environment.NewLine}";
                if (node != null) node.ReceiveMessage += str;
            }
        };

        _serverManager.SessionConnectedHandler += (sender, args) =>
        {
            if (args.IsTcpServer)
            {
                var tcpServer = TreeDataList.FirstOrDefault(x => x.TypeEnum == NetTypeEnum.TcpServer);
                var server = tcpServer?.Children.FirstOrDefault(x => x.Id == args.ServerId);

                server?.Children.Add(new SocketTreeModel(NetTypeEnum.TcpClient, (IPEndPoint)args.RemoteEndPoint)
                {
                    Id = args.SessionID,
                    SessionId = args.SessionID,
                    LocalEndPoint = (IPEndPoint)args.LocalEndPoint,
                    IsRun = true
                });
            }
        };

        _serverManager.SessionClosedHandler += (sender, args) =>
        {
            if (args.IsTcpServer)
            {
                //删除Server下的Client节点
                var tcpServer = TreeDataList.FirstOrDefault(x => x.TypeEnum == NetTypeEnum.TcpServer);
                var server = tcpServer?.Children.FirstOrDefault(x => x.Id == args.ServerId);

                var closeSession = server?.Children.FirstOrDefault(x => x.Id == args.SessionID);
                server?.Children.Remove(closeSession);

                //找到Client下的节点，改变其状态
                var tcpClient = TreeDataList.FirstOrDefault(x => x.TypeEnum == NetTypeEnum.TcpClient);
                if (tcpClient != null)
                {
                    var client = tcpClient.Children.FirstOrDefault(x => Equals(x.LocalEndPoint, args.RemoteEndPoint));
                    if (client != null)
                    {
                        client.IsRun = false;
                        client.LocalEndPoint = null;
                    }
                }
            }
        };

        _clientManager.PackageHandler += (sender, args) =>
        {
            SocketTreeModel clientRoot;
            if (args.IsTcpServer)
            {
                clientRoot = TreeDataList.FirstOrDefault(x => x.TypeEnum == NetTypeEnum.TcpClient);
            }
            else
            {
                clientRoot = TreeDataList.FirstOrDefault(x => x.TypeEnum == NetTypeEnum.UdpClient);
            }

            var client = clientRoot?.Children.FirstOrDefault(x => Equals(x.LocalEndPoint, sender));
            var str = $"{DateTime.Now:HH:mm:dd}收到数据： {args.Message}{Environment.NewLine}";
            if (client != null) client.ReceiveMessage += str;
        };

        _clientManager.ClosedHandler += (sender, args) =>
        {
            //找到Client下的节点，改变其状态
            var tcpClient = TreeDataList.FirstOrDefault(x => x.TypeEnum == NetTypeEnum.TcpClient);
            if (tcpClient != null)
            {
                var client = tcpClient.Children.FirstOrDefault(x => Equals(x.LocalEndPoint, args.LocalEndPoint));
                if (client != null)
                {
                    client.IsRun = false;
                    client.LocalEndPoint = null;
                }
            }
        };
    }
}