using Common.Enums;
using System.Text;

namespace Common.Extensions;

public class ConsoleExtensions
{
    private static readonly BaseConsoleExtensions baseConsoleExtensions = new BaseConsoleExtensionsImpl();

    public static void PrintCenteredText(string text) => baseConsoleExtensions.PrintCenteredText(text);
    public static void PrintLine() => baseConsoleExtensions.PrintLine();
    public static string CustomLine(int length = 0, LineType lineType = LineType.Dashed, int lineThickness = 1) => baseConsoleExtensions.CustomLine(length, lineType, lineThickness);
    public static string SetCenteredText(string text) => baseConsoleExtensions.SetCenteredText(text);

    public static void StartProgram(string appTitle, string appConsoleTitle, string appDescription)
    {
        Console.Title = appTitle;
        PrintCenteredText(appConsoleTitle);
        PrintCenteredText(appDescription);
    }

    public static void EndProgram() => baseConsoleExtensions.EndProgram();
}