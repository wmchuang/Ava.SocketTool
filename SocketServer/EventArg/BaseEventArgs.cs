namespace SocketServer.EventArg;

public class BaseEventArgs : EventArgs
{
    public bool IsTcpServer { get; set; }
    public string ServerId { get; set; }

    public string SessionID { get; set; }
}