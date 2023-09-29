public class MoveCommand : ICommand
{
    private IMovable obj;

    public MoveCommand(IMovable movable)
    {
        obj = movable;
    }

    public void Execute()
    {
        obj.Position = Vector.Add(obj.Position, obj.Velocity)
    }
}