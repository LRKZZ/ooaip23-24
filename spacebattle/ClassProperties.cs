using System.Reflection;

namespace spacebattle
{
    public class ClassProperties
    {
        public string Name;
        public string PropertyType;
        public bool CanRead;
        public bool CanWrite;

        public ClassProperties(PropertyInfo property)
        {
            Name = property.Name;
            PropertyType = Format(property.PropertyType);
            CanRead = property.CanRead;
            CanWrite = property.CanWrite;
        }
        public static string Format(Type property)
        {
            var type = property.Name;
            if (property.IsGenericType && property.GetGenericTypeDefinition() == typeof(Dictionary<,>))
            {
                var genericArguments = property.GetGenericArguments();
                return $"Dictionary<{genericArguments[0].Name.ToLower()}, {genericArguments[1].Name.ToLower()}>";
            }
            else
            {
                return type;
            }
        }
    }
}
