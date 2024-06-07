namespace spacebattle;
using Hwdtech;

public class DeployFleetCommand : ICommand
{
    private readonly IEnumerable<IUObject> _fleetUObjects;

    public DeployFleetCommand(IEnumerable<IUObject> fleetUObjects) { _fleetUObjects = fleetUObjects; }

    public void Execute()
    {
        var fleetPositionIterator = IoC.Resolve<IEnumerator<object>>("Game.Iterator.FleetPosition");

        _fleetUObjects.ToList().ForEach(ship => IoC.Resolve<ICommand>("Game.Deploy.Ship", ship, fleetPositionIterator).Execute());

        fleetPositionIterator.Reset();
    }
}
