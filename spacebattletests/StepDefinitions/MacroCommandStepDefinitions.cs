using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace spacebattle
{
    [Binding]
    public class MacroCommandStepDefinitions
    {
        private Mock<ICommand> moveCommand = new Mock<ICommand>();
        private Mock<ICommand> fireCommand = new Mock<ICommand>();
        private IUObject? _object;
        private List<ICommand>? cmds;
        private MacroCommand? macroCommand;
        private Exception? _exception;
        public static void StartTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.MoveWithFire", (object[] args) =>
            {
                return new string[] { "Move", "Fire" };
            }).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.EmptyOrder", (object[] args) =>
            {
                return Array.Empty<string>();
            }).Execute();
        }
        [Given(@"есть приказ на перемещение и стрельбу")]
        public void GivenЕстьПриказНаПеремещениеИСтрельбу()
        {
            StartTest();
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

            _object = new Mock<IUObject>().Object;
            cmds = new MacroCommandBuilder("Game.Commands.MoveWithFire", _object).BuildCommands();
        }

        [When(@"выполняется соответствующий приказ")]
        public void WhenВыполняетсяСоответствующийПриказ()
        {
            macroCommand = new MacroCommand(cmds);
            macroCommand.Execute();
        }

        [Then(@"объект совершает последовательно перемещение и стрельбу")]
        public void ThenОбъектСовершаетПоследовательноПеремещениеИСтрельбу()
        {
            moveCommand.Verify(mc => mc.Execute(), Times.Once());
            fireCommand.Verify(cfc => cfc.Execute(), Times.Once());
        }

        [Given(@"ошибочно был передан пустой приказ")]
        public void GivenОшибочноБылПереданПустойПриказ()
        {
            StartTest();
            _object = new Mock<IUObject>().Object;
            cmds = new MacroCommandBuilder("Game.Commands.EmptyOrder", _object).BuildCommands();
        }

        [When(@"объект объект обрабатывает приказ")]
        public void WhenОбъектОбъектОбрабатываетПриказ()
        {
            try
            {
                macroCommand = new MacroCommand(cmds);
            }
            catch(Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"ошибка выполнения приказа")]
        public void ThenОшибкаВыполненияПриказа()
        {
            Assert.IsType<Exception>(_exception);
        }
    }
}
