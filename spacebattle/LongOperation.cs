using Hwdtech;

namespace spacebattle
{
    public class LongOperation : IStrategy
    {
        private readonly string _name;
        private readonly IUObject _target;

        public LongOperation(string name, IUObject target)
        {
            _name = name;
            _target = target;
        }
        public void Invoke()
        {
            var command = IoC.Resolve<ICommand>("Game.Commands." + _name, _target);

            var injectableCommand = IoC.Resolve<IInjectable>("Game.Commands.Inject", command);

            IoC.Resolve<object>(
                "Game.IUObject.SetProperty",
                _target,
                $"Game.Commands.Inject.{_name}",
                injectableCommand
            );

            IoC.Resolve<IQueue>("Game.Queue").Add((ICommand)injectableCommand);
        }
    }
}
