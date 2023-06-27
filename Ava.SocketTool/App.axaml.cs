using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Ava.SocketTool.ViewModels;
using Ava.SocketTool.Views;

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
            var  vm =  new MainViewModel();
            vm.InitData();
            desktop.MainWindow = new MainView
            {
                DataContext = vm
            };
        }

        base.OnFrameworkInitializationCompleted();
    }
}