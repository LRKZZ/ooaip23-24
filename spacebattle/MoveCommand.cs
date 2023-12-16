namespace spacebattle
{
    public class MoveCommand : ICommand
    {
        private readonly IMovable obj;

        public MoveCommand(IMovable movable)
        {
            obj = movable;
        }

        public void Execute()
        {
            obj.Position += obj.Velocity;
        }
    }
}
