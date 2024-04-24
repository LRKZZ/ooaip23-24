namespace spacebattle
{
    public class MessageHandler : ICommand
    {
        private readonly IMessage _message;

        public MessageHandler(IMessage message) 
        {
            _message = message;
        }
        public void Execute()
        {
        }
    }
}