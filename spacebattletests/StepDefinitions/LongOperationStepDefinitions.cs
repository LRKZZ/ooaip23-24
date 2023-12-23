using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace spacebattle
{
    [Binding]
    public class LongOperationStepDefinitions
    {
        public static void StartLongOpTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.IUObject.SetProperty",
            (object[] args) =>
            {
                var order = (IUObject)args[0];
                var key = (string)args[1];
                var value = args[2];

                order.SetProperty(key, value);
                return new object();
            }
        ).Execute();
        }

        private Mock<ICommand> mockCommand = new Mock<ICommand>();
        private readonly string name = "LongMacroCommand";
        private Mock<IQueue> queue = new Mock<IQueue>();
        private Queue<ICommand> realQueue = new Queue<ICommand>();
        private Mock<IUObject> mockUObject = new Mock<IUObject>();
        private Mock<ICommand> unactiveMockCommand = new Mock<ICommand>();

        [Given(@"Инициализирован IoC контейнер с необходимыми зависимостями\.")]
        public void GivenИнициализированIoCКонтейнерСНеобходимымиЗависимостями_()
        {
            StartLongOpTest();
            mockCommand = new Mock<ICommand>();
            mockCommand.Setup(x => x.Execute()).Verifiable();

            mockUObject = new Mock<IUObject>();

            queue = new Mock<IQueue>();
            realQueue = new Queue<ICommand>();
            queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);
            queue.Setup(q => q.Take()).Returns(() => realQueue.Dequeue());

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Queue",
                (object[] args) =>
                {
                    return queue.Object;
                }
            ).Execute();

            var startable = new Mock<IMoveStartable>();
            var order = new Mock<IUObject>();
            var orderDict = new Dictionary<string, object>();
            var properties = new Dictionary<string, object> {
                { "id", 1 },
            };

            startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
            startable.SetupGet(s => s.Order).Returns(order.Object);
            order.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);
            queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command." + name, (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ConvertToStartable", (object[] args) =>
            {
                var emptyStartableObject = new Mock<IMoveStartable>();
                emptyStartableObject.Setup(x => x.Order).Returns((IUObject)args[0]);
                emptyStartableObject.Setup(x => x.PropertiesOfOrder).Returns(new Dictionary<string, object>());
                return emptyStartableObject.Object;
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMoveCommand", (object[] args) => new StartMoveCommand((IMoveStartable)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Inject", (object[] args) => new ReplaceCommand(mockCommand.Object)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Operation." + name, (object[] args) => { return new LongOperation(name, (IUObject)args[0]).Invoke(); }).Execute();
        }

        [When(@"Выполняется команда Game.Operation.Movement.")]
        public void WhenВыполняетсяКоманда_()
        {
            IoC.Resolve<ICommand>("Game.Operation." + name, mockUObject.Object).Execute();
        }

        [Then(@"Команда успешно завершает выполнение\.")]
        public void ThenКомандаУспешноЗавершаетВыполнение_()
        {
            queue.Object.Take().Execute();
            mockCommand.Verify();
        }

        [Given(@"Инициализирован IoC контейнер без активации команды.")]
        public void GivenИнициализированIoCКонтейнерБезАктивацииКоманды_()
        {
            StartLongOpTest();
            mockCommand = new Mock<ICommand>();
            mockCommand.Setup(x => x.Execute());

            unactiveMockCommand = new Mock<ICommand>();
            unactiveMockCommand.Setup(u => u.Execute()).Verifiable();

            mockUObject = new Mock<IUObject>();

            queue = new Mock<IQueue>();
            realQueue = new Queue<ICommand>();

            queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);
            queue.Setup(q => q.Take()).Returns(() => realQueue.Dequeue());

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Queue",
                (object[] args) =>
                {
                    return queue.Object;
                }
            ).Execute();

            var startable = new Mock<IMoveStartable>();
            var order = new Mock<IUObject>();
            var orderDict = new Dictionary<string, object>();
            var properties = new Dictionary<string, object> {
                { "id", 1 },
            };

            startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
            startable.SetupGet(s => s.Order).Returns(order.Object);
            order.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);
            queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command." + name, (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.ConvertToStartable", (object[] args) =>
            {
                var emptyStartableObject = new Mock<IMoveStartable>();
                emptyStartableObject.Setup(x => x.Order).Returns((IUObject)args[0]);
                emptyStartableObject.Setup(x => x.PropertiesOfOrder).Returns(new Dictionary<string, object>());
                return emptyStartableObject.Object;
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.StartMoveCommand", (object[] args) => new StartMoveCommand((IMoveStartable)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Macro", (object[] args) => mockCommand.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Inject", (object[] args) => new ReplaceCommand(mockCommand.Object)).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Operation." + name, (object[] args) => { return new LongOperation(name, (IUObject)args[0]).Invoke(); }).Execute();
        }

        [Then(@"Команда не вызывается и не выполняется\.")]
        public void ThenКомандаНеВызываетсяИНеВыполняется_()
        {
            queue.Object.Take().Execute();
            unactiveMockCommand.Verify(x => x.Execute(), Times.Never);
        }
    }
}
