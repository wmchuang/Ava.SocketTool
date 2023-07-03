namespace SocketServer.EventArg;

public class BaseEventArgs : EventArgs
{
    public string ServerId { get; set; }
    
    public string SessionID { get; set; }

 
}