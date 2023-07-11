namespace SocketServer.Model;

public class SocketModel
{
    /// <summary>
    /// 唯一Id
    /// </summary>
    public string Id { get; set; }

    /// <summary>
    /// Ip
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// 端口
    /// </summary>
    public int Port { get; set; }

    public SocketModel ClientModel { get; set; }

    public string Key => $"{Id}_{Ip}:{Port}";
}