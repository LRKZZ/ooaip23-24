using Hwdtech;
namespace spacebattle;

public class RemoveObjectFromMapCommand : ICommand
{
    private readonly int _idGame;

    public RemoveObjectFromMapCommand(int idGame)
    {
        _idGame = idGame;
    }
    public void Execute()
    {
        var objectDictionary = IoC.Resolve<IDictionary<int, IUObject>>("RetrieveObjects");
        if (!objectDictionary.TryGetValue(_idGame, out _))
        {
            throw new Exception("IUObject with specified Id not found in dictionary.");
        }

        objectDictionary.Remove(_idGame);
    }
}
