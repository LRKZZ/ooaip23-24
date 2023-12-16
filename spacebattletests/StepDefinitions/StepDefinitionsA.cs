using Moq;
using spacebattle;

namespace spacebattletests.RotateActionStepDefinitions
{
    [Binding]
    public class RotateActionStepDefinitions
    {
        private Exception excep = new Exception();
        private readonly Mock<IRotatable> rotateMock = new Mock<IRotatable>();

        [Given(@"космический корабль имеет угол наклона (.*) град к оси OX")]
        public void GivenCoordinates(int p0)
        {
            rotateMock.SetupProperty(m => m.angle);
            rotateMock.Object.angle = new Angle(p0);
        }

        [Given(@"имеет мгновенную угловую скорость (.*) град")]
        public void GivenSpeedVector(int p0)
        {
            rotateMock.SetupGet(m => m.angleSpeed).Returns(new Angle(p0));
        }

        [Given(@"космический корабль, угол наклона которого невозможно определить")]
        public void GivenImpossibleCoordinates()
        {
            rotateMock.Setup(m => m.angle).Throws<Exception>();
        }

        [Given(@"мгновенную угловую скорость невозможно определить")]
        public void GivenImpossibleSpeedVector()
        {
            rotateMock.Setup(m => m.angleSpeed).Throws<Exception>();
        }

        [Given(@"невозможно изменить уголд наклона к оси OX космического корабля")]
        public void GivenImpossibleAction()
        {
            rotateMock.Setup(m => m.angle).Throws<Exception>();
        }

        [When(@"происходит вращение вокруг собственной оси")]
        public void WhenMovingAction()
        {
            try
            {
                var rotateCommand = new RotateCommand(rotateMock.Object);
                rotateCommand.Execute();
            }
            catch (Exception ex)
            {
                excep = ex;
            }
        }

        [Then(@"угол наклона космического корабля к оси OX составляет (.*) град")]
        public void ThenSpaceshipMovingToCoordinates(int p0)
        {
            rotateMock.VerifySet(m => m.angle = new Angle(p0), Times.Once);
        }

        [Then(@"возникает ошибка вращения")]
        public void ThenThrowsExceptionRotating()
        {
            Assert.IsType<Exception>(excep);
        }
    }
}
