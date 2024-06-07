namespace spacebattle;
using Hwdtech;

public class FleetPositionIterator : IEnumerator<object>
{
    private readonly IList<Vector> _fleetPositions;
    private int _currentPositionIndex;

    public FleetPositionIterator()
    {
        _currentPositionIndex = 0;
        _fleetPositions = IoC.Resolve<List<Vector>>("Game.FleetPositions");
    }

    public object Current => _fleetPositions[_currentPositionIndex];

    public bool MoveNext() => ++_currentPositionIndex < _fleetPositions.Count;

    public void Reset() => _currentPositionIndex = 0;

    public void Dispose() => throw new NotImplementedException();
}