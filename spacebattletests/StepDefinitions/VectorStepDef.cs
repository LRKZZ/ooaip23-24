using spacebattle;

namespace spacebattletests.VectorStepDef
{
    [Binding]
    public class VectorStepDef
    {
        private object v1 = new Vector(0, 0);
        private object? v2;
        private bool ans;
        private Exception? _ex;

        [Given(@"первый вектор равен \((.*), (.*)\)")]
        public void GivenFirstVec(int p0, int p1)
        {
            v1 = new Vector(p0, p1);
        }

        [Given(@"второй вектор равен \((.*), (.*)\)")]
        public void GivenSecondVector(int p0, int p1)
        {
            v2 = new Vector(p0, p1);
        }

        [Given(@"второй вектор равен null")]
        public static void GivenNullVector()
        {

        }

        [When(@"происходит сравнение векторов")]
        public void WhenVectorAction()
        {
            try
            {
                ans = v1.Equals(v2);
            }
            catch (Exception ex)
            {
                _ex = ex;
            }
        }

        [Then(@"получаем \((true|false)\)")]
        public void checkResult(bool expectedResult)
        {
            Assert.Equal(expectedResult, ans);
        }

        [Then(@"возникает ошибка")]
        public void ThrowEx()
        {
            Assert.IsType<Exception>(_ex);
        }
    }
}
