using System;
using System.Reactive;
using Ava.SocketTool.Extensions;
using Ava.SocketTool.Models;
using Ava.SocketTool.Models.Dialog;
using Ava.SocketTool.Views.Dialog;
using Avalonia.Threading;
using ReactiveUI;
using SocketServer;
using SuperSocket;

namespace Ava.SocketTool.ViewModels.Dialog;

public class CreateNodeViewModel : ViewModelBase
{
    public MainViewModel Owner { get; set; }

    public NodeModel NodeModel { get; set; }

    public CreateNodeViewModel()
    {
    }

    public CreateNodeViewModel(MainViewModel owner, NetTypeEnum typeEnum) : this()
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

        var state = await SocketManager.Instance.CreateTcpServer(new SocketModel()
        {
            Id = socketModel.Id,
            Ip = socketModel.Ip,
            Port = socketModel.Port,
        });
        if (state == ServerState.Failed)
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("创建失败，端口可能被占用！"));
            return;
        }

        socketModel.ServerStateModel.ServerState = state;
        Owner.Add(NodeModel.TypeEnum, socketModel);
        await Dispatcher.UIThread.InvokeAsync(OverlayExtension.CloseDialog);
    });
}