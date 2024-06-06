using spacebattle;
namespace spacebattletests;
public class RemoveGameStrategyTests
{
    [Fact]
    public void Valid_Input_Strategy()
    {
        var gameId = 1;
        var strategy = new RemoveGameStrategy();

        var command = strategy.Execute(gameId);

        Assert.NotNull(command);
    }
}
