using System;
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
        var socketModel = new SocketTreeModel(NetworkExtension.GetIp(), NodeModel.Port)
        {
            TypeEnum = NodeModel.TypeEnum
        };

        if (socketModel.TypeEnum == NetTypeEnum.TcpServer)
        {
            if (!await CreateTcpServer(socketModel)) return;
        }

        if (socketModel.TypeEnum == NetTypeEnum.TcpClient)
        {
            await CreateTcpClient(socketModel);
        }

        Owner.Add(NodeModel.TypeEnum, socketModel);
        await Dispatcher.UIThread.InvokeAsync(OverlayExtension.CloseDialog);
    });

    private async Task<bool> CreateTcpServer(SocketTreeModel socketModel)
    {
        var result = await _serverManager.CreateTcpServer(new SocketModel
        {
            Id = socketModel.Id,
            Ip = socketModel.Ip,
            Port = socketModel.Port,
        });
        if (!result)
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("创建失败，端口可能被占用！"));
            return false;
        }

        socketModel.IsRun = true;
        return true;
    }

    private async Task CreateTcpClient(SocketTreeModel socketModel)
    {
        var model = new SocketModel
        {
            Id = socketModel.Id,
            Ip = socketModel.Ip,
            Port = socketModel.Port,
        };
        _clientManager.CreateTcpClient(model);
        var point = await _clientManager.ConnectAsync(model.Key);
        if (point != null)
        {
            socketModel.IsRun = true;
        }
    }
}