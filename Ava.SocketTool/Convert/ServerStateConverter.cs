using System;
using System.Globalization;
using Avalonia.Data.Converters;
using SuperSocket;

namespace Ava.SocketTool.Convert;

public class ServerStateConverter : IValueConverter
{
    public static readonly ServerStateConverter Instance = new ServerStateConverter();
    
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ServerState source)
        {
            if (source == ServerState.Started)
            {
                return true;
            }
        }

        return false;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}