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
            Port = 60000,
            IsStart = false,
            
        },
        TreeDataList = new ObservableCollection<SocketTreeModel>()
        {
            new()
            {
                Name = "TCP Server",
                Children = new ObservableCollection<SocketTreeModel>()
                {
                    new()
                    {
                        Name = "127.0.0.0.1:[60000]",
                    },
                    new()
                    {
                        Name = "127.0.0.0.1:[60001]",
                    }
                }
            }
        }
    };

    public static HandleViewModel ExampleHandleViewModel { get; } = new HandleViewModel()
    {
        CurrentSelectModel = new SocketTreeModel()
        {
            TypeEnum = NetTypeEnum.TcpServer,
            Ip = "127.0.0.1",
            Port = 6000,
            IsStart = false,
        }
    };
}