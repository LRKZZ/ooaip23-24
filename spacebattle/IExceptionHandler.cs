namespace spacebattle;
public interface IExceptionHandler
{
    void HandleException(Exception exception, string commandName);
}
