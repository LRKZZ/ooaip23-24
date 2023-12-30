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

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.MoveWithFire", (object[] args) =>
            {
                return new string[] { "Move", "Fire" };
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.EmptyMacroCommand", (object[] args) =>
            {
                return Array.Empty<string>();
            }).Execute();
        }

        private string name = "";
        private Mock<IQueue> queue = new Mock<IQueue>();
        private Queue<ICommand> realQueue = new Queue<ICommand>();
        private Mock<IUObject> mockUObject = new Mock<IUObject>();
        private Mock<ICommand> moveCommand = new Mock<ICommand>();
        private Mock<ICommand> fireCommand = new Mock<ICommand>();

        [Given(@"Инициализирован IoC контейнер с необходимыми зависимостями\.")]
        public void GivenИнициализированIoCКонтейнерСНеобходимымиЗависимостями_()
        {
            StartLongOpTest();
            name = "Macro.MoveWithFire";
            moveCommand = new Mock<ICommand>();
            moveCommand.Setup(mc => mc.Execute()).Callback(() => { }).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Move", (object[] args) =>
            {
                return moveCommand.Object;
            }).Execute();

            fireCommand = new Mock<ICommand>();
            fireCommand.Setup(fc => fc.Execute()).Callback(() => { }).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Fire", (object[] args) =>
            {
                return fireCommand.Object;
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands." + name, (object[] args) => new MacroCommand(new MacroCommandBuilder("Game.Commands.MoveWithFire", (IUObject)args[0]).BuildCommands())).Execute();

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

            var orderDict = new Dictionary<string, object>();

            mockUObject.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);
            queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Inject", (object[] args) => new ReplaceCommand((ICommand)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Operation." + name, (object[] args) => { return new LongOperation(name, (IUObject)args[0]); }).Execute();
        }

        [Given(@"Инициализирован IoC контейнер с обычной командой\.")]
        public void GivenИнициализированIoCКонтейнерСНОбычнойКомандой_()
        {
            StartLongOpTest();
            name = "Move";
            moveCommand = new Mock<ICommand>();
            moveCommand.Setup(mc => mc.Execute()).Callback(() => { }).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Move", (object[] args) =>
            {
                return moveCommand.Object;
            }).Execute();

            fireCommand = new Mock<ICommand>();
            fireCommand.Setup(fc => fc.Execute()).Callback(() => { }).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Fire", (object[] args) =>
            {
                return fireCommand.Object;
            }).Execute();

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

            var orderDict = new Dictionary<string, object>();

            mockUObject.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);
            queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Inject", (object[] args) => new ReplaceCommand((ICommand)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Operation." + name, (object[] args) => { return new LongOperation(name, (IUObject)args[0]); }).Execute();
        }

        [Then(@"Команда успешно завершает выполнение\.")]
        public void ThenКомандаУспешноЗавершаетВыполнение_()
        {
            queue.Object.Take().Execute();
            moveCommand.Verify(mc => mc.Execute(), Times.Once());
            fireCommand.Verify(cfc => cfc.Execute(), Times.Never());
        }

        [When(@"Выполняется команда Game.Operation.Movement.")]
        public void WhenВыполняетсяКоманда_()
        {
            IoC.Resolve<LongOperation>("Game.Operation." + name, mockUObject.Object).Invoke();
        }

        [Then(@"Макро Команда успешно завершает выполнение\.")]
        public void ThenМакроКомандаУспешноЗавершаетВыполнение_()
        {
            queue.Object.Take().Execute();
            moveCommand.Verify(mc => mc.Execute(), Times.Once());
            fireCommand.Verify(cfc => cfc.Execute(), Times.Once());
        }

        [Given(@"Инициализирован IoC контейнер без активации команды.")]
        public void GivenИнициализированIoCКонтейнерБезАктивацииКоманды_()
        {
            StartLongOpTest();
            name = "Macro.EmptyMacroCommand";
            moveCommand.Setup(mc => mc.Execute()).Callback(() => { }).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Move", (object[] args) =>
            {
                return moveCommand.Object;
            }).Execute();

            fireCommand = new Mock<ICommand>();
            fireCommand.Setup(fc => fc.Execute()).Callback(() => { }).Verifiable();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Fire", (object[] args) =>
            {
                return fireCommand.Object;
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands." + name, (object[] args) => new MacroCommand(new MacroCommandBuilder("Game.Commands.EmptyMacroCommand", (IUObject)args[0]).BuildCommands())).Execute();

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

            var orderDict = new Dictionary<string, object>();

            mockUObject.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(orderDict.Add);
            queue.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(realQueue.Enqueue);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Inject", (object[] args) => new ReplaceCommand((ICommand)args[0])).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Operation." + name, (object[] args) => { return new LongOperation(name, (IUObject)args[0]); }).Execute();
        }

        [Then(@"Команда не вызывается и не выполняется\.")]
        public void ThenКомандаНеВызываетсяИНеВыполняется_()
        {
            queue.Object.Take().Execute();
            fireCommand.Verify(x => x.Execute(), Times.Never);
            moveCommand.Verify(x => x.Execute(), Times.Never);
        }
    }
}
