using Hwdtech;
namespace spacebattle;

public class CreateGameStrategy : Strategy
{
    public object Execute(params object[] args)
    {
        var idGame = (int)args[0];
        var scope = IoC.Resolve<object>("RetrieveGameScope");
        var gameQueue = new Queue<ICommand>();
        _ = IoC.Resolve<ICommand>("GameCommand", gameQueue, scope);
        var gameMappings = IoC.Resolve<IDictionary<int, ICommand>>("GameMappings");

        IEnumerable<ICommand> commandList = new List<ICommand>();
        var macroCommand = IoC.Resolve<ICommand>("MacroCommand", commandList);
        var injectCommand = IoC.Resolve<ICommand>("InjectCommand", macroCommand);
        var repeatCommand = IoC.Resolve<ICommand>("RepeatCommand", injectCommand);

        _ = commandList.Append(repeatCommand);

        gameMappings.Add(idGame, injectCommand);
        return injectCommand;
    }
}
