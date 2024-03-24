using Hwdtech;
namespace spacebattle
{
    public class StartApplication 
{
    private readonly int _threadCount;

    public StartApplication(int threadCount)
    {
        _threadCount = threadCount;
    }

    private static ICommand GetCommand(string key, params object[] args)
    {
        return IoC.Resolve<ICommand>(key, args);
    }

    public void Execute()
    {
        var commands = new List<ICommand>
        {
            GetCommand("ConsoleOutputStrategy", "press any button for start"),
            GetCommand("StartAppStrategy", _threadCount),
            GetCommand("ConsoleOutputStrategy", "press any button for stop"),
            GetCommand("StopServerStrategy")
        };

        foreach (var command in commands) 
        {
            command.Execute();
        }
    }
}
}