using System;
using System.Reactive;
using Ava.SocketTool.Extensions;
using Ava.SocketTool.Models;
using Ava.SocketTool.Views.Dialog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using SocketServer;

namespace Ava.SocketTool.ViewModels.Page;

public class HandleViewModel : ViewModelBase
{
    private readonly ISocketServerManager _serverManager;
    private readonly ISocketClientManager _clientManager;

    public HandleViewModel()
    {
    }

    public HandleViewModel(ISocketServerManager serverManager,ISocketClientManager clientManager)
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
        var state = await _serverManager.EnableServer(CurrentSelectModel.Key);
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
    /// 停止监听
    /// </summary>
    public ReactiveCommand<Unit, Unit> StopListenCommand => CreateCommand<Unit>(async tree =>
    {
        var state = await _serverManager.DisableServer(CurrentSelectModel.Key);
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
    /// 发送消息
    /// </summary>
    public ReactiveCommand<Unit, Unit> SendCommand => CreateCommand<Unit>(async tree =>
    {  
        await _clientManager.SendMessage(CurrentSelectModel.Key,CurrentSelectModel.SendMessage);
         
        var str = $"{DateTime.Now:HH:mm:dd}发送数据： {CurrentSelectModel.SendMessage}{Environment.NewLine}";
        CurrentSelectModel.ReceiveMessage += str;
        CurrentSelectModel.SendMessage = string.Empty;
    });
}