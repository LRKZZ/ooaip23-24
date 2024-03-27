namespace spacebattle;

public class FileLoggingExceptionHandler : IExceptionHandler
{
    private readonly string _logFilePath;

    public FileLoggingExceptionHandler(string logFilePath)
    {
        _logFilePath = logFilePath;
    }

    public void HandleException(Exception exception, string commandName)
    {
        var errorMessage = $"Ошибка в команде: {commandName}\nИсключение: {exception}\n";
        File.AppendAllText(_logFilePath, errorMessage);
    }
}