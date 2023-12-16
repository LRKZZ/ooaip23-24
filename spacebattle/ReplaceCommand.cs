namespace spacebattle
{
    internal class ReplaceCommand : ICommand
    {
        private readonly UObject _object;
        private readonly string _cmd;
        private readonly object emptyVector = new Vector(0, 0);
        public ReplaceCommand(UObject obj, string cmd) 
        {
            _object = obj;
            _cmd = cmd;
        }

        public void Execute() 
        {
            _object.args.Add(_cmd, emptyVector);
        }
    }
}
