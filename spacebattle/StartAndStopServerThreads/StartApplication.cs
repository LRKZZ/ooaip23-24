namespace spacebattle;
using Hwdtech;

public class StartApplication 
{
    private readonly int _threadCount;

    public StartApplication(int threadCount)
    {
        _threadCount = threadCount;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("ConsoleOutputStrategy", "press any button for start").Execute();
        IoC.Resolve<ICommand>("StartAppStrategy", _threadCount).Execute();
        IoC.Resolve<ICommand>("ConsoleOutputStrategy", "press any button for stop").Execute();
        IoC.Resolve<ICommand>("StopServerStrategy").Execute();
    }

}