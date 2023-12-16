namespace spacebattle
{
    internal class LongObjectCommand : ICommand
    {
        // Данная команда должна содержать ссылку на команду, которая заменяет значение скорости в объекте на 0.
        // Добавить сюда ссылку на пустую команду
        // В методе Inject произвести замену команды в _object.args.GetP(_cmd)
        private readonly UObject _object;
        private readonly string _cmd;
        private readonly ReplaceCommand _replaceCommand;
        public LongObjectCommand(UObject obj, string cmd) 
        {
            _object = obj;
            _cmd = cmd;
            _replaceCommand = new ReplaceCommand(obj, cmd);
        }
        
        public void Inject()
        {
            _replaceCommand.Execute();
        }
        public void Execute() 
        {
            ((ICommand)_object.args.GetP(_cmd)).Execute();
            Hwdtech.IoC.Resolve<Queue<ICommand>>("Game.Commands").Enqueue(this);
        }
    }
}
