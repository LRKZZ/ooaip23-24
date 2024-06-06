namespace spacebattle;

using System.Reflection;
using System.Text.RegularExpressions;

public class AdapterBuilder
{
    private readonly string _builderName;
    private readonly string _typeName;
    private readonly string _targetType;
    private string? _adapterProperties;

    public AdapterBuilder(Type sourceType, Type destinationType)
    {
        _builderName = sourceType.Name.Substring(1);
        _typeName = sourceType.Name;
        _targetType = FormatGenericType(destinationType);
    }

    public void CreateProperty(PropertyInfo prop)
    {
        string getProperty = string.Empty, setProperty = string.Empty;

        var propertyName = FormatGenericType(prop.PropertyType);

        if (prop.CanRead)
        {
            getProperty = $@"   get {{ return IoC.Resolve<{propertyName}>(""Game.{prop.Name}.Get"", target); }}";
        }

        if (prop.CanWrite)
        {
            setProperty = $@"   set {{ IoC.Resolve<_ICommand.ICommand>(""Game.{prop.Name}.Set"", target, value).Execute(); }}";
        }

        _adapterProperties +=
        $@"
        public {propertyName} {prop.Name} {{
            {getProperty}
            {setProperty}
        }}
        ";
    }

    private static string FormatGenericType(Type type)
    {
        var typeName = type.Name;

        if (type.IsGenericType)
        {
            typeName = typeName[..typeName.IndexOf('`')];

            var typeArgs = type.GetGenericArguments();
            var typeArgNames = new string[typeArgs.Length];
            for (var i = 0; i < typeArgs.Length; i++)
            {
                typeArgNames[i] = FormatGenericType(typeArgs[i]);
            }

            typeName = $"{typeName}<{string.Join(", ", typeArgNames)}>";
        }

        return typeName;
    }

    public string Build()
    {
        var result = $@"
public class {_builderName}Adapter : {_typeName}
{{
    private {_targetType} _internalTarget;

    public {_builderName}Adapter({_targetType} target)
    {{
        _internalTarget = target;
    }}

    {_adapterProperties}

    public override string ToString()
    {{
        return ""{_builderName}Adapter wrapping "" + _internalTarget.ToString();
    }}
}}";

        result = Regex.Replace(result, @"\b_ICommand\b", "Hwdtech");
        result = Regex.Replace(result, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
        return result;
    }
}
