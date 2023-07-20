using System.Text;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;
using SocketServer.Encoder;

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


    private void SelectingItemsControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (sender is ComboBox comboBox)
        {
            var index = comboBox.SelectedIndex;
            if (index == 1)
            {
                DefaultEncoder.Encoding = Encoding.UTF8;
            }
            else
            {
                DefaultEncoder.Encoding = Encoding.GetEncoding("GBK");
            }
        }
    }
}