public class RotateCommand : ICommand
{
    private IRotatable obj;

    public RotateCommand(IRotatable rotatable)
    {
        obj = rotatable;
    }
    // необходимо добавить обработку исключений, если нет значения угла или угловой скорости
    public void Execute()
    {
        obj.angle = Angle.CalculateAngle(obj.angle, obj.angleSpeed);
    }
}
