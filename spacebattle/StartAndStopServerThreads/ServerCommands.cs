namespace spacebattle 
{
    public class StopServer: ICommand
    {
        public void Execute()
        {
              
        }
    }
    
    public class StartServer: ICommand
    {
        private readonly int count;
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
}