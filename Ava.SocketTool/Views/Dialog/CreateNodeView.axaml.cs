using Ava.SocketTool.Extensions;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Ava.SocketTool.Views.Dialog;

public partial class CreateNodeView : UserControl
{
    public CreateNodeView()
    {
        InitializeComponent();
    }

   


    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {
        OverlayExtension.CloseDialog();
    }
}