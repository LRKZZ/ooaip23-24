namespace Program
{
    using Hwdtech;
    using spacebattle;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
                {
                    Console.WriteLine("Please provide the number of threads as a command-line argument.");
                    return;
                }

                int numThreads;
                if (!int.TryParse(args[0], out numThreads))
                {
                    Console.WriteLine("Invalid argument. Please provide a valid number of threads.");
                    return;
                }

                Console.WriteLine("Starting server");
                IoC.Resolve<spacebattle.ICommand>("StartCommand", numThreads).Execute();
                Console.WriteLine("Server started");

                Console.ReadKey();

                Console.WriteLine("Stop server");
                IoC.Resolve<spacebattle.ICommand>("StopServerCommand").Execute();
                Console.WriteLine("Server stopped");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}