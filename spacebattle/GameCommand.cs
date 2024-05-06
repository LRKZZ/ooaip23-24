namespace spacebattle
{
    public class GameCommand : ICommand
    {
        private readonly Guid _gameId;
        private object _scope;
        private Queue<ICommand> _queue;
        public GameCommand(Guid gameId, object scope, Queue<ICommand> queue) 
        {
            _gameId = gameId;
            _scope = scope;
            _queue = queue;
        }
        public void Execute() 
        {

        }
    }
}
