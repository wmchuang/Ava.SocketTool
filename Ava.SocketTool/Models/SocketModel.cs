using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Ava.SocketTool.Models;

public class SocketModel : TreeDataModel
{
    public SocketModel(string name)
    {
        Name = name;
    }

    public SocketModel(string ip, string port)
    {
        Ip = ip;
        Port = port;
        Name = $"{ip}[{port}]";
    }

    /// <summary>
    /// Ip
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    public string Port { get; set; }

    /// <summary>
    /// Is Enable
    /// </summary>
    [Reactive]
    public bool IsEnable { get; set; } = true;

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
    public new ObservableCollection<SocketModel> Children { get; set; } = new();
}