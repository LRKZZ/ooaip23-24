namespace spacebattle
{
    public interface IUObject
    {
        public object GetProperty(string name);
        public void SetProperty(string name, object value);
        public void DeleteProperty(string name);
    }
}
