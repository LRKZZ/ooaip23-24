namespace spacebattle;
public class MacroCommand : ICommand
{
    public readonly List<ICommand> _cmds;

    public MacroCommand(List<ICommand> cmds)
    {
        _cmds = cmds;
    }

    public void Execute()
    {
        _cmds.ForEach(cmd => cmd.Execute());
    }
}
