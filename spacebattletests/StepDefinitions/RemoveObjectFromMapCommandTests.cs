using Hwdtech;
using Hwdtech.Ioc;
using Moq;
using spacebattle;

namespace spacebattletests
{
    public class RemoveObjectFromMapCommandTests
    {
        private readonly Dictionary<int, IUObject> _objectDictionary = new(){
            {1, new Mock<IUObject>().Object}
        };

        public RemoveObjectFromMapCommandTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "RetrieveObjects", (object[] args) => _objectDictionary).Execute();
        }

        [Fact]
        public void Execute_RemovesObjectFromDictionary()
        {
            var objectId = 1;
            Assert.Single(_objectDictionary);
            var command = new RemoveObjectFromMapCommand(objectId);

            command.Execute();

            Assert.Empty(_objectDictionary);
        }

        [Fact]
        public void UnsuccessfulRetrieveObjectFromMapStrategyRunStrategyBecauseCannotFindKeyObjectMap()
        {
            var nonExistentId = 2;

            Assert.Single(_objectDictionary);
            var command = new RemoveObjectFromMapCommand(nonExistentId);

            Assert.Throws<Exception>(() => command.Execute());
        }
    }
}
