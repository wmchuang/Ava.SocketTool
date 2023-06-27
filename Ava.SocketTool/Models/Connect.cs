namespace Ava.SocketTool.Models;

public class Connect : ModelBase
{
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
}