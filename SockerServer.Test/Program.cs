
using SuperSocket.ProtoBase;
using SuperSocket;
using System.Text;
using Microsoft.Extensions.Hosting;

internal class Program
{
    private static async Task Main (string[] args)
    {
        Console.WriteLine("Hello, World!");

        Console.WriteLine("Press any key to start the server!");

        //创建宿主：用Package的类型和PipelineFilter的类型创建SuperSocket宿主。
        var host = SuperSocketHostBuilder.Create<StringPackageInfo, CommandLinePipelineFilter>()
            //注册用于处理接收到的数据的包处理器
            .UsePackageHandler(async (session, package) =>
            {
                var result = 0;
                switch (package.Key.ToUpper())
                {
                    case ("ADD"):
                        result = package.Parameters
                            .Select(t => int.Parse(t))
                            .Sum();
                        break;

                    case ("SUB"):
                        result = package.Parameters
                            .Select(t => int.Parse(t))
                            .Aggregate((x, y) => x - y);
                        break;

                    case ("MULT"):
                        result = package.Parameters
                            .Select(t => int.Parse(t))
                            .Aggregate((x, y) => x * y);
                        break;
                }

                //发送消息给客户端
                await session.SendAsync(Encoding.UTF8.GetBytes(result.ToString() + "\r\n"));
            })
            //配置服务器如服务器名和监听端口等基本信息
            .ConfigureSuperSocket(options =>
            {
                options.Name = "Echo Server";
                options.Listeners = new[]
                {
                        new ListenOptions
                        {
                            Ip = "Any",
                            Port = 4040
                        }
                }.ToList();
            })
           
            .Build();
        await host.RunAsync();
    }

}