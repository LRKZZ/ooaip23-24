namespace spacebattle;

public class StartServer: ICommand 
{
    private int count;
    public StartServer(int count) 
    {
        this.count = count;
    }

    public void Execute() 
    {
        for (var i = 0; i < count; i++) 
        {
            
        }
    }
}