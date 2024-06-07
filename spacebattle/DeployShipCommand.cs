namespace spacebattle;
using Hwdtech;

public class DeployShipCommand : ICommand
{
    private readonly IUObject _targetGameUObject;
    private readonly IEnumerator<object> _shipPositionEnumerator;

    public DeployShipCommand(IUObject targetGameUObject, IEnumerator<object> shipPositionEnumerator)
    {
        _targetGameUObject = targetGameUObject;
        _shipPositionEnumerator = shipPositionEnumerator;
    }

    public void Execute()
    {
        IoC.Resolve<ICommand>("Game.UObject.SetPosition", _targetGameUObject, "Position", _shipPositionEnumerator.Current).Execute();
        _shipPositionEnumerator.MoveNext();
    }
}
