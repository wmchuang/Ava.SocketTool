using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

namespace Ava.SocketTool.Extensions;

public class EnumItemInfo<T>
{
    public T Type { get; set; }
    public string Description { get; set; } = string.Empty;
}

public static class EnumExtension
{
    public static List<EnumItemInfo<T>> GetList<T>() where T : Enum
    {
        List<EnumItemInfo<T>> netTypeListWithDescription = new List<EnumItemInfo<T>>();

        foreach (T item in Enum.GetValues(typeof(T)))
        {
            var description = GetEnumDescription((Enum)item);

            netTypeListWithDescription.Add(new EnumItemInfo<T>
            {
                Type = item,
                Description = description
            });
        }

        return netTypeListWithDescription;
    }

    private static string GetEnumDescription(this Enum value)
    {
        FieldInfo field = value.GetType().GetField(value.ToString());
        DescriptionAttribute attribute =
            Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return attribute == null ? value.ToString() : attribute.Description;
    }
}