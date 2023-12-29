using Hwdtech;
namespace spacebattle
{
    public class CollisionCommand : ICommand
    {
        private readonly IUObject _uObj_f, _uObj_s;
        public CollisionCommand(IUObject uObj_f, IUObject uObj_s)
        {
            _uObj_f = uObj_f;
            _uObj_s = uObj_s;
        }
        public void Execute()
        {
            var posFirst = IoC.Resolve<Vector>("Game.UObject.GetProperty", _uObj_f, "Position");
            var velFirst = IoC.Resolve<Vector>("Game.UObject.GetProperty", _uObj_f, "Velocity");
            var posSecond = IoC.Resolve<Vector>("Game.UObject.GetProperty", _uObj_s, "Position");
            var velSecond = IoC.Resolve<Vector>("Game.UObject.GetProperty", _uObj_s, "Velocity");
            var solvedCollision = IoC.Resolve<List<float>>("Game.UObject.SolveCollision", posFirst, posSecond, velFirst, velSecond);
            var collisionTree = IoC.Resolve<IDictionary<float, object>>("Game.Command.BuildCollisionTree");

            solvedCollision.ForEach(variation => collisionTree = (IDictionary<float, object>)collisionTree[variation]);

            IoC.Resolve<ICommand>("Game.Event.Collision", _uObj_f, _uObj_s).Execute();
        }
    }
}
