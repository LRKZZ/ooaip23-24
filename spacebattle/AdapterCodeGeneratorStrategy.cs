namespace spacebattle;
using System.Reflection;

public class AdapterCodeGeneratorStrategy : Strategy
{
    public object Execute(params object[] paramsArray)
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
