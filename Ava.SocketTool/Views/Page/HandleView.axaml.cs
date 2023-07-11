using Ava.SocketTool.Extensions;
using Ava.SocketTool.ViewModels.Page;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

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