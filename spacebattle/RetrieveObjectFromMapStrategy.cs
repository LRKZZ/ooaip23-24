using Hwdtech;
namespace spacebattle;

public class RetrieveObjectFromMapStrategy : IStrategy
{
    public object Execute(params object[] args)
    {
        var id = (int)args[0];

        var objectDictionary = IoC.Resolve<IDictionary<int, IUObject>>("RetrieveObjects");
        if (!objectDictionary.TryGetValue(id, out var obj))
        {
            throw new Exception("IUObject with specified Id not found in dictionary.");
        }

        return obj;
    }
}
