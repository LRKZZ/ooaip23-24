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

        var scope = IoC.Resolve<object>("Scopes.CreateNew", parentScope);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Update", scope).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueuePushStrategy", (object[] args) => new QueuePushStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "QueuePopStrategy", (object[] args) => new QueuePopStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RetrieveObjectFromMapStrategy", (object[] args) => new RetrieveObjectFromMapStrategy().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RetrieveQuantum", (object[] args) => (object)quantum).Execute();

        gameScopeMapping.Add(idGame, scope);
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Update", parentScope).Execute();

        return gameScopeMapping;
    }
}
