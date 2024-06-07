namespace spacebattle;

public interface Strategy
{
    object Execute(params object[] args);

}
