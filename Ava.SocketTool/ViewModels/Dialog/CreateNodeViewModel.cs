using System;
using System.Net;
using System.Reactive;
using System.Threading.Tasks;
using Ava.SocketTool.Extensions;
using Ava.SocketTool.Models;
using Ava.SocketTool.Models.Dialog;
using Ava.SocketTool.Views.Dialog;
using Avalonia.Threading;
using ReactiveUI;
using SocketServer;
using SocketServer.Model;
using SuperSocket;

namespace Ava.SocketTool.ViewModels.Dialog;

public class CreateNodeViewModel : ViewModelBase
{
    private readonly ISocketServerManager _serverManager;
    private readonly ISocketClientManager _clientManager;
    private MainViewModel Owner { get; set; }

    public NodeModel NodeModel { get; set; }

    public CreateNodeViewModel()
    {
    }

    public CreateNodeViewModel(ISocketServerManager serverManager, ISocketClientManager clientManager)
    {
        _serverManager = serverManager;
        _clientManager = clientManager;
    }

    public void Init(MainViewModel owner, NetTypeEnum typeEnum)
    {
        Owner = owner;
        NodeModel = new NodeModel
        {
            TypeEnum = typeEnum
        };
    }

    /// <summary>
    /// 创建节点
    /// </summary>
    public ReactiveCommand<Unit, Unit> CreateCommand => CreateCommand<Unit>(async _ =>
    {
        var ipEndPoint = new IPEndPoint(NetworkExtension.GetIp(), NodeModel.Port);
        var socketModel = new SocketTreeModel(NodeModel.TypeEnum, ipEndPoint);

        if (socketModel.TypeEnum == NetTypeEnum.TcpServer || socketModel.TypeEnum == NetTypeEnum.UdpServer)
        {
            if (!await CreateServer(socketModel, socketModel.TypeEnum == NetTypeEnum.TcpServer)) return;
        }

        if (socketModel.TypeEnum == NetTypeEnum.TcpClient || socketModel.TypeEnum == NetTypeEnum.UdpClient)
        {
            await CreateClient(socketModel);
        }

        Owner.Add(NodeModel.TypeEnum, socketModel);
        await Dispatcher.UIThread.InvokeAsync(OverlayExtension.CloseDialog);
    });

    private async Task<bool> CreateServer(SocketTreeModel socketModel, bool isTcpServer)
    {
        var result = await _serverManager.CreateServer(new SocketModel
        {
            Id = socketModel.Id,
            LocalEndPoint = socketModel.LocalEndPoint
        }, isTcpServer);
        if (!result)
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("创建失败，端口可能被占用！"));
            return false;
        }

        socketModel.IsRun = true;
        return true;
    }

    private async Task CreateClient(SocketTreeModel socketModel)
    {
        var model = new SocketModel
        {
            Id = socketModel.Id,
            LocalEndPoint = socketModel.RemoteEndPoint
        };
        _clientManager.CreateClient(model);
    }
}