namespace spacebattle
{
    public interface Strategy
    {
        public object Execute(params object[] args);
    }
}
