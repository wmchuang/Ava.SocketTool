using System.Collections.ObjectModel;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace Ava.SocketTool.Models;

public class NetType : ModelBase
{
    public NetType()
    {
    }

    public NetType(string ip, string port)
    {
        Ip = ip;
        Port = port;
        Name = $"{ip}[{port}]";
    }

    public string Name { get; set; } = string.Empty;
    public NetTypeEnum TypeEnum { get; set; }
    
    /// <summary>
    /// Ip
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    public string Port { get; set; }
    
    /// <summary>
    /// Send Message
    /// </summary>
    public string SendMessage { get; set; }

    /// <summary>
    /// Receive Message
    /// </summary>
    public string ReceiveMessage { get; set; }

    /// <summary>
    /// 
    /// </summary>
    [Reactive] 
    public ObservableCollection<NetType> Children { get; set; } = new();
    
    
    
}