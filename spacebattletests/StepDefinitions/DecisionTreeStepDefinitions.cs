using Hwdtech;
using Hwdtech.Ioc;
using Moq;
namespace spacebattle
{
    [Binding]
    public class DecisionTreeStepDefinitions
    {
        private Mock<IFileReader> reader = new Mock<IFileReader>();
        private Dictionary<float, object>.KeyCollection layer1;
        private IEnumerable<float> layer2;
        private IEnumerable<float> layer3;
        private IEnumerable<float> layer4;
        private HashSet<float> expected_layer_1 = new HashSet<float>();
        private HashSet<float> expected_layer_2 = new HashSet<float>();
        private HashSet<float> expected_layer_3 = new HashSet<float>();
        private HashSet<float> expected_layer_4 = new HashSet<float>();
        private DecisionTreeBuilderCommand cmd;
        public static void StartDecisionTreeTest()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

            var tree = new Dictionary<float, object>();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Collisions.Tree", (object[] args) =>
            {
                return tree;
            }).Execute();
        }

        [Given(@"дан файл с векторами по пути '([^']*)'")]
        public void GivenДанФайлСВекторамиПоПути(string p0)
        {
            StartDecisionTreeTest();
            reader = new Mock<IFileReader>();
            var path = p0;
            var arrays = File.ReadAllLines(path).Select(
                line => line.Split(" ").Select(num => float.Parse(num)).ToArray()
            ).ToList();

            expected_layer_1 = new HashSet<float>() { 1, 2 };
            expected_layer_2 = new HashSet<float>() { 1, 1, 2 };
            expected_layer_3 = new HashSet<float>() { 1, 3, 3 };
            expected_layer_4 = new HashSet<float>() { 1, 1, 1 };

            reader.Setup(r => r.ReadFile()).Returns(arrays);
        }

        [When(@"происходит считывание файла")]
        public void WhenПроисходитСчитываниеФайла()
        {
            try
            {
                cmd = new DecisionTreeBuilderCommand(reader.Object);
                cmd.Execute();
                var tree = IoC.Resolve<Dictionary<float, object>>("Game.Collisions.Tree");

                layer1 = tree.Keys;
                layer2 = ((Dictionary<float, object>)tree[1]).Keys.Union(((Dictionary<float, object>)tree[2]).Keys);
                layer3 = ((Dictionary<float, object>)((Dictionary<float, object>)tree[1])[1]).Keys.Union(((Dictionary<float, object>)((Dictionary<float, object>)tree[2])[1]).Keys.Union(((Dictionary<float, object>)((Dictionary<float, object>)tree[2])[2]).Keys));
                layer4 = ((Dictionary<float, object>)((Dictionary<float, object>)((Dictionary<float, object>)tree[1])[1])[1]).Keys.Union(((Dictionary<float, object>)((Dictionary<float, object>)((Dictionary<float, object>)tree[2])[1])[3]).Keys.Union(((Dictionary<float, object>)((Dictionary<float, object>)((Dictionary<float, object>)tree[2])[2])[3]).Keys));
            }
            catch (Exception)
            {

            }
        }

        [Then(@"считанное дерево равно ожидаемому")]
        public void ThenСчитанноеДеревоРавноОжидаемому()
        {
            Assert.True(expected_layer_1.SequenceEqual(layer1));
            Assert.True(expected_layer_2.SequenceEqual(layer2));
            Assert.True(expected_layer_3.SequenceEqual(layer3));
            Assert.True(expected_layer_4.SequenceEqual(layer4));
        }

        [Given(@"дан файл с векторами")]
        public void GivenДанФайлСВекторами()
        {
            StartDecisionTreeTest();
            reader.Setup(r => r.ReadFile()).Throws(new Exception());
        }

        [Then(@"получается ошибка считывания файла")]
        public void ThenПолучаетсяОшибкаСчитыванияФайла()
        {
            Assert.Throws<Exception>(cmd.Execute);
        }
    }
}
