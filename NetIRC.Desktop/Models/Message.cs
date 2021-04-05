using System;

namespace NetIRC.Desktop.Models
{
    public class Message
    {
        public string From { get; }
        public string Text { get; }
        public DateTime Timestamp { get; }
        public bool IsSentByClient { get; }

        private Message(string from, string text, DateTime timestamp, bool isSentByClient)
        {
            From = from;
            Text = text;
            Timestamp = timestamp;
            IsSentByClient = isSentByClient;
        }

        public static Message Received(QueryMessage queryMessage) =>
            new Message(queryMessage.User.Nick, queryMessage.Text, queryMessage.Timestamp, isSentByClient: false);

        public static Message Sent(QueryMessage queryMessage) =>
            new Message(queryMessage.User.Nick, queryMessage.Text, queryMessage.Timestamp, isSentByClient: true);

        public static Message Received(ChannelMessage channelMessage) =>
            new Message(channelMessage.User.Nick, channelMessage.Text, channelMessage.Timestamp, isSentByClient: false);

        public static Message Sent(ChannelMessage channelMessage) =>
            new Message(channelMessage.User.Nick, channelMessage.Text, channelMessage.Timestamp, isSentByClient: true);

        public static Message Received(ServerMessage serverMessage) =>
            new Message(string.Empty, serverMessage.Text, serverMessage.Timestamp, isSentByClient: false);
    }
}
