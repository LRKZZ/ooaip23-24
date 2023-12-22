using Hwdtech;

namespace spacebattle;

public class MacroCommandBuilder
{
    private readonly string _dependencyName;
    private readonly IUObject _obj;

    public MacroCommandBuilder(string dependencyName, IUObject obj)
    {
        _dependencyName = dependencyName;
        _obj = obj;
    }

    public List<ICommand> BuildCommands()
    {
        var cmds = new List<ICommand>();
        var cmdNames = IoC.Resolve<string[]>(_dependencyName);

        cmdNames.ToList().ForEach(cmd_name =>
        {
            cmds.Add(IoC.Resolve<ICommand>(
                cmd_name,
                _obj
                ));
        });

        return cmds;
    }
}
