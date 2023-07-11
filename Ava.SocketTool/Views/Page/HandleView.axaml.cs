using Ava.SocketTool.Extensions;
using Ava.SocketTool.ViewModels.Page;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Ava.SocketTool.Views.Page;

public partial class HandleView : UserControl
{
    public HandleView()
    {
        AvaloniaXamlLoader.Load(this);

        this.DataContext = Bootstrapper.GetService<HandleViewModel>();
    }



    private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox tb)
        {
            tb.CaretIndex = tb.Text?.Length ?? 0;
        }
    }
}