using System;
using System.Reactive;
using Ava.SocketTool.Extensions;
using Ava.SocketTool.Models;
using Ava.SocketTool.Views.Dialog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SocketServer;
using SuperSocket;

namespace Ava.SocketTool.ViewModels.Page;

public class HandleViewModel : ViewModelBase
{
    private readonly ISocketServerManager _serverManager;
    private readonly ISocketClientManager _clientManager;

    public HandleViewModel()
    {
    }

    public HandleViewModel(ISocketServerManager serverManager, ISocketClientManager clientManager)
    {
        _serverManager = serverManager;
        _clientManager = clientManager;
    }

    /// <summary>
    /// 当前选择的对象
    /// </summary>
    [Reactive]
    public SocketTreeModel CurrentSelectModel { get; set; } = new SocketTreeModel();

    /// <summary>
    /// 开始监听
    /// </summary>
    public ReactiveCommand<Unit, Unit> StartListenCommand => CreateCommand<Unit>(async tree =>
    {
        var result = await _serverManager.StartListen(CurrentSelectModel.Key);
        if (!result)
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("启动失败"));
        }
        else
        {
            CurrentSelectModel.IsRun = true;
        }
    });

    /// <summary>
    /// 停止监听
    /// </summary>
    public ReactiveCommand<Unit, Unit> StopListenCommand => CreateCommand<Unit>(async tree =>
    {
        var result = await _serverManager.StopListen(CurrentSelectModel.Key);
        if (!result)
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("停止失败"));
        }
        else
        {
            CurrentSelectModel.IsRun = false;
        }
    });

    public ReactiveCommand<Unit, Unit> ConnectCommand => CreateCommand<Unit>(async tree =>
    {
        var ipEndPoint = await _clientManager.ConnectAsync(CurrentSelectModel.Key);
        if (ipEndPoint == null)
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("操作失败"));
        }
        else
        {
            CurrentSelectModel.LocalEndPoint = ipEndPoint;
            CurrentSelectModel.IsRun = true;
        }
    });

    public ReactiveCommand<Unit, Unit> CloseCommand => CreateCommand<Unit>(async tree =>
    {
        await _clientManager.CloseAsync(CurrentSelectModel.Key);
        CurrentSelectModel.IsRun = false;
        CurrentSelectModel.LocalEndPoint = null;
    });

    /// <summary>
    /// 发送消息
    /// </summary>
    public ReactiveCommand<Unit, Unit> SendCommand => CreateCommand<Unit>(async tree =>
    {
        await _clientManager.SendMessage(CurrentSelectModel.Key, CurrentSelectModel.SendMessage);

        var str = $"{DateTime.Now:HH:mm:dd}发送数据： {CurrentSelectModel.SendMessage}{Environment.NewLine}";
        CurrentSelectModel.ReceiveMessage += str;
        CurrentSelectModel.SendMessage = string.Empty;
    });
}