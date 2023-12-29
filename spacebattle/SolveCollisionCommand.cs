using Hwdtech;
namespace spacebattle
{
    public class SolveCollisionCommand : ICommand
    {
        public void Execute()
        {
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.SolveCollision", (object[] args) =>
            {
                var posFirst = (Vector)args[0];
                var posSecond = (Vector)args[1];
                var velFirst = (Vector)args[2];
                var velSecond = (Vector)args[3];

                var differences = posSecond.Floats.Zip(posFirst.Floats, (second, first) => second - first)
                    .Concat(velSecond.Floats.Zip(velFirst.Floats, (second, first) => second - first))
                    .ToList();

                return differences;
            }).Execute();
        }
    }
}
