using Ava.SocketTool.ViewModels;
using Ava.SocketTool.ViewModels.Dialog;
using Ava.SocketTool.ViewModels.Page;
using Microsoft.Extensions.DependencyInjection;
using SocketServer;

namespace Ava.SocketTool.Extensions;

public static class Bootstrapper
{
    private static ServiceProvider _serviceProvider;

    public static void ConfigureServices(IServiceCollection services)
    {
        services.AddSingleton<ISocketServerManager, SocketServerManager>();
        services.AddSingleton<ISocketClientManager, SocketClientManager>();

        services.AddSingleton<MainViewModel>();
        services.AddSingleton<HandleViewModel>();
        services.AddSingleton<CreateNodeViewModel>();
        _serviceProvider = services.BuildServiceProvider();
    }

    public static TService GetService<TService>()
    {
        return _serviceProvider.GetService<TService>();
    }
}