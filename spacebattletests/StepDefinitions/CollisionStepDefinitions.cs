using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace spacebattle
{
    [Binding]
    public class CollisionStepDefinitions
    {
        public static void StartCollisionTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.UObject.GetProperty", (object[] args) => new Vector(1, 1)).Execute();

            new SolveCollisionCommand().Execute();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.CheckCollision", (object[] args) => new CollisionCommand((IUObject)args[0], (IUObject)args[1])).Execute();
        }

        private Mock<ICommand> mockCommand = new Mock<ICommand>();
        private ICommand checkCollisionCommand;
        private Mock<IDictionary<float, object>> mockDictionary = new Mock<IDictionary<float, object>>();

        [Given(@"команда просчитать коллизию")]
        public void GivenКомандаПросчитатьКоллизию()
        {
            StartCollisionTest();

            mockCommand = new Mock<ICommand>();
            mockCommand.Setup(c => c.Execute()).Verifiable();

            var mockDictionary = new Mock<IDictionary<float, object>>();
            mockDictionary.SetupGet(d => d[It.IsAny<float>()]).Returns(mockDictionary.Object);

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.BuildCollisionTree", (object[] args) => mockDictionary.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Event.Collision", (object[] args) => mockCommand.Object).Execute();

            var mockUObject = new Mock<IUObject>();

            checkCollisionCommand = IoC.Resolve<ICommand>("Game.Command.CheckCollision", mockUObject.Object, mockUObject.Object);
        }

        [When(@"выполняется команда проверки коллизий")]
        public void WhenВыполняетсяКомандаПроверкиКоллизий()
        {
            checkCollisionCommand.Execute();
        }

        [Then(@"команда-обработчик коллизий должна быть вызвана")]
        public void ThenКоманда_ОбработчикКоллизийДолжнаБытьВызвана()
        {
            mockCommand.Verify();
        }

        [Given(@"коллизии нет в дереве решений")]
        public void GivenКоллизииНетВДеревеРешений()
        {
            StartCollisionTest();

            mockCommand = new Mock<ICommand>();
            mockCommand.Setup(c => c.Execute()).Verifiable();

            mockDictionary = new Mock<IDictionary<float, object>>();
            mockDictionary.SetupGet(d => d[It.IsAny<float>()]).Throws(new Exception()).Verifiable();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Command.BuildCollisionTree", (object[] args) => mockDictionary.Object).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Event.Collision", (object[] args) => mockCommand.Object).Execute();

            var mockUObject = new Mock<IUObject>();

            checkCollisionCommand = IoC.Resolve<ICommand>("Game.Command.CheckCollision", mockUObject.Object, mockUObject.Object);
        }

        [When(@"выполняется команда проверки коллизий с ошибкой")]
        public void WhenВыполняетсяКомандаПроверкиКоллизийСОшибкой()
        {
            Assert.Throws<Exception>(() => checkCollisionCommand.Execute());
        }

        [Then(@"команда-обработчик коллизий не вызывается")]
        public void ThenКоманда_ОбработчикКоллизийНеВызывается()
        {
            mockDictionary.Verify(d => d[It.IsAny<float>()], Times.Once());
            mockCommand.Verify(command => command.Execute(), Times.Never());
        }
    }
}
