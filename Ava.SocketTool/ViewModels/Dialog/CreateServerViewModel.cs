using System;
using System.Reactive;
using Ava.SocketTool.Extensions;
using Ava.SocketTool.Models;
using Ava.SocketTool.Views.Dialog;
using Avalonia.Controls;
using Avalonia.Threading;
using ReactiveUI;

namespace Ava.SocketTool.ViewModels.Dialog;

public class CreateServerViewModel : ViewModelBase
{
    public NetTypeEnum TypeEnum { get; set; }

    public MainViewModel Owner { get; set; }

    public CreateServerViewModel()
    {
    }

    public CreateServerViewModel(MainViewModel owner,NetTypeEnum typeEnum) : this()
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
            OverlayExtension.ShowDialog(new ErrorDialogView("端口号不正确"));
            return;
        }

        var netType = new NetType(NetworkExtension.GetIp(),portStr)
        {
            TypeEnum = TypeEnum
        };

        await SocketServer.SocketManager.Instance.CreateTcpServer(netType.Ip, port);
        Owner.Add(TypeEnum, netType);

        await Dispatcher.UIThread.InvokeAsync(OverlayExtension.CloseDialog);
    });
}