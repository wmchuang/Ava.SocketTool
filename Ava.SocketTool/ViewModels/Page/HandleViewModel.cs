using System;
using System.Collections.Generic;
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
    /// 发送次数
    /// </summary>
    [Reactive]
    public int SendNumber { get; set; } = 1;

    public List<int> SendNumbers => new List<int>()
    {
        1, 10, 100, 1000, 10000
    };

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

    /// <summary>
    /// 连接
    /// </summary>
    public ReactiveCommand<Unit, Unit> ConnectCommand => CreateCommand<Unit>(async tree =>
    {
        var ipEndPoint = await _clientManager.ConnectAsync(CurrentSelectModel.Key);
        if (ipEndPoint == null)
        {
            OverlayExtension.ShowDialog(new ErrorDialogView("连接失败"));
        }
        else
        {
            CurrentSelectModel.LocalEndPoint = ipEndPoint;
            CurrentSelectModel.IsRun = true;
        }
    });

    /// <summary>
    /// 断开
    /// </summary>
    public ReactiveCommand<Unit, Unit> CloseCommand => CreateCommand<Unit>(async tree =>
    {
        await _clientManager.CloseAsync(CurrentSelectModel.Key);

        if (!string.IsNullOrWhiteSpace(CurrentSelectModel.SessionId))
        {
            await _serverManager.CloseSession(CurrentSelectModel.LocalEndPoint, CurrentSelectModel.SessionId);
        }
    });

    /// <summary>
    /// 发送消息
    /// </summary>
    public ReactiveCommand<Unit, Unit> SendCommand => CreateCommand<Unit>(async tree =>
    {
        for (var i = 0; i < SendNumber; i++)
        {
            if (CurrentSelectModel.TypeEnum == NetTypeEnum.TcpServer)
            {
            }
            else if (CurrentSelectModel.TypeEnum == NetTypeEnum.UdpServer)
            {
                await _serverManager.SendMessage(CurrentSelectModel.LocalEndPoint,
                    CurrentSelectModel.SendMessage);
            }
            else  if (CurrentSelectModel.TypeEnum == NetTypeEnum.TcpClient)
            {
                if (!string.IsNullOrWhiteSpace(CurrentSelectModel.SessionId))
                {
                    await _serverManager.SendMessage(CurrentSelectModel.LocalEndPoint,
                        CurrentSelectModel.SendMessage, CurrentSelectModel.SessionId);
                }
                else
                {
                    await _clientManager.SendMessage(CurrentSelectModel.Key, CurrentSelectModel.SendMessage);
                }
            }
            else if (CurrentSelectModel.TypeEnum == NetTypeEnum.UdpClient)
            {
                await _clientManager.AsUdpAsync(CurrentSelectModel.Key);
                await _clientManager.SendMessage(CurrentSelectModel.Key, CurrentSelectModel.SendMessage);
            }

            var str = $"{DateTime.Now:HH:mm:dd}发送数据： {CurrentSelectModel.SendMessage}{Environment.NewLine}";
            CurrentSelectModel.ReceiveMessage += str;
        }

        CurrentSelectModel.SendMessage = string.Empty;
    });
}