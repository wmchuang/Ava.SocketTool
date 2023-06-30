using System.Collections.ObjectModel;
using Ava.SocketTool.Models;
using SuperSocket;

namespace Ava.SocketTool.ViewModels;

public static class DesignData
{
    public static MainViewModel ExampleMainViewModel { get; } = new MainViewModel()
    {
        CurrentSelectModel = new SocketTreeModel()
        {
            Port = 60000,
            ServerStateModel = new ServerStateModel()
            {
                ServerState = ServerState.Starting
            }
            
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
}