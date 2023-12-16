using Moq;
using spacebattle;

namespace spacebattletests.MovingActionStepDefinitions
{
    [Binding]
    public class MovingActionStepDefinition
    {
        private Exception excep = new Exception();
        private readonly Mock<IMovable> moveMock = new Mock<IMovable>();

        [Given(@"космический корабль находится в точке пространства с координатами \((.*), (.*)\)")]
        public void GivenCoordinates(int p0, int p1)
        {
            moveMock.SetupProperty(m => m.Position);
            moveMock.Object.Position = new Vector(p0, p1);
        }

        [Given(@"имеет мгновенную скорость \((.*), (.*)\)")]
        public void GivenSpeedVector(int p0, int p1)
        {
            moveMock.SetupGet(m => m.Velocity).Returns(new Vector(p0, p1));
        }

        [Given(@"космический корабль, положение в пространстве которого невозможно определить")]
        public void GivenImpossibleCoordinates()
        {
            moveMock.Setup(m => m.Position).Throws<Exception>();
        }

        [Given(@"скорость корабля определить невозможно")]
        public void GivenImpossibleSpeedVector()
        {
            moveMock.Setup(m => m.Velocity).Throws<Exception>();
        }

        [Given(@"изменить положение в пространстве космического корабля невозможно")]
        public void GivenImpossibleAction()
        {
            moveMock.Setup(m => m.Position).Throws<Exception>();
        }

        [When(@"происходит прямолинейное равномерное движение без деформации")]
        public void WhenMovingAction()
        {
            try
            {
                var moveCommand = new MoveCommand(moveMock.Object);
                moveCommand.Execute();
            }
            catch (Exception ex)
            {
                excep = ex;
            }
        }

        [Then(@"космический корабль перемещается в точку пространства с координатами \((.*), (.*)\)")]
        public void ThenSpaceshipMovingToCoordinates(int p0, int p1)
        {
            moveMock.VerifySet(m => m.Position = new Vector(p0, p1), Times.Once);
        }

        [Then(@"возникает ошибка перемещения")]
        public void ThenThrowsExceptionMoving()
        {
            Assert.IsType<Exception>(excep);
        }
    }
}
