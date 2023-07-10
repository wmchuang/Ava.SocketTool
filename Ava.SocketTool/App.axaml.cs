using System.Text;
using Ava.SocketTool.Extensions;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ava.SocketTool.ViewModels;
using Ava.SocketTool.Views;
using Microsoft.Extensions.DependencyInjection;

namespace Ava.SocketTool;

public partial class App : Application
{
    public App()
    {
        var viewLocator = new ViewLocator();
        DataTemplates.Add(viewLocator);
    }

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var vm = Bootstrapper.GetService<MainViewModel>();
            vm.InitData();
            desktop.MainWindow = new MainView
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }

    public override void RegisterServices()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        
        var serviceCollection = new ServiceCollection();
        Bootstrapper.ConfigureServices(serviceCollection);
        
        base.RegisterServices();
    }
}