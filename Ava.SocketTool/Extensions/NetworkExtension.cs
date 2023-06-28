using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace Ava.SocketTool.Extensions;

public class NetworkExtension
{
    
    static List<IPAddress> GetNonLoopbackIPs()
    {
        return Dns.GetHostAddresses(Dns.GetHostName())
            .Where(ip => !IPAddress.IsLoopback(ip))
            .ToList();
    }
    
    
    static List<IPAddress> GetLocalIPs()
    {
        return NetworkInterface.GetAllNetworkInterfaces()
            .Where(nic => nic.OperationalStatus == OperationalStatus.Up && !nic.Description.Contains("Virtual")) 
            .SelectMany(nic => nic.GetIPProperties().UnicastAddresses)
            .Where(addr => addr.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
            .Select(addr => addr.Address)
            .ToList();
    }
    /// <summary>
    /// 获取ip
    /// </summary>
    /// <returns></returns>
    public static string GetIp()
    {
        try
        {



            var ipEntry = GetLocalIPs();
            foreach (var ip in ipEntry)
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