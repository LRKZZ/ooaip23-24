using Hwdtech;
namespace spacebattle;

public class GameStrategyScope : IStrategy
{
    public object Execute(params object[] args)
    {
        var idGame = (int)args[0];
        var parentScope = args[1];
        var quantum = (double)args[2];

        var gameScopeMapping = IoC.Resolve<IDictionary<int, object>>("GameScopeMapping");

        var scope = IoC.Resolve<object>("Scopes.New", parentScope);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", scope).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueuePushStrategy", (object[] strategyArgs) =>
        {
            return new QueuePushCommand((int)strategyArgs[0], (ICommand)strategyArgs[1]);
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueuePopStrategy", (object[] strategyArgs) => new QueuePopStrategy().Execute(strategyArgs)).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RetrieveObjectFromMapStrategy", (object[] strategyArgs) =>
        {
            var id = (int)strategyArgs[0];
            var objectDictionary = IoC.Resolve<IDictionary<int, IUObject>>("RetrieveObjects");
            if (!objectDictionary.TryGetValue(id, out var obj))
            {
                throw new Exception("IUObject with specified Id not found in dictionary.");
            }

            return obj;
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RetrieveQuantum", (object[] strategyArgs) => (object)quantum).Execute();

        gameScopeMapping.Add(idGame, scope);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", parentScope).Execute();

        return gameScopeMapping;
    }
}
