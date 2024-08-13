using Common.Enums;
using System.Text;

namespace Common.Extensions;

public abstract class ABaseConsoleExtensions : IConsoleExtensions
{
    public void PrintLine() => Console.WriteLine(CustomLine());

    public string CustomLine(int length = 0, LineType lineType = LineType.Dashed, int lineThickness = 1)
    {
        // If length is not provided, use the console width
        if (length <= 0)
        {
            length = Console.WindowWidth;
        }

        char lineChar = lineType.GetLineChar();
        string lineSegment = new string(lineChar, length);
        StringBuilder line = new StringBuilder((length + 2) * (lineThickness + 2)); // Pre-allocate capacity

        line.AppendLine();
        for (int i = 0; i < lineThickness; i++)
        {
            line.AppendLine(lineSegment);
        }
        line.AppendLine();

        return line.ToString();
    }

    public void PrintCenteredText(string text)
    {
        Console.WriteLine(SetCenteredText(text));
    }

    public string SetCenteredText(string text)
    {
        string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
        int consoleWidth = Console.WindowWidth;

        StringBuilder lineString = new StringBuilder();
        foreach (string line in lines)
        {
            int padding = (consoleWidth - line.Length) / 2;
            if (padding > 0)
            {
                lineString.Append(new string(' ', padding));
            }
            lineString.AppendLine(line);
        }
        return lineString.ToString();
    }

    public void EndProgram()
    {
        PrintLine();
        Console.Write("\nPress any key to exit... ");
        Console.ReadKey();
        Environment.Exit(0);
    }

    public void StartPogram(string appTitle, string appConsoleTitle, string appDescription)
    {
        throw new NotImplementedException();
    }
}

public class BaseConsoleExtensions : ABaseConsoleExtensions
{

}