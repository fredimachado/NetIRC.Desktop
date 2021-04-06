namespace NetIRC.Desktop.Messages
{
    public class OpenQueryMessage
    {
        public User User { get; }

        public OpenQueryMessage(User user) => User = user;
    }
}
