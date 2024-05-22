namespace spacebattle
{
    public interface IStrategy
    {
        public object Execute(params object[] args);
    }
}
