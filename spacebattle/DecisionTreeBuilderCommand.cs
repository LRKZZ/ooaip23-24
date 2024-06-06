using Hwdtech;

namespace spacebattle
{
    public class DecisionTreeBuilderCommand : ICommand
    {
        private readonly IFileReader _reader;

        public DecisionTreeBuilderCommand(IFileReader reader)
        {
            _reader = reader;
        }

        public void Execute()
        {
            var arrays = _reader.ReadFile();

            arrays.ForEach(array =>
            {
                var node = IoC.Resolve<Dictionary<float, object>>("Game.Collisions.Tree");
                array.ToList().ForEach(num =>
                {
                    node.TryAdd(num, new Dictionary<float, object>());
                    node = (Dictionary<float, object>)node[num];
                });
            });
        }
    }
}
