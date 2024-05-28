using Scriban;

namespace spacebattle
{
    public class AdapterBuilder
    {
        private readonly Type _typeOld;
        private readonly Type _typeNew;

        public AdapterBuilder(Type typeOld, Type typeNew)
        {
            _typeOld = typeOld;
            _typeNew = typeNew;
        }
        public string Build()
        {
            var propertiesNew = _typeNew.GetProperties().ToList();

            var list = new List<ClassProperties>();

            _typeNew.GetProperties().ToList().ForEach(property =>
            {
                list.Add(new ClassProperties(property));
            });

            var templateString = @"using Hwdtech;
namespace spacebattle;

public class {{new_type_name}}Adapter : {{new_type_name}}
{
	private readonly {{old_type_name}} _obj;
    	public {{new_type_name}}Adapter({{old_type_name}} obj) => _obj = obj;
    {{for property in (properties_new)}}
    	public {{property.property_type}} {{property.name}}
    	{
{{if property.can_read}}
		get
		{
			{
				return IoC.Resolve<{{property.property_type}}>(""IUObject.Get"", _obj, ""{{property.name}}"");
			}
		}{{end}}{{if property.can_write}}
		set
		{
			{
				IoC.Resolve<ICommand>(""IUObject.Set"", _obj, ""{{property.name}}"", value).Execute();
			}
		}{{end}}
	}{{end}}
}";
            var template = Template.Parse(templateString);
            var result = template.Render(new
            {
                new_type_name = _typeNew.Name,
                old_type_name = _typeOld.Name,
                properties_new = list,
            });
            return result;
        }
    }
}
