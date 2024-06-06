using Hwdtech;

namespace spacebattle
{
    public class FireCommand : ICommand
    {
        private readonly IFirable _torpedo;
        private readonly Vector _speed;
        public FireCommand(IFirable torpedo, Vector speed)
        {
            _torpedo = torpedo;
            _speed = speed;
        }

        public void Execute()
        {
            var torpedo = IoC.Resolve<IUObject>("Create.Torpedo", _torpedo, _speed);
            var adapter = IoC.Resolve<IMoveStartable>("IMoveStartableAdapter", torpedo);
            new StartMoveCommand(adapter).Execute();
        }
    }
}
