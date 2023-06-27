using Avalonia;
using Avalonia.Controls;

namespace Ava.SocketTool.Controls;

public class Overlay : ContentControl
{
    public static readonly StyledProperty<bool> IsOverlayVisibleProperty = AvaloniaProperty.Register<Overlay, bool>(nameof(IsOverlayVisible));

    public static readonly StyledProperty<object?> OverlayContentProperty = AvaloniaProperty.Register<Overlay, object?>(nameof(OverlayContent));

    public bool IsOverlayVisible
    {
        get => GetValue(IsOverlayVisibleProperty);
        set => SetValue(IsOverlayVisibleProperty, value);
    }

    public object? OverlayContent
    {
        get => GetValue(OverlayContentProperty);
        set => SetValue(OverlayContentProperty, value);
    }
}