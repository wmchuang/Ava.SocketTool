using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Ava.SocketTool.Views.Dialog;

public partial class CreateServerView : DialogBaseUserControl
{
    public CreateServerView()
    {
        InitializeComponent();
    }

   


    private void Cancel_OnClick(object? sender, RoutedEventArgs e)
    {
        CloseDialog();
    }
}