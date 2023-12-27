using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using spacebattle;

namespace spacebattletests.StepDefinitions
{

    [Binding]
    public class StartMoveCommandStepDefinitions
    {
        private Mock<IQueue> queue = new Mock<IQueue>();
        private Queue<spacebattle.ICommand> realQueue = new Queue<spacebattle.ICommand>();
        private Mock<IMoveStartable> startable = new Mock<IMoveStartable>();
        private Mock<IUObject> order = new Mock<IUObject>();
        private Dictionary<string, object> orderDict = new Dictionary<string, object>();
        private Dictionary<string, object> properties = new Dictionary<string, object>();

        public static void StartScope()
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

            var longMoveCommand = new Mock<spacebattle.ICommand>().Object;
            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Commands.LongMove",
                (object[] args) =>
                {
                    return longMoveCommand;
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Commands.Inject",
                (object[] args) =>
                {
                    return new ReplaceCommand((spacebattle.ICommand)args[0]);
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EmptyCommand", (object[] args) => new EmptyCommand()).Execute();
        }

        [Given(@"я выполняю команду начала движения")]
        public void GivenЯВыполняюКомандуНачалаДвижения()
        {
            StartScope();
            queue = new Mock<IQueue>();
            realQueue = new Queue<spacebattle.ICommand>();
            queue.Setup(q => q.Add(It.IsAny<spacebattle.ICommand>())).Callback(realQueue.Enqueue);
            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Queue",
                (object[] args) =>
                {
                    return queue.Object;
                }
            ).Execute();

            startable = new Mock<IMoveStartable>();
            order = new Mock<IUObject>();
            orderDict = new Dictionary<string, object>();
            properties = new Dictionary<string, object> {
            { "id", 1 },
        };

            startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
            startable.SetupGet(s => s.Order).Returns(order.Object);
            order.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);
            queue.Setup(q => q.Add(It.IsAny<spacebattle.ICommand>())).Callback(realQueue.Enqueue);
        }

        [When(@"команда успешно выполняется")]
        public void WhenКомандаУспешноВыполняется()
        {
            var startMoveCommand = new StartMoveCommand(startable.Object);
            startMoveCommand.Execute();
        }

        [Then(@"она добавляется в очередь")]
        public void ThenОнаДобавляетсяВОчередь()
        {
            Assert.NotEmpty(realQueue);
        }

        [Then(@"устанавливаются необходимые свойства")]
        public void ThenУстанавливаютсяНеобходимыеСвойства()
        {
            Assert.Contains("id", orderDict.Keys);
            Assert.Contains("Game.Commands.Inject.LongMove", orderDict.Keys);
        }

        [When(@"не могу установить свойства")]
        public void WhenНеМогуУстановитьСвойства()
        {
            startable = new Mock<IMoveStartable>();
            order = new Mock<IUObject>();
            orderDict = new Dictionary<string, object>();

            properties = new Dictionary<string, object> {
            { "position", new Vector(  2, 1 ) },
        };

            startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
            startable.SetupGet(s => s.Order).Returns(order.Object);
            order.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback(() => throw new NotImplementedException());
        }

        [Then(@"возникает исключение")]
        public void ThenВозникаетИсключение()
        {
            var startMoveCommand = new StartMoveCommand(startable.Object);
            Assert.Throws<NotImplementedException>(startMoveCommand.Execute);
        }

        [When(@"не могу добавить команду в очередь")]
        public void WhenНеМогуДобавитьКомандуВОчередь()
        {
            queue = new Mock<IQueue>();
            realQueue = new Queue<spacebattle.ICommand>();
            queue.Setup(q => q.Add(It.IsAny<spacebattle.ICommand>())).Callback(() => { });

            startable = new Mock<IMoveStartable>();
            order = new Mock<IUObject>();
            orderDict = new Dictionary<string, object>();
            properties = new Dictionary<string, object> {
            { "action", new Mock<spacebattle.ICommand>() }
        };

            startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
            startable.SetupGet(s => s.Order).Returns(order.Object);
            order.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);

            var startMoveCommand = new StartMoveCommand(startable.Object);
            startMoveCommand.Execute();
        }

        [Then(@"очередь остаётся пустой")]
        public void ThenОчередьОстаётсяПустой()
        {
            Assert.Empty(realQueue);
        }

        [When(@"не могу прочитать свойства заказа")]
        public void WhenНеМогуПрочитатьСвойстваЗаказа()
        {
            startable = new Mock<IMoveStartable>();
            startable.SetupGet(s => s.PropertiesOfOrder).Throws(new NotImplementedException());
        }

        [When(@"не могу прочитать заказ")]
        public void WhenНеМогуПрочитатьЗаказ()
        {
            startable = new Mock<IMoveStartable>();
            startable.SetupGet(s => s.Order).Throws(new NotImplementedException());
            properties = new Dictionary<string, object> {
            { "position", new Vector(  2, 1 ) },
        };

            startable.SetupGet(s => s.PropertiesOfOrder).Returns(properties);
        }

        private Mock<spacebattle.ICommand> _mockCommand = new Mock<spacebattle.ICommand>();

        [Given(@"заменяемая команда и внедряемая команда созданы")]
        public void GivenReplaceableAndInjectableCommandsCreated()
        {
            StartScope();
            _mockCommand = new Mock<spacebattle.ICommand>();
            _mockCommand.Setup(x => x.Execute()).Verifiable();
        }

        [When(@"внедряемая команда внедряется в заменяемую команду")]
        public void WhenInjectableCommandInjectedIntoReplaceableCommand()
        {
            var injectCommand = new ReplaceCommand(_mockCommand.Object);
            injectCommand.Inject(IoC.Resolve<spacebattle.ICommand>("Game.Command.EmptyCommand"));
            injectCommand.Execute();
        }

        [Then(@"заменяемая команда не выполняется")]
        public void ThenReplaceableCommandNotExecuted()
        {
            _mockCommand.Verify(m => m.Execute(), Times.Never());
        }
    }
}
