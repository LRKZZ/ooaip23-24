namespace spacebattle
{
    public class UObject
    {
        public IDict<string, object> args { get; set; }

        public UObject(IDict<string, object> args) 
        {
            this.args = args;
        }
    }
}
