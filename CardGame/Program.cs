namespace CardGame;

internal static class Program
{
    public static void Main()
    {
        CustomServer.CustomServer.StartWebServer();

        Console.WriteLine("Server started. Press any key to stop.");

        Console.ReadKey();

        CustomServer.CustomServer.Stop();
        
        Console.WriteLine("Server stopped. Press any key to exit.");
        
        Console.ReadKey();
    }
}