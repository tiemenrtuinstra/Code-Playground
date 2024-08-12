using Common.Enums;
using System.Text;

namespace Common.Extensions;

public class ConsoleExtensions
{
    private static readonly BaseConsoleExtensions baseConsoleExtensions = new BaseConsoleExtensionsImpl();

    public static void StartProgram(string appTitle, string appConsoleTitle, string appDescription)
    {
        Console.Title = appTitle;
        baseConsoleExtensions.PrintCenteredText(appConsoleTitle);
        baseConsoleExtensions.PrintCenteredText(appDescription);
    }

    public static void EndProgram() => baseConsoleExtensions.EndProgram();
}