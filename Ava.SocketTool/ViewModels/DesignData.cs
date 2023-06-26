using System.Collections.ObjectModel;
using Ava.SocketTool.Models;

namespace Ava.SocketTool.ViewModels;

public static class DesignData
{
    public static MainWindowViewModel ExampleMainWindowViewModel { get; } = new MainWindowViewModel()
    {
        NetTypes = new ObservableCollection<NetType>()
        {
            new NetType()
            {
                Name = "TCP Server",
                Sons = new ObservableCollection<NetType>()
                {
                    new NetType()
                    {
                        Name = "1",
                    },
                    new NetType()
                    {
                        Name = "2"
                    }
                }
            },
            new NetType()
            {
                Name = "TCP Client",
                Sons = new ObservableCollection<NetType>()
                {
                    new NetType()
                    {
                        Name = "1",
                    },
                    new NetType()
                    {
                        Name = "2"
                    }
                }
            }
        }
    };
}