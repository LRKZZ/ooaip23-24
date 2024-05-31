using Hwdtech;

namespace spacebattle
{
    public class StartMoveCommand : ICommand
    {
        private readonly IMoveStartable _startable;
        public StartMoveCommand(IMoveStartable startable)
        {
            _startable = startable;
        }

        public void Execute()
        {
            _startable.PropertiesOfOrder.ToList().ForEach(property => IoC.Resolve<ICommand>(
                "Game.IUObject.SetProperty",
                _startable.Order,
                property.Key,
                property.Value
            ).Execute());

            var longCmd = IoC.Resolve<ICommand>(
                "Game.Commands.LongMove",
                _startable.Order
            );

            var injectCmd = IoC.Resolve<IInjectable>("Game.Commands.Inject", longCmd);

            IoC.Resolve<ICommand>("Game.IUObject.SetProperty",
                _startable.Order,
                "Game.Commands.Inject.LongMove",
                injectCmd).Execute();

            IoC.Resolve<IQueue>("Game.Queue").Add((ICommand)injectCmd);
        }
    }
}
