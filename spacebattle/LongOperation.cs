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
        public object Invoke(params object[] args)
        {
            var cmd = IoC.Resolve<ICommand>("Game.Command." + _name, _target);

            var macroCmd = IoC.Resolve<ICommand>("Game.Command.Macro", cmd);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register",
            "Game.Commands.LongMove", (object[] args) =>
            { return macroCmd; }).Execute();

            var startObject = IoC.Resolve<IMoveStartable>("Game.ConvertToStartable", _target);

            var startCmd = IoC.Resolve<ICommand>("Game.Command.StartMoveCommand", startObject);

            return startCmd;
        }
    }
}
