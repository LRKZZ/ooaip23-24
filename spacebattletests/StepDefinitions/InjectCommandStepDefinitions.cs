using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace spacebattle
{
    [Binding]
    public class InjectCommandStepDefinition
    {
        private Mock<ICommand> _mockCommand = new Mock<spacebattle.ICommand>();

        [Given(@"заменяемая команда и внедряемая команда созданы для инъекции")]
        public void GivenReplaceableAndInjectableCommandsCreated()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.EmptyCommand", (object[] args) => new EmptyCommand()).Execute();
            _mockCommand = new Mock<ICommand>();
            _mockCommand.Setup(x => x.Execute()).Verifiable();
        }

        [When(@"внедряемая команда внедряется в заменяемую команду")]
        public void WhenInjectableCommandInjectedIntoReplaceableCommand()
        {
            var injectCommand = new ReplaceCommand(_mockCommand.Object);
            injectCommand.Inject(IoC.Resolve<ICommand>("Game.Command.EmptyCommand"));
            injectCommand.Execute();
        }

        [Then(@"заменяемая команда не выполняется")]
        public void ThenReplaceableCommandNotExecuted()
        {
            _mockCommand.Verify(m => m.Execute(), Times.Never());
        }
    }
}
