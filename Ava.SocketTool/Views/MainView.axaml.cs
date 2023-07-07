using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace Ava.SocketTool.Views;

public partial class MainView : Window
{
    public MainView()
    {
        InitializeComponent();
    }
    
    private void ToggleButton_OnIsCheckedChanged(object sender, RoutedEventArgs e)
    {
        var app = Application.Current;
        if (app is not null)
        {
            var theme = app.ActualThemeVariant;
            app.RequestedThemeVariant = theme == ThemeVariant.Dark ? ThemeVariant.Light : ThemeVariant.Dark;
        }
    }

    private void TextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        if (sender is TextBox tb)
        {
            tb.CaretIndex = tb.Text?.Length ?? 0;
        }
    }
}