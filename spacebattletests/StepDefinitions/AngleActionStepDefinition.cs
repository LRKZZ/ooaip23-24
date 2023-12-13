using spacebattle;
using System;
using TechTalk.SpecFlow;
using Moq;
using System.Reflection.Metadata;
using System.Numerics;

namespace spacebattletests.AngleActionStepDefinition
{
    [Binding]
    public class StepDefinitions
    {
        object v1 = new Angle(0);
        object? v2;
        bool ans;
        Exception? _ex;

        [Given(@"первый угол равен (.*)")]
        public void GivenFirstVec(int p0)
        {
            v1 = new Angle(p0);
        }

        [Given(@"второй угол равен (.*)")]
        public void GivenSecondVector(int p0)
        {
            v2 = new Angle(p0);
        }

        [Given(@"второй угол неопределён")]
        public void GivenNullVector()
        {

        }

        [When(@"происходит сравнение углов")]
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