using System;
using System.Reactive;
using Ava.SocketTool.Extensions;
using Ava.SocketTool.Models;
using Ava.SocketTool.Views.Dialog;
using Avalonia.Threading;
using ReactiveUI;
using SocketServer;
using SuperSocket;

namespace Ava.SocketTool.ViewModels.Dialog;

public class CreateServerViewModel : ViewModelBase
{
    public NetTypeEnum TypeEnum { get; set; }

    public MainViewModel Owner { get; set; }

    public CreateServerViewModel()
    {
    }

    public CreateServerViewModel(MainViewModel owner, NetTypeEnum typeEnum) : this()
    {
        Owner = owner;
        TypeEnum = typeEnum;
    }

    /// <summary>
    /// 创建
    /// </summary>
    public ReactiveCommand<string, Unit> CreateCommand => CreateCommand<string>(async portStr =>
    {
        if (!int.TryParse(portStr, out var port))
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("端口号不正确！"));
            return;
        }

        var socketModel = new SocketTreeModel(NetworkExtension.GetIp(), port)
        {
            TypeEnum = TypeEnum
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
        Owner.Add(TypeEnum, socketModel);
        await Dispatcher.UIThread.InvokeAsync(OverlayExtension.CloseDialog);
    });
}