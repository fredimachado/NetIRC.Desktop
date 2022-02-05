using NetIRC.Connection;
using NetIRC.Desktop.Core;
using NetIRC.Desktop.Messages;
using NetIRC.Desktop.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NetIRC.Desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public Client Client { get; private set; }

        public bool IsConnected { get; private set; }

        public static IEventAggregator EventAggregator { get; } = new EventAggregator();

        public Client CreateClient()
        {
            if (IsConnected)
            {
                return null;
            }

            var user = new User(Settings.Default.Nick, Settings.Default.RealName);

            var connection = new TcpClientConnection(Settings.Default.ServerAddress, Convert.ToInt32(Settings.Default.ServerPort));

            connection.Connected += (s, e) => IsConnected = true;
            connection.Disconnected += async (s, e) => await Disconnected();

            Client = new Client(user, connection);

            Client.SetDispatcherInvoker(Dispatcher.Invoke);

            return Client;
        }

        private Task Disconnected()
        {
            IsConnected = false;

            return EventAggregator.PublishOnUIThreadAsync(new ClientDisconnectedMessage());
        }
    }
}
