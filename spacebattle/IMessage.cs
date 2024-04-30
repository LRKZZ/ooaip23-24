namespace spacebattle;

public interface IMessage
{
    public string cmdType { get; }
    public string GameID { get; }
    public string GameItemID { get; }
    public IDictionary<string, object> Properties { get; }
}