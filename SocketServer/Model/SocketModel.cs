using System.Net;

namespace SocketServer.Model;

public class SocketModel
{
    /// <summary>
    /// 唯一Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// 本地地址
    /// </summary>
    public IPEndPoint LocalEndPoint { get; set; }
    
    public string Key => $"{Id}_{LocalEndPoint.ToString()}";
}