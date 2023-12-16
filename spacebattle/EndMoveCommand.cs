using Hwdtech;

namespace spacebattle
{
    internal class EndMoveCommand : ICommand
    {
        private readonly Order _order;
        public EndMoveCommand(Order order) 
        {
            _order = order;
            //в данном классе мы получаем команду, которую необходимо удалить
            //объект в котором необходимо удалить комманду
            //саму очередь
            //по сути, удалять комманду необязательно
            //надо просто достать эту команду из очереди
        }

        public void Execute()
        {
            var command = new LongObjectCommand(_order.obj, _order.cmd);
            command.Inject();
            _order.args.dictionary.ToList().ForEach(e =>
                _order.obj.args.Add(e.Key, e.Value)
            );

            _order.obj.args.Add(
                _order.cmd,
                Hwdtech.IoC.Resolve<ICommand>(
                        _order.cmd,
                        _order.obj
                    )
                );
            IoC.Resolve<IQueue<ICommand>>("Game.Queue").Add(command);
        }
    }
}
