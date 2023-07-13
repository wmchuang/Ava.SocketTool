using System;
using System.Reactive;
using Ava.SocketTool.Extensions;
using Ava.SocketTool.Views.Dialog;
using ReactiveUI;

namespace Ava.SocketTool.ViewModels;

public class ViewModelBase : ReactiveObject
{
    /// <summary>
    /// 创建命令
    /// </summary>
    /// <typeparam name="TParam"></typeparam>
    /// <param name="func"></param>
    /// <returns></returns>
    protected static ReactiveCommand<TParam, Unit> CreateCommand<TParam>(
        Func<TParam, System.Threading.Tasks.Task> func)
    {
        var command = ReactiveCommand.CreateFromTask<TParam>(async (val) => await func(val));
        command.ThrownExceptions.Subscribe((ex) =>
        {
            OverlayExtension.ShowDialog(new ErrorDialogView($"客户端异常:{ex.Message}"));
        });
        return command;
    }
}