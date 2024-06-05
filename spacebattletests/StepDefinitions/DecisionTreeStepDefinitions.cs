using Hwdtech;
using Hwdtech.Ioc;
using Moq;
namespace spacebattle
{
    [Binding]
    public class DecisionTreeStepDefinitions
    {
        private Mock<IFileReader> _reader = new();
        private HashSet<float> _expected_layer_1 = new();
        private HashSet<float> _expected_layer_2 = new();
        private HashSet<float> _expected_layer_3 = new();
        private HashSet<float> _expected_layer_4 = new();
        private Dictionary<float, object> tree = new();
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
            _reader = new Mock<IFileReader>();
            var path = p0;
            var arrays = File.ReadAllLines(path).Select(
                line => line.Split(" ").Select(num => float.Parse(num)).ToArray()
            ).ToList();

            _expected_layer_1 = new HashSet<float>() { 1, 2 };
            _expected_layer_2 = new HashSet<float>() { 1, 1, 2 };
            _expected_layer_3 = new HashSet<float>() { 1, 3, 3 };
            _expected_layer_4 = new HashSet<float>() { 1, 1, 1 };

            _reader.Setup(r => r.ReadFile()).Returns(arrays);
        }

        [When(@"происходит считывание файла")]
        public void WhenПроисходитСчитываниеФайла()
        {
            try
            {
                var cmd = new DecisionTreeBuilderCommand(_reader.Object);
                cmd.Execute();
                tree = IoC.Resolve<Dictionary<float, object>>("Game.Collisions.Tree");
            }
            catch (Exception)
            {

            }
        }

        [Then(@"считанное дерево равно ожидаемому")]
        public void ThenСчитанноеДеревоРавноОжидаемому()
        {
            var layer1 = tree.Keys;
            var layer2 = ((Dictionary<float, object>)tree[1]).Keys.Union(((Dictionary<float, object>)tree[2]).Keys);
            var layer3 = ((Dictionary<float, object>)((Dictionary<float, object>)tree[1])[1]).Keys.Union(((Dictionary<float, object>)((Dictionary<float, object>)tree[2])[1]).Keys.Union(((Dictionary<float, object>)((Dictionary<float, object>)tree[2])[2]).Keys));
            var layer4 = ((Dictionary<float, object>)((Dictionary<float, object>)((Dictionary<float, object>)tree[1])[1])[1]).Keys.Union(((Dictionary<float, object>)((Dictionary<float, object>)((Dictionary<float, object>)tree[2])[1])[3]).Keys.Union(((Dictionary<float, object>)((Dictionary<float, object>)((Dictionary<float, object>)tree[2])[2])[3]).Keys));

            Assert.True(_expected_layer_1.SequenceEqual(layer1));
            Assert.True(_expected_layer_2.SequenceEqual(layer2));
            Assert.True(_expected_layer_3.SequenceEqual(layer3));
            Assert.True(_expected_layer_4.SequenceEqual(layer4));
        }

        [Given(@"дан файл с векторами")]
        public void GivenДанФайлСВекторами()
        {
            StartDecisionTreeTest();
            _reader.Setup(r => r.ReadFile()).Throws(new Exception());
        }

        [Then(@"получается ошибка считывания файла")]
        public void ThenПолучаетсяОшибкаСчитыванияФайла()
        {
            var cmd = new DecisionTreeBuilderCommand(_reader.Object);
            Assert.Throws<Exception>(cmd.Execute);
        }
    }
}
