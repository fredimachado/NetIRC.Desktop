using MvvmHelpers.Commands;
using NetIRC.Messages;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace NetIRC.Desktop.ViewModels
{
    public class QueryViewModel : TabItemViewModel
    {
        public Query Query { get; }

        public QueryViewModel(Query query)
        {
            Query = query;

            query.Messages.CollectionChanged += Messages_CollectionChanged;

            SendMessageCommand = new AsyncCommand(SendQueryMessage);
        }

        private async Task SendQueryMessage()
        {
            if (string.IsNullOrWhiteSpace(Message))
            {
                return;
            }

            Messages.Add(ViewModels.Message.Sent(new QueryMessage(App.Client.User, Message)));
            await App.Client.SendAsync(new PrivMsgMessage(Query.Nick, Message));

            Message = string.Empty;
        }

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (QueryMessage message in e.NewItems)
            {
                App.Dispatcher.Invoke(() => Messages.Add(ViewModels.Message.Received(message)));
            }
        }

        public override string ToString() => Query.Nick;
    }
}
