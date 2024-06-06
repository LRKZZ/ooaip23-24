namespace spacebattle;

public record Message
{
    public string cmd { get; init; }
    public int gameId { get; init; }

    [System.Text.Json.Serialization.JsonExtensionDataAttribute]
    public IDictionary<string, object>? _additionalData { get; set; }

    public Message(string cmd, int gameId)
    {
        this.cmd = cmd;
        this.gameId = gameId;
    }
}
