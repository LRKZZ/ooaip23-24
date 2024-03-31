namespace spacebattle;

public class FileLoggingExceptionHandler : ICommand
{
    private readonly string _message;
    private readonly string _path;

    public FileLoggingExceptionHandler(string message)
    {
        _message = message;
        _path = Path.GetTempFileName();
    }

    public void Execute()
    {
        var createText = _message + Environment.NewLine;
        File.AppendAllText(_path, createText);
    }

    public string GetFilePath()
    {
        return _path;
    }
}

