namespace spacebattle;

public interface IMessage
{
    public string cmdType { get; }
    public int GameID { get; }
    public int GameItemID { get; }
    public IDictionary<string, object> Properties { get; }
}