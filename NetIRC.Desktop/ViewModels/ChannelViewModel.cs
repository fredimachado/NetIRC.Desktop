using MvvmHelpers.Commands;
using NetIRC.Messages;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace NetIRC.Desktop.ViewModels
{
    public class ChannelViewModel : TabItemViewModel
    {
        public Channel Channel { get; }

        public ChannelViewModel(Channel channel)
        {
            Channel = channel;

            channel.Messages.CollectionChanged += Messages_CollectionChanged;

            SendMessageCommand = new AsyncCommand(SendChannelMessage);
        }

        private async Task SendChannelMessage()
        {
            if (string.IsNullOrWhiteSpace(Message))
            {
                return;
            }

            Messages.Add(ViewModels.Message.Sent(new ChannelMessage(App.Client.User, Channel, Message)));
            await App.Client.SendAsync(new PrivMsgMessage(Channel.Name, Message));

            Message = string.Empty;
        }

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (ChannelMessage message in e.NewItems)
            {
                App.Dispatcher.Invoke(() => Messages.Add(ViewModels.Message.Received(message)));
            }
        }

        public override string ToString() => Channel.Name;
    }
}
