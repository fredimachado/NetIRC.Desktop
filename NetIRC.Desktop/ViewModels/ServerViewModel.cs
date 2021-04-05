using MvvmHelpers.Commands;
using System.Threading.Tasks;

namespace NetIRC.Desktop.ViewModels
{
    public class ServerViewModel : TabItemViewModel
    {
        public ServerViewModel(Client client)
        {
            client.ServerMessages.CollectionChanged += ServerMessages_CollectionChanged;

            SendMessageCommand = new AsyncCommand(SendServerMessage);
        }

        private async Task SendServerMessage()
        {
            if (string.IsNullOrWhiteSpace(Message))
            {
                return;
            }

            if (Message.StartsWith("/"))
            {
                Messages.Add(ViewModels.Message.Received(new ServerMessage("Oops! Commands are not implemented yet...")));
                return;
            }

            await App.Client.SendRaw(Message);

            Message = string.Empty;
        }

        private void ServerMessages_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            foreach (ServerMessage message in e.NewItems)
            {
                App.Dispatcher.Invoke(() => Messages.Add(ViewModels.Message.Received(message)));
            }
        }

        public override string ToString() => Settings.Default.ServerName ?? "Server";
    }
}
