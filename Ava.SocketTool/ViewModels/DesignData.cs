using System.Collections.ObjectModel;
using Ava.SocketTool.Models;

namespace Ava.SocketTool.ViewModels;

public static class DesignData
{
    public static MainViewModel ExampleMainViewModel { get; } = new MainViewModel()
    {
        // TreeDataList = new ObservableCollection<Tr>()
        // {
        //     new NetType()
        //     {
        //         Name = "TCP Server",
        //         Children = new ObservableCollection<NetType>()
        //         {
        //             new NetType()
        //             {
        //                 Name = "1",
        //             },
        //             new NetType()
        //             {
        //                 Name = "2"
        //             }
        //         }
        //     },
        //     new NetType()
        //     {
        //         Name = "TCP Client",
        //         Children = new ObservableCollection<NetType>()
        //         {
        //             new NetType()
        //             {
        //                 Name = "1",
        //             },
        //             new NetType()
        //             {
        //                 Name = "2"
        //             }
        //         }
        //     }
        // }
    };
}