using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;

namespace Ava.SocketTool.Views.Dialog;

public partial class ErrorDialogView : DialogBaseUserControl
{
    public ErrorDialogView()
    {
        InitializeComponent();
    }

    public ErrorDialogView(string errorMsg) : this()
    {
        this.ErrorMsg.Text = errorMsg;
    }


    private void Close_OnClick(object sender, RoutedEventArgs e)
    {
        CloseDialog();
    }
}