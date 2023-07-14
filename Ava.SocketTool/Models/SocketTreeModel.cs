using System;
using System.Collections.ObjectModel;
using System.Net;
using ReactiveUI.Fody.Helpers;

namespace Ava.SocketTool.Models;

public class SocketTreeModel : ModelBase
{
    public SocketTreeModel()
    {
    }

    public SocketTreeModel(NetTypeEnum typeEnum)
    {
        TypeEnum = typeEnum;
    }

    public SocketTreeModel(NetTypeEnum typeEnum, string displayName) : this(typeEnum)
    {
        DisplayName = displayName;
    }

    public SocketTreeModel(NetTypeEnum typeEnum, IPEndPoint ipEndPoint) : this(typeEnum)
    {
        if (TypeEnum == NetTypeEnum.TcpServer || TypeEnum == NetTypeEnum.UdpServer)
            LocalEndPoint = ipEndPoint;
        else
            RemoteEndPoint = ipEndPoint;

        SetDisplayName(ipEndPoint);
    }

    /// <summary>
    /// 唯一Id
    /// </summary>
    public string Id { get; set; } = Guid.NewGuid().ToString();

    /// <summary>
    /// UI 显示名称
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// NetTypeEnum
    /// </summary>
    public NetTypeEnum TypeEnum { get; set; }

    /// <summary>
    /// 本地地址
    /// </summary>
    [Reactive]
    public IPEndPoint LocalEndPoint { get; set; }

    /// <summary>
    /// 远程地址
    /// </summary>
    [Reactive]
    public IPEndPoint RemoteEndPoint { get; set; }

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

    public string Key
    {
        get
        {
            if ((TypeEnum == NetTypeEnum.TcpServer || TypeEnum == NetTypeEnum.UdpServer) && LocalEndPoint != null)
                return $"{Id}_{LocalEndPoint}";
            
            if ((TypeEnum == NetTypeEnum.TcpClient || TypeEnum == NetTypeEnum.UdpClient) && RemoteEndPoint != null)
                return $"{Id}_{RemoteEndPoint}";

            return string.Empty;
        }
    }

    private void SetDisplayName(IPEndPoint ipEndPoint)
    {
        DisplayName = $"{ipEndPoint.Address}[{ipEndPoint.Port}]";
    }
}