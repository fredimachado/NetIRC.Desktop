using MvvmHelpers.Commands;
using NetIRC.Desktop.Core;
using NetIRC.Desktop.Messages;
using NetIRC.Messages;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace NetIRC.Desktop.ViewModels
{
    public class MainViewModel : BaseViewModel, IHandle<ConnectMessage>, IHandle<OpenQueryMessage>
    {
        public ObservableCollection<TabItemViewModel> Tabs { get; } = new ObservableCollection<TabItemViewModel>();

        private TabItemViewModel selectedTab;
        public TabItemViewModel SelectedTab
        {
            get => selectedTab;
            set => SetProperty(ref selectedTab, value);
        }

        public ICommand ShowSettingsWindow { get; }
        public ICommand ShowAboutWindow { get; }

        public MainViewModel(Action showSettingsAction, Action showAboutAction)
        {
            ShowSettingsWindow = new Command(showSettingsAction);
            ShowAboutWindow = new Command(showAboutAction);

            App.EventAggregator.SubscribeOnPublishedThread(this);
        }

        public async Task HandleAsync(ConnectMessage message, CancellationToken cancellationToken)
        {
            if (App.IsConnected)
            {
                MessageBox.Show("Client is already connected.");
                return;
            }

            var client = App.CreateClient();

            var serverTab = new ServerViewModel(client);
            Tabs.Add(serverTab);
            SelectedTab = serverTab;

            client.RegistrationCompleted += Client_RegistrationCompleted;

            client.Queries.CollectionChanged += Queries_CollectionChanged;
            client.Channels.CollectionChanged += Channels_CollectionChanged;

            await client.ConnectAsync();
        }

        public Task HandleAsync(OpenQueryMessage message, CancellationToken cancellationToken)
        {
            App.Client.Queries.GetQuery(message.User);

            var tab = FindQueryTab(message.User);
            if (tab != null)
            {
                SelectedTab = tab;
            }

            return Task.CompletedTask;
        }

        private async void Client_RegistrationCompleted(object sender, EventArgs e)
        {
            var channel = Settings.Default.DefaultChannel;
            if (string.IsNullOrWhiteSpace(channel))
            {
                return;
            }

            await App.Client.SendAsync(new JoinMessage(channel));
        }

        private void Queries_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Query query in e.NewItems)
            {
                App.Dispatcher.Invoke(() => Tabs.Add(new QueryViewModel(query)));
            }
        }

        private void Channels_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Channel channel in e.NewItems)
            {
                App.Dispatcher.Invoke(() => Tabs.Add(new ChannelViewModel(channel)));
            }
        }

        private TabItemViewModel FindQueryTab(User user)
        {
            return Tabs.OfType<QueryViewModel>()
                .FirstOrDefault(q => q.Query.User == user);
        }
    }
}
