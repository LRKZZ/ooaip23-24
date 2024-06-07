namespace spacebattletests;
using Hwdtech;
using Hwdtech.Ioc;
using spacebattle;

public class TestFleetPositionIterator
{
    public TestFleetPositionIterator()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();
    }

    [Fact]
    public void SuccessfulIteration()
    {
        var positions = new List<Vector>{
            new Vector(0, 0),
            new Vector(1, 0),
        };

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.FleetPositions", (object[] args) => positions).Execute();
        var iterator = new FleetPositionIterator();

        Assert.Equal(positions[0], iterator.Current);
        Assert.True(iterator.MoveNext());
        Assert.Equal(positions[1], iterator.Current);
        Assert.False(iterator.MoveNext());

        iterator.Reset();
        Assert.Equal(positions[0], iterator.Current);

        Assert.Throws<NotImplementedException>(() => iterator.Dispose());
    }

    [Fact]
    public void IterationThrowsOutOfRangeException()
    {
        var positions = new List<Vector>{
            new Vector(0, 0),
        };

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.FleetPositions", (object[] args) => positions).Execute();
        var iterator = new FleetPositionIterator();

        Assert.Equal(positions[0], iterator.Current);
        Assert.False(iterator.MoveNext());
        Assert.Throws<ArgumentOutOfRangeException>(() => iterator.Current);
    }
}
