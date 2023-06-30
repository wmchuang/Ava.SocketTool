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
        Name = $"{ip}[{port}]";
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
    /// ServerState
    /// </summary>
    [Reactive]
    public ServerStateModel ServerStateModel { get; set; } = new ServerStateModel();
    
    /// <summary>
    /// 是否运行
    /// </summary>
    [Reactive]
    public bool IsStart { get; set; }

    /// <summary>
    /// Send Message
    /// </summary>
    public string SendMessage { get; set; }

    /// <summary>
    /// Receive Message
    /// </summary>
    public string ReceiveMessage { get; set; }

    /// <summary>
    /// Children data
    /// </summary>
    [Reactive]
    public ObservableCollection<SocketTreeModel> Children { get; set; } = new();
    
    public string Key => $"{Id}_{Ip}:{Port}";
}