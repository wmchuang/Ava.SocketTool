
using SuperSocket.ProtoBase;
using SuperSocket;
using System.Text;
using Microsoft.Extensions.Hosting;
using SocketServer;

internal class Program
{
    private static async Task Main (string[] args)
    {
        Console.WriteLine("Hello, World!");

        Console.WriteLine("Press any key to start the server!");

        SocketServer.SocketServerManager.Instance.CreateTcpServer(new SocketModel()
        {
            Ip = "Any",
            Port = 60000
        });
        Console.ReadLine();
    }

}