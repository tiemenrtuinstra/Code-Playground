using Common.Enums;
using System.Text;

namespace Common.Extensions;

public static class ConsoleExtensions
{
    private static readonly ABaseConsoleExtensions baseConsoleExtensions = new BaseConsoleExtensions();

    public static void PrintCenteredText(string text) => baseConsoleExtensions.PrintCenteredText(text);
    public static void PrintLine() => baseConsoleExtensions.PrintLine();
    public static string CustomLine(int length = 0, LineType lineType = LineType.Dashed, int lineThickness = 1) => baseConsoleExtensions.CustomLine(length, lineType, lineThickness);
    public static string SetCenteredText(string text) => baseConsoleExtensions.SetCenteredText(text);

    public static void StartProgram(string appTitle, string appConsoleTitle, string appDescription)
    {
        Console.Title = appTitle;
        PrintCenteredText(appConsoleTitle);
        PrintCenteredText(appDescription);
        Console.WriteLine();
        CustomLine(0, LineType.Solid, 1);
        Console.WriteLine();
    }

    public static void EndProgram() => baseConsoleExtensions.EndProgram();

    public static char GetInputWithPrompt(this string promptMessage, Dictionary<int, string> options)
    {
        var sb = new StringBuilder(promptMessage.Trim());
        if (!sb.ToString().EndsWith(" "))
        {
            sb.Append(" ");
        }

        foreach (var option in options)
        {
            sb.Append($"({option.Key}) {option.Value} ");
        }

        sb.Append(" ");

        Console.Write(sb.ToString().Replace("  "," "));

        char inputChar = Console.ReadKey().KeyChar;
        Console.WriteLine();

        return inputChar;
    }

    public static char GetInputWithYesNoPrompt(this string promptMessage)
    {
        var sb = new StringBuilder(promptMessage.Trim());
        if (!sb.ToString().EndsWith(" "))
        {
            sb.Append(" ");
        }
        sb.Append("(Y) Yes (N) No ");

        sb.Append(" ");

        Console.Write(sb.ToString().Replace("  ", " "));

        char inputChar = Console.ReadKey().KeyChar;
        Console.WriteLine();

        return inputChar;

    }
}