using System.Collections.ObjectModel;
using System.Reactive;
using Ava.SocketTool.Models;
using ReactiveUI.Fody.Helpers;
using Ava.SocketTool.Extensions;
using Avalonia.Controls;
using ReactiveUI;

namespace Ava.SocketTool.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        
    }

    [Reactive]
    public ObservableCollection<NetType> NetTypes { get; set; } = new();
    
    

    public void InitData()
    {
        var list = EnumExtension.GetList<NetTypeEnum>();
        foreach (var item in list)
        {
            NetTypes.Add(new NetType()
            {
                Name = item.Description,
                TypeEnum = item.Type,
            });
        }
    }
    
    /// <summary>
    /// 创建
    /// </summary>
    public ReactiveCommand<TreeView,Unit> CreateCommand => CreateCommand<TreeView>(async tree =>
    {
        var n = tree;
        var t = n.SelectedItem;
    });

}