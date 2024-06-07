namespace spacebattle;
using Hwdtech;

public class AssignFuelToUObjectsCommand : ICommand
{
    private readonly IEnumerable<IUObject> _targetGameUObjects;
    private readonly double _specifiedFuelVolume;

    public AssignFuelToUObjectsCommand(IEnumerable<IUObject> targetGameUObjects, double specifiedFuelVolume)
    {
        _targetGameUObjects = targetGameUObjects;
        _specifiedFuelVolume = specifiedFuelVolume;
    }

    public void Execute()
    {
        _targetGameUObjects.ToList().ForEach(
            uObject => IoC.Resolve<ICommand>("Game.UObject.AssignFuel", uObject, "Fuel", _specifiedFuelVolume).Execute()
        );
    }
}
