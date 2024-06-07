namespace spacebattle;
using Hwdtech;

public class GenerateGameUObjectsCommand : ICommand
{
    private readonly int _uObjectCount;

    public GenerateGameUObjectsCommand(int uObjectCount) => _uObjectCount = uObjectCount;

    public void Execute()
    {
        var gameUObjectRegistry = IoC.Resolve<IDictionary<int, IUObject>>("Game.UObject.Registry");

        Enumerable.Range(0, _uObjectCount).ToList().ForEach(
            i => gameUObjectRegistry.Add(i, IoC.Resolve<IUObject>("Game.UObject.Create"))
        );
    }
}
