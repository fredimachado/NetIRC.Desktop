using MvvmHelpers.Commands;
using NetIRC.Desktop.Messages;
using NetIRC.Messages;
using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Input;

namespace NetIRC.Desktop.ViewModels
{
    public class ChannelViewModel : TabItemViewModel
    {
        public Channel Channel { get; }

        public ICommand SortUsersCommand { get; }

        public ICommand OpenQueryCommand { get; }

        public ChannelViewModel(Channel channel)
        {
            Channel = channel;

            channel.Messages.CollectionChanged += Messages_CollectionChanged;

            SendMessageCommand = new AsyncCommand(SendChannelMessage);

            SortUsersCommand = new Command(SortUsers);

            OpenQueryCommand = new AsyncCommand<ChannelUser>(OpenQuery);
        }

        private void SortUsers(object items)
        {
            var view = CollectionViewSource.GetDefaultView(items) as ListCollectionView;
            view.CustomSort = new ChannelUserComparer();
        }

        private async Task OpenQuery(ChannelUser channelUser)
        {
            await App.EventAggregator.PublishOnUIThreadAsync(new OpenQueryMessage(channelUser.User));
        }

        private async Task SendChannelMessage()
        {
            if (string.IsNullOrWhiteSpace(Message))
            {
                return;
            }

            Messages.Add(Models.Message.Sent(new ChannelMessage(App.Client.User, Channel, Message)));
            await App.Client.SendAsync(new PrivMsgMessage(Channel.Name, Message));

            Message = string.Empty;
        }

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (ChannelMessage message in e.NewItems)
            {
                App.Dispatcher.Invoke(() => Messages.Add(Models.Message.Received(message)));
            }
        }

        public override string ToString() => Channel.Name;
    }
}
