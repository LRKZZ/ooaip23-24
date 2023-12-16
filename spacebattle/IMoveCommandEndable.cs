namespace spacebattle
{
    internal interface IMoveCommandEndable
    {
        string endCommand { get; }
        UObject Target { get; }
        IDict<string, object> args { get; }
    }
}
