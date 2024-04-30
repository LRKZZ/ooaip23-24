namespace spacebattle;

public interface IStrategy
{
    object Execute(params object[] args);
}
