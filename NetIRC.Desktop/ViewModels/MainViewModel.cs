using NetIRC;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace NetIRC.Desktop.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        public ObservableCollection<TabItemViewModel> Tabs { get; } = new ObservableCollection<TabItemViewModel>();

        public void RegisterClient(Client client)
        {
            client.Queries.CollectionChanged += Queries_CollectionChanged;
            client.Channels.CollectionChanged += Channels_CollectionChanged;
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
    }
}
