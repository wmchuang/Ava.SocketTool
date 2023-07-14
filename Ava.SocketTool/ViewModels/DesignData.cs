using System.Collections.ObjectModel;
using Ava.SocketTool.Models;
using Ava.SocketTool.ViewModels.Page;
using SuperSocket;

namespace Ava.SocketTool.ViewModels;

public static class DesignData
{
    public static MainViewModel ExampleMainViewModel { get; } = new MainViewModel()
    {
        CurrentSelectModel = new SocketTreeModel()
        {
            IsRun = false,
            
        },
        TreeDataList = new ObservableCollection<SocketTreeModel>()
        {
            new()
            {
                DisplayName = "TCP Server",
                Children = new ObservableCollection<SocketTreeModel>()
                {
                    new()
                    {
                        DisplayName = "127.0.0.0.1:[60000]",
                    },
                    new()
                    {
                        DisplayName = "127.0.0.0.1:[60001]",
                    }
                }
            }
        }
    };

    public static HandleViewModel ExampleHandleViewModel { get; } = new HandleViewModel()
    {
        CurrentSelectModel = new SocketTreeModel()
        {
            TypeEnum = NetTypeEnum.TcpClient,
            IsRun = false,
        }
    };
}