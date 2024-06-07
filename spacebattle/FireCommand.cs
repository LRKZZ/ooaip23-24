using Hwdtech;

namespace spacebattle
{
    public class FireCommand : ICommand
    {
        private readonly IShootable _torpedo;
        public FireCommand(IShootable torpedo)
        {
            _torpedo = torpedo;
        }

        public void Execute()
        {
            var torpedo = IoC.Resolve<IUObject>("Create.Torpedo", _torpedo);
            var adapter = IoC.Resolve<IMoveStartable>("IMoveStartableAdapter", torpedo);
            new StartMoveCommand(adapter).Execute();
        }
    }
}
