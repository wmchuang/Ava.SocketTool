using System;
using System.Net;
using System.Net.Sockets;

namespace Ava.SocketTool.Extensions;

public class NetworkExtension
{
    /// <summary>
    /// 获取ip
    /// </summary>
    /// <returns></returns>
    public static string GetIp()
    {
        try
        {
            IPHostEntry ipEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in ipEntry.AddressList)
            {
                //从IP地址列表中筛选出IPv4类型的IP地址
                //AddressFamily.InterNetwork表示此IP为IPv4,
                //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return "127.0.0.1";
        }
        catch (System.Exception e)
        {
            Console.WriteLine(e.Message);
            return "127.0.0.1";
        }
    }
}