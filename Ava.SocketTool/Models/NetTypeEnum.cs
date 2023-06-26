using System.ComponentModel;

namespace Ava.SocketTool.Models;

public enum NetTypeEnum
{
    [Description("TCP Server")]
    TcpServer,
    [Description("TCP Client")]
    TcpClient,
    [Description("UDP Server")]
    UdpServer,
    [Description("UDP Client")]
    UdpClient
}