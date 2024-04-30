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
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQueue", (object[] args) =>
        {

            if (IoC.Resolve<IDictionary<int, Queue<ICommand>>>("GetGameQueue").TryGetValue((int)args[0], out var queue))
            {
                return queue;
            }

            throw new Exception();
        }).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "InsertIntoQueue", (object[] args) =>
        {
            var id = (int)args[0];
            var cmd = (ICommand)args[1];

            var queue = IoC.Resolve<Queue<ICommand>>("GetQueue", id);
            return new ActionCommand(() => { queue.Enqueue(cmd); });
        }).Execute();
    }

    private static void SetupMockMessage(Mock<IMessage> mockMessage, dynamic testData)
    {
        mockMessage.SetupGet(x => x.cmdType).Returns(testData.CommandType);
        mockMessage.SetupGet(x => x.GameID).Returns(testData.GameID);
        mockMessage.SetupGet(x => x.GameItemID).Returns(testData.ItemID);
        mockMessage.SetupGet(x => x.Properties).Returns(testData.Properties);
    }

    [Fact]
    public void TestPositiveScenario()
    {
        var mockMessage = new Mock<IMessage>();
        var properties = new Dictionary<string, object> { { "test command", 1 } };
        var testData1 = new { CommandType = "CommandName", GameID = "1", ItemID = "1", Properties = properties };
        var testData2 = new { CommandType = "CommandName", GameID = "3", ItemID = "1", Properties = properties };

        SetupMockMessage(mockMessage, testData1);
        var command1 = CommandFactory.CreateCommand(mockMessage.Object);
        IoC.Resolve<ICommand>("InsertIntoQueue", int.Parse(testData1.GameID), command1).Execute();

        SetupMockMessage(mockMessage, testData2);
        var command2 = CommandFactory.CreateCommand(mockMessage.Object);
        IoC.Resolve<ICommand>("InsertIntoQueue", int.Parse(testData2.GameID), command2).Execute();
        IoC.Resolve<ICommand>("InsertIntoQueue", int.Parse(testData2.GameID), command2).Execute();

        Assert.True(currentGames[1].Count == 1);
        Assert.True(currentGames[2].Count == 0);
        Assert.True(currentGames[3].Count == 2);
    }
}
