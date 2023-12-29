using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using spacebattle;

namespace spacebattletests.StepDefinitions
{
    [Binding]
    public class EndMoveCommandStepDefinitions
    {
        private static void EndCommandStartTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.Inject", (object[] args) =>
            {
                var target = (IInjectable)args[0];
                var injectedCommand = (spacebattle.ICommand)args[1];
                target.Inject(injectedCommand);
                return target;
            }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EmptyCommand", (object[] args) => new EmptyCommand()).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EndMovement", (object[] args) => { return new EndMoveCommand((IMoveCommandEndable)args[0]); }).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.DeleteProperty", (object[] args) =>
            {
                var target = (IUObject)args[0];
                var properties = (List<string>)args[1];
                properties.ForEach(prop => target.DeleteProperty(prop));
                return "";
            }).Execute();
        }

        private Mock<IMoveCommandEndable> _mockEndable = new Mock<IMoveCommandEndable>();
        private Mock<spacebattle.ICommand> _mockCommand = new Mock<spacebattle.ICommand>();
        private Mock<IUObject> _target = new Mock<IUObject>();
        private List<string>? _keys;
        private Dictionary<string, object>? _characteristics;

        [Given(@"у игрового объекта задана команда движения")]
        public void GivenGameobjectHasMovementCommand()
        {
            EndCommandStartTest();
            _mockEndable = new Mock<IMoveCommandEndable>();
            _mockCommand = new Mock<spacebattle.ICommand>();
            _target = new Mock<IUObject>();
            _keys = new List<string>() { "Movement" };
            _characteristics = new Dictionary<string, object>();

            _target.Setup(t => t.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>((key, value) => _characteristics.Add(key, value));
            _target.Setup(t => t.DeleteProperty(It.IsAny<string>())).Callback<string>((string key) => _characteristics.Remove(key));
            _target.Setup(t => t.GetProperty(It.IsAny<string>())).Returns((string key) => _characteristics[key]);
            _target.Object.SetProperty("Movement", new ReplaceCommand(_mockCommand.Object));

            _mockEndable.SetupGet(e => e.Target).Returns(_target.Object);
            _mockEndable.SetupGet(e => e.args).Returns(_keys);
        }

        [When(@"выполняется команда завершения движения")]
        public void WhenEndMovementCommandExecuted()
        {
            IoC.Resolve<spacebattle.EndMoveCommand>("Game.Command.EndMovement", _mockEndable.Object).Execute();
        }

        [Then(@"исключение KeyNotFoundException генерируется при попытке доступа к свойству Движение")]
        public void ThenMovementPropertyRemovedAndExceptionThrown()
        {
            Assert.Throws<System.Collections.Generic.KeyNotFoundException>(() => _target.Object.GetProperty("Movement"));
        }

        [Given(@"заменяемая команда и внедряемая команда созданы")]
        public void GivenReplaceableAndInjectableCommandsCreated()
        {
            EndCommandStartTest();
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
