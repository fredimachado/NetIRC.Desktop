using MahApps.Metro.Controls;
using NetIRC.Connection;
using NetIRC.Desktop.ViewModels;
using NetIRC.Desktop.Views;
using NetIRC.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetIRC.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private readonly App app = (App)Application.Current;

        private Client client;

        private readonly MainViewModel mainViewModel;
        private ServerViewModel serverTab;

        public MainWindow()
        {
            InitializeComponent();

            mainViewModel = new MainViewModel(ShowSettingsWindowDialog, ShowAboutWindowDialog);
            DataContext = mainViewModel;

            ContentRendered += MainWindow_ContentRendered;
        }

        private void MainWindow_ContentRendered(object sender, EventArgs e)
        {
            ShowSettingsWindowDialog();
        }

        private void ShowSettingsWindowDialog()
        {
            new SettingsWindow(this).ShowDialog();
        }

        private void ShowAboutWindowDialog()
        {
            new AboutWindow(this).ShowDialog();
        }

        internal async Task ConnectAsync()
        {
            client = CreateClient();

            serverTab = new ServerViewModel(client);
            mainViewModel.Tabs.Add(serverTab);
            TabControl.SelectedIndex = 0;

            client.RegistrationCompleted += Client_RegistrationCompleted;

            mainViewModel.RegisterClient(client);

            await client.ConnectAsync();
        }

        private Client CreateClient()
        {
            var user = new User(Settings.Default.Nick, Settings.Default.RealName);

            var client = new Client(
                user,
                new TcpClientConnection(Settings.Default.ServerAddress, Convert.ToInt32(Settings.Default.ServerPort)));
            client.SetDispatcherInvoker(Dispatcher.Invoke);

            app.SetClient(client);

            return client;
        }

        private async void Client_RegistrationCompleted(object sender, EventArgs e)
        {
            var channel = Settings.Default.DefaultChannel;
            if (string.IsNullOrWhiteSpace(channel))
            {
                return;
            }

            await client.SendAsync(new JoinMessage(channel));
        }
    }
}
