using System;
using System.Collections.Generic;
using Ava.SocketTool.Models.Dialog;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Avalonia.Metadata;

namespace Ava.SocketTool.DataTemplate;

public class NodeModelTemplateSelector : IDataTemplate
{
    // This Dictionary should store our shapes. We mark this as [Content], so we can directly add elements to it later.
    [Content]
    public Dictionary<string, IDataTemplate> AvailableTemplates { get; } = new Dictionary<string, IDataTemplate>();

    // Build the DataTemplate here
    public Control Build(object param)
    {
        if (param is NodeModel model)
        {
            var key = model.TypeEnum.ToString(); // Our Keys in the dictionary are strings, so we call .ToString() to get the key to look up
            if (key is null) // If the key is null, we throw an ArgumentNullException
            {
                throw new ArgumentNullException(nameof(param));
            }

            return AvailableTemplates[key].Build(param); // finally we look up the provided key and let the System build the DataTemplate for us
        }

        return null;
    }

    // Check if we can accept the provided data
    public bool Match(object data)
    {
        if (data is NodeModel model)
        {
            // Our Keys in the dictionary are strings, so we call .ToString() to get the key to look up
            var key = model.TypeEnum.ToString();

            return !string.IsNullOrEmpty(key)           // and the key must not be null or empty
                   && AvailableTemplates.ContainsKey(key); // and the key must be found in our Dictionary
        }

        return false;

    }
}