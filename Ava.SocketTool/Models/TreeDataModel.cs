using System.Collections.ObjectModel;
using ReactiveUI.Fody.Helpers;

namespace Ava.SocketTool.Models;

/// <summary>
/// Tree Data
/// </summary>
public class TreeDataModel  : ModelBase
{
    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// NetTypeEnum
    /// </summary>
    public NetTypeEnum TypeEnum { get; set; }

    
    /// <summary>
    /// Children data
    /// </summary>
    [Reactive] 
    public ObservableCollection<TreeDataModel> Children { get; set; } = new();
}