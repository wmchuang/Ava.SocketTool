using System;
using System.Reactive;
using Ava.SocketTool.Extensions;
using Ava.SocketTool.Models;
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
    public ReactiveCommand<Unit, Unit> CreateCommand => CreateCommand<Unit>(async tree =>
    {
        var ip = NetworkExtension.GetIp();
        var port = 60000;
        await SocketServer.SocketManager.Instance.CreateTcpServer(ip, port);
        Owner.Add(TypeEnum,$"{ip}:{port}");
        
        await Dispatcher.UIThread.InvokeAsync(OverlayExtension.CloseDialog);
     
    });
}