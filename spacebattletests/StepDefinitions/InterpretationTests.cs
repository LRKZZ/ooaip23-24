namespace spacebattle;

using Hwdtech;
using Hwdtech.Ioc;
using Moq;

public class InterpretationTest
{
    private readonly Dictionary<int, Queue<ICommand>> _gameCommandQueues = new();

    public InterpretationTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        _gameCommandQueues.Add(1, new Queue<ICommand>());
        _gameCommandQueues.Add(2, new Queue<ICommand>());
        _gameCommandQueues.Add(3, new Queue<ICommand>());

        var command = new Mock<ICommand>();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Command.CommandName", (object[] args) => command.Object).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetGameQueue", (object[] args) => _gameCommandQueues).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "CreateCommand", (object[] args) => CommandFactory.Invoke(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "EnqueueCommand", (object[] args) => new GetGameQueue().Execute(args)).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "GetQueue", (object[] args) => new GetQueue().Execute(args)).Execute();
    }

    private static void SetupMockMessage(Mock<IMessage> mockMessage, dynamic testData)
    {
        mockMessage.SetupGet(x => x.cmdType).Returns(testData.CommandType);
        mockMessage.SetupGet(x => x.GameID).Returns(testData.GameID);
        mockMessage.SetupGet(x => x.GameItemID).Returns(testData.ItemID);
        mockMessage.SetupGet(x => x.Properties).Returns(testData.Properties);
    }

    [Fact]
    public void TestQueuesCorrect()
    {
        var mockMessage = new Mock<IMessage>();
        var properties = new Dictionary<string, object> { { "test command", 1 } };
        var testData1 = new { CommandType = "CommandName", GameID = 1, ItemID = 1, Properties = properties };
        var testData2 = new { CommandType = "CommandName", GameID = 3, ItemID = 1, Properties = properties };
        SetupMockMessage(mockMessage, testData1);

        var intCmd1 = new InterpretationCommand(mockMessage.Object);
        intCmd1.Execute();

        SetupMockMessage(mockMessage, testData2);
        intCmd1 = new InterpretationCommand(mockMessage.Object);
        intCmd1.Execute();
        intCmd1.Execute();

        Assert.True(_gameCommandQueues[1].Count == 1);
        Assert.True(_gameCommandQueues[2].Count == 0);
        Assert.True(_gameCommandQueues[3].Count == 2);
    }
    [Fact]
    public void QueueNotFoundForGameId()
    {
        var mockMessage = new Mock<IMessage>();
        var properties = new Dictionary<string, object> { { "test command", 1 } };
        var testData = new { CommandType = "CommandName", GameID = 4, ItemID = 1, Properties = properties };

        SetupMockMessage(mockMessage, testData);

        var interpretationCommand = new InterpretationCommand(mockMessage.Object);

        var exception = Assert.Throws<Exception>(() => interpretationCommand.Execute());
        Assert.Contains("Queue not found for the specified game ID.", exception.Message);
    }

    [Fact]
    public void ErrorWithCommandType()
    {
        var mockMessage = new Mock<IMessage>();
        var properties = new Dictionary<string, object> { { "test command", 1 } };
        var testData = new { CommandType = "EmptyCommand", GameID = 2, ItemID = 1, Properties = properties };
        SetupMockMessage(mockMessage, testData);
        var interpretationCommand = new InterpretationCommand(mockMessage.Object);

        var exception = Assert.ThrowsAny<Exception>(() => interpretationCommand.Execute());
        Assert.Contains("Unknown IoC dependency key: ", exception.Message);
    }

    [Fact]
    public void EmptyCommandTypeThrowsException()
    {
        var mockMessage = new Mock<IMessage>();
        var properties = new Dictionary<string, object> { { "test command", 1 } };
        var testData = new { CommandType = "", GameID = 2, ItemID = 1, Properties = properties };
        SetupMockMessage(mockMessage, testData);
        var interpretationCommand = new InterpretationCommand(mockMessage.Object);

        var exception = Assert.Throws<Exception>(() => interpretationCommand.Execute());
        Assert.Contains("CommandType cannot be empty", exception.Message);
    }
    [Fact]
    public void NullMessageInConstructorThrowsException()
    {
        Action createInterpretationCommand = () => new InterpretationCommand(null!);

        var exception = Assert.Throws<ArgumentNullException>(createInterpretationCommand);

        Assert.Contains("Message cannot be null", exception.Message);
    }
}
