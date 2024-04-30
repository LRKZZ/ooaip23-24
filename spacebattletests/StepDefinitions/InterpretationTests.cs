namespace spacebattle;

using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class InterpretationTest
{
    private readonly Dictionary<int, Queue<ICommand>> currentGames = new Dictionary<int, Queue<ICommand>>();

    public InterpretationTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        currentGames.Add(1, new Queue<ICommand>());
        currentGames.Add(2, new Queue<ICommand>());
        currentGames.Add(3, new Queue<ICommand>());

        var command = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.CommandName", (object[] args) => command.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameQueue", (object[] args) => currentGames).Execute();
    }

    [Fact]
    public void PositiveTest()
    {
        var mockMessage = new Mock<IMessage>();
        var properties = new Dictionary<string, object> { { "initial velocity", 1 } };
        var jsonData1 = new { CommandType = "CommandName", GameID = "1", ItemID = "1", Properties = properties };
        var jsonData2 = new { CommandType = "CommandName", GameID = "3", ItemID = "1", Properties = properties };

        mockMessage.SetupGet(x => x.cmdType).Returns(jsonData1.CommandType);
        mockMessage.SetupGet(x => x.GameID).Returns(jsonData1.GameID);
        mockMessage.SetupGet(x => x.GameItemID).Returns(jsonData1.ItemID);
        mockMessage.SetupGet(x => x.Properties).Returns(jsonData1.Properties);

        var intCmd = new InterpretationCommands(mockMessage.Object);
        intCmd.Execute();

        mockMessage.SetupGet(x => x.cmdType).Returns(jsonData2.CommandType);
        mockMessage.SetupGet(x => x.GameID).Returns(jsonData2.GameID);
        mockMessage.SetupGet(x => x.GameItemID).Returns(jsonData2.ItemID);
        mockMessage.SetupGet(x => x.Properties).Returns(jsonData2.Properties);

        intCmd = new InterpretationCommands(mockMessage.Object);
        intCmd.Execute();
        intCmd.Execute();

        Assert.True(currentGames[1].Count == 1);
        Assert.True(currentGames[2].Count == 0);
        Assert.True(currentGames[3].Count == 2);
    }
}
