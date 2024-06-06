namespace spacebattle;

using System.Reflection;
using System.Text.RegularExpressions;

public class AdapterBuilder
{
    private readonly string adaptername;
    private readonly string adaptertypename;
    private readonly string targettypename;
    private string? properties;

    public AdapterBuilder(Type adaptertype, Type targettype)
    {
        adaptername = adaptertype.Name.Substring(1);
        adaptertypename = adaptertype.Name;
        targettypename = FormatGenericType(targettype);
    }

    public void CreateProperty(PropertyInfo p)
    {
        string get = string.Empty, set = string.Empty;

        var propertyname = FormatGenericType(p.PropertyType);

        if (p.CanRead)
        {
            get = $@"   get {{ return IoC.Resolve<{propertyname}>(""Game.{p.Name}.Get"", target); }}";
        }

        if (p.CanWrite)
        {
            set = $@"   set {{ IoC.Resolve<_ICommand.ICommand>(""Game.{p.Name}.Set"", target, value).Execute(); }}";
        }

        properties +=
        $@"
        public {propertyname} {p.Name} {{
            {get}
            {set}
        }}
        ";
    }

    private static string FormatGenericType(Type type)
    {
        var typeName = type.Name;

        if (type.IsGenericType)
        {
            typeName = typeName.Substring(0, typeName.IndexOf('`'));

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
        var result = @$"class {adaptername}Adapter : {adaptertypename} {{
        {targettypename} target;
        public {adaptername}Adapter({targettypename} target) => this.target = target; 
        {properties}
    }}";

        result = Regex.Replace(result, @"^\s+$[\r\n]*", string.Empty, RegexOptions.Multiline);
        return result;
    }
}
