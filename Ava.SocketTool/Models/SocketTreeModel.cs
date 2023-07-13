using System;
using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;
using SuperSocket;

namespace Ava.SocketTool.Models;

public class SocketTreeModel : ModelBase
{
    public SocketTreeModel()
    {
    }

    public SocketTreeModel(string name)
    {
        Name = name;
    }

    public SocketTreeModel(string ip, int port)
    {
        Ip = ip;
        Port = port;
        SetName(ip, port);
    }

    /// <summary>
    /// 唯一Id
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// NetTypeEnum
    /// </summary>
    public NetTypeEnum TypeEnum { get; set; }

    /// <summary>
    /// Ip
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// 是否正在运行
    /// </summary>
    [Reactive]
    public bool IsRun { get; set; }

    /// <summary>
    /// Send Message
    /// </summary>
    [Reactive]
    public string SendMessage { get; set; }

    /// <summary>
    /// Receive Message
    /// </summary>
    [Reactive]
    public string ReceiveMessage { get; set; }

    /// <summary>
    /// Children data
    /// </summary>
    [Reactive]
    public ObservableCollection<SocketTreeModel> Children { get; set; } = new();

    public string Key => $"{Id}_{Ip}:{Port}";

    public void SetName(string ip, int port)
    {
        Name = $"{ip}[{port}]";
    }
}