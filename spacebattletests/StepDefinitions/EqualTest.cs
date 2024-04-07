namespace spacebattle;

using System.Collections.Concurrent;

public class EqualTest
{
    public EqualTest()
    {

    }

    [Fact]
    public void EqualNull()
    {
        var q_1 = new BlockingCollection<ICommand>(100);
        var st_1 = new ServerThread(q_1);
        var bl_1 = st_1.Equals(null);

        Assert.False(bl_1);
    }

    [Fact]
    public void EqualServerThread()
    {
        var q_1 = new BlockingCollection<ICommand>(100);
        var q_2 = new BlockingCollection<ICommand>(100);
        var st_1 = new ServerThread(q_1);
        var st_2 = new ServerThread(q_2);

        var bl_3 = st_1.Equals(st_2);

        Assert.False(bl_3);
    }

    [Fact]
    public void EqualDifferentType()
    {
        var q_1 = new BlockingCollection<ICommand>(100);
        var st_1 = new ServerThread(q_1);

        var bl_2 = st_1.Equals(15);

        Assert.False(bl_2);
    }
}
