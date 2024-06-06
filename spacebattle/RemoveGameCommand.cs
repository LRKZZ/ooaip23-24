using Hwdtech;
namespace spacebattle;

public class RemoveGameCommand : ICommand
{
    private readonly int _idGame;

    public RemoveGameCommand(int idGame)
    {
        _idGame = idGame;
    }

    public void Execute()
    {
        var gameMap = IoC.Resolve<IDictionary<int, IInjectable>>("GameMappings");
        var gameCommand = gameMap[_idGame];
        gameCommand.Inject(IoC.Resolve<ICommand>("EmptyCommand"));
        var gameScopeMapping = IoC.Resolve<IDictionary<int, object>>("GameScopeMapping");
        gameScopeMapping.Remove(_idGame);
    }
}
