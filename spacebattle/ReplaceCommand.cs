namespace spacebattle
{
    public class ReplaceCommand : ICommand, IInjectable
    {
        private ICommand _cmd;

        public ReplaceCommand(ICommand cmd) => _cmd = cmd;

        public void Execute() => _cmd.Execute();

        public void Inject(ICommand obj) => _cmd = obj;
    }
}
