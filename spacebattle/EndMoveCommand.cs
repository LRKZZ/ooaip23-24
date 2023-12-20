using Hwdtech;

namespace spacebattle
{
    public class EndMoveCommand : ICommand
    {
        private readonly IMoveCommandEndable _order;
        public EndMoveCommand(IMoveCommandEndable order)
        {
            _order = order;
        }

        public void Execute()
        {
            IoC.Resolve<string>("Game.UObject.DeleteProperty", _order.Target, _order.args);
            var command = _order.endCommand;
            var emptyCommand = IoC.Resolve<ICommand>("Game.Command.EmptyCommand");
            IoC.Resolve<IInjectable>("Game.Command.Inject", command, emptyCommand);
        }
    }
}
