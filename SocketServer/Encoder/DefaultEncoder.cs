using System.Text;

namespace SocketServer.Encoder;

/// <summary>
/// 默认编码
/// </summary>
public static class DefaultEncoder
{
    public static Encoding Encoding = Encoding.GetEncoding("GBK");
}