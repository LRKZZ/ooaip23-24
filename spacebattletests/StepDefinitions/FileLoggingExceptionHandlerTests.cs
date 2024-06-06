namespace spacebattletests;
using spacebattle;

public class FileLoggingExceptionHandlerTests
{
    [Fact]
    public void Correct_Writer_ExecuteAsync()
    {
        var cmd = new FileLoggingExceptionHandler("ErrorWithCommand");
        cmd.Execute();

        var filePath = cmd.GetFilePath();
        var containsErrorWithCommand = File.ReadLines(filePath).Any(line => line.Contains("ErrorWithCommand"));

        Assert.True(containsErrorWithCommand, "File should contain ErrorWithCommand");
    }
}
