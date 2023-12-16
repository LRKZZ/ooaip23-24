namespace spacebattle
{
    public class RotateCommand : ICommand
    {
        private readonly IRotatable obj;

        public RotateCommand(IRotatable rotatable)
        {
            obj = rotatable;
        }
        public void Execute()
        {
            obj.angle += obj.angleSpeed;
        }
    }
}
