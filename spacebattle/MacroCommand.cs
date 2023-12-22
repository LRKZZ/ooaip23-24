namespace spacebattle;
public class MacroCommand : ICommand
{
    private readonly List<ICommand> _cmds;

    public MacroCommand(List<ICommand>? cmds)
    {
        _cmds = cmds ?? throw new Exception();
    }

    public void Execute()
    {
        _cmds.ForEach(cmd => cmd.Execute());
    }
}
