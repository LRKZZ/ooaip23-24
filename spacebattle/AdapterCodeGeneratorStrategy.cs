namespace spacebattle;

using System.Reflection;
using SpaceBattle;

public class AdapterCodeGeneratorStrategy : IStrategy
{
    public object Strategy(params object[] args)
    {
        var adapterType = (Type)args[0];
        var targetType = (Type)args[1];

        var builder = new AdapterBuilder(adapterType, targetType);

        var properties = adapterType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var p in properties)
        {
            builder.CreateProperty(p);
        }

        return builder.Build();
    }
}
