using System.Linq;
using Ava.SocketTool.Controls;
using Ava.SocketTool.ViewModels;
using Ava.SocketTool.Views;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.VisualTree;
using ReactiveUI;

namespace Ava.SocketTool.Extensions;

public class OverlayExtension
{
    /// <summary>
    /// 打开弹窗
    /// </summary>
    /// <param name="viewModel"></param>
    public static void ShowDialog(ViewModelBase viewModel)
    {
        var control = Application.Current.DataTemplates.First().Build(viewModel);
        
        var overlay = FindOverlay();
        if (overlay != null)
        {
            overlay.OverlayContent = control;
            overlay.IsOverlayVisible = true;
        }
    }

    
    public static void ShowDialog(Control control)
    {
        var overlay = FindOverlay();
        if (overlay != null)
        {
            overlay.OverlayContent = control;
            overlay.IsOverlayVisible = true;
        }
    }

    public static void CloseDialog()
    {
        var overlay = FindOverlay();
        if (overlay != null)
        {
            overlay.OverlayContent = null;
            overlay.IsOverlayVisible = false;
        }
    }

    private static Overlay? FindOverlay()
    {
        var window = ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime)
            .Windows.FirstOrDefault(x => x.GetType() == typeof(MainView));
        if (window == null)
            return null;

        var overlay = window.GetVisualDescendants().OfType<Overlay>().FirstOrDefault();
        return overlay;
    }
}