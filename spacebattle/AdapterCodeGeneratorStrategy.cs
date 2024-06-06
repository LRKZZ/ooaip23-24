namespace spacebattle;

using System.Reflection;
using SpaceBattle;

public class AdapterCodeGeneratorStrategy : IStrategy
{
    public object Strategy(params object[] paramsArray)
    {
        var sourceType = (Type)paramsArray[0];
        var destinationType = (Type)paramsArray[1];

        var adapterConstructor = new AdapterBuilder(sourceType, destinationType);

        var sourceProperties = sourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        foreach (var propertyItem in sourceProperties)
        {
            adapterConstructor.CreateProperty(propertyItem);
        }

        return adapterConstructor.Build();
    }
}
