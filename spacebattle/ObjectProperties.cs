namespace spacebattle
{
    public class ObjectProperties : IDict<string, object>
    {
        public object GetP(string key)
        {
            return dictionary[key];
        }
        public void Add(string key, object value)
        {
            dictionary[key] = value;
        }
        public IDictionary<string, object> dictionary { get; } = new Dictionary<string, object>();
    }
}
