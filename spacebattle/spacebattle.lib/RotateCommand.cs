public class RotateCommand : ICommand
{
    private IRotatable obj;

    public RotateCommand(IRotatable rotatable)
    {
        obj = rotatable;
    }

    public void Execute()
    {
        obj.Angle = Angle.CalculateAngle(obj.angle, obj.angleSpeed);
    }
}
