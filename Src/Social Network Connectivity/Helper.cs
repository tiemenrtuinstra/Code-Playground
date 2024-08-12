namespace SocialNetworkConnectivity;

internal class Helper
{
    public static void PressKeyToExit()
    {
        Console.WriteLine();
        Console.WriteLine("Press any key to exit");
        Console.ReadKey();
    }

    public static void ConsoleDottedLine() => Console.WriteLine("..................................................");

}
