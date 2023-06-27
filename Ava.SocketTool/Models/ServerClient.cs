namespace Ava.SocketTool.Models;

public class ServerClient : ModelBase
{
    /// <summary>
    /// Net Type
    /// </summary>
    public NetTypeEnum TypeEnum { get; set; }

    /// <summary>
    /// Ip
    /// </summary>
    public string Ip { get; set; }

    /// <summary>
    /// Port
    /// </summary>
    public string Port { get; set; }
}