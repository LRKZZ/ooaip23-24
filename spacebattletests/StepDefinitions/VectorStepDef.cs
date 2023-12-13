using spacebattle;
using System;
using TechTalk.SpecFlow;
using Moq;
using TechTalk.SpecFlow.CommonModels;

namespace spacebattletests.VectorStepDef
{
    [Binding]
    public class VectorStepDef
    {
        object v1 = new Vector(0, 0);
        object? v2;
        bool ans;
        Exception? _ex;

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
        public void GivenNullVector()
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
