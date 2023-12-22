using Hwdtech.Ioc;
using Hwdtech;
using Moq;

namespace spacebattle
{
    [Binding]
    public class MacroCommandStepDefinitions
    {
        private Mock<ICommand>? moveCommand;
        private Mock<ICommand>? fireCommand;
        private IUObject? _object;
        private List<ICommand>? cmds;
        private MacroCommand? macroCommand;
        public static void StartTest() 
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.MoveWithFire", (object[] args) =>
            {
                return new string[] { "Move", "Fire" };
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
            fireCommand.Setup(cfc => cfc.Execute()).Callback(() => { }).Verifiable();
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
    }
}
