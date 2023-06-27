using Ava.SocketTool.Extensions;
using Avalonia.Controls;

namespace Ava.SocketTool.Views.Dialog;

public class DialogBaseUserControl : UserControl
{
    public void CloseDialog()
    {
        OverlayExtension.CloseDialog();
    }
}