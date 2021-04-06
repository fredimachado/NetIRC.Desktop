namespace NetIRC.Desktop.Messages
{
    public class OpenQueryMessage
    {
        public ChannelUser ChannelUser { get; }

        public OpenQueryMessage(ChannelUser channelUser) => ChannelUser = channelUser;
    }
}
