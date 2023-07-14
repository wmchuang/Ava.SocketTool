using System;
using System.Globalization;
using System.Net;
using Avalonia.Data.Converters;

namespace Ava.SocketTool.Convert;

/// <summary>
/// IpAddress 转换成字符串IP
/// </summary>
public class IpAddressToIpConverter : IValueConverter
{
    public static readonly IpAddressToIpConverter Instance = new();

    /// <summary>
    /// 将数据从源（通常是数据源）转换为目标（通常是UI元素）所需的类型
    /// </summary>
    /// <param name="value">要转换的值（源值）</param>
    /// <param name="targetType">要转换为的目标类型。通常用于指示转换后的数据类型</param>
    /// <param name="parameter">可选的附加参数，供转换过程中使用</param>
    /// <param name="culture">转换所使用的区域性（例如，格式化数字或日期时）</param>
    /// <returns></returns>
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IPAddress source)
        {
            return source.ToString();
        }

        return IPAddress.Loopback;
    }

    /// <summary>
    /// 将目标值（用户输入或 UI 元素的值）转换回源值
    /// </summary>
    /// <param name="value">要转换的值（目标值）</param>
    /// <param name="targetType">要转换为的目标类型，通常指示目标数据应该是什么类型</param>
    /// <param name="parameter">可选的附加参数，供转换过程中使用</param>
    /// <param name="culture">转换所使用的区域性</param>
    /// <returns></returns>
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string strValue && IPAddress.TryParse(strValue, out var ipAddress))
        {
            return ipAddress;
        }

        return null;
    }
}