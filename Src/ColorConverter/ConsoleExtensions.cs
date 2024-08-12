using ColorConverter.ValueObjects;

namespace ColorConverter.Extensions;

public static class ConsoleExtensions
{
    internal static void PrintLine()
    {
        "\n--------------------------------\n".PrintColoredText();
    }

    internal static void PrintColoredText(this string text)
    {
        var allColors = Enum.GetValues(typeof(ConsoleColor));
        var excludedColors = new ConsoleColor[] { ConsoleColor.Black, ConsoleColor.White, ConsoleColor.DarkGray, ConsoleColor.Gray };
        var availableColors = allColors.Cast<ConsoleColor>().Where(color => !excludedColors.Contains(color)).ToArray();

        Random random = new Random();

        foreach (char c in text)
        {
            int randomIndex = random.Next(availableColors.Length);
            Console.ForegroundColor = availableColors[randomIndex];
            Console.Write(c);
        }

        Console.ResetColor(); // Reset to default color after printing
    }

    internal static void HexForegroundColor(Hex hex)
    {
        Rgb rgb = hex.ToRgb();
        Console.Write($"\x1b[38;2;{rgb.Red};{rgb.Green};{rgb.Blue}m");
    }

    internal static void RgbForegroundColor(Rgb rgb) =>
            Console.Write($"\x1b[38;2;{rgb.Red};{rgb.Green};{rgb.Blue}m");

    internal static void CmYkForegroundColor(Cmyk cmyk) =>
        Console.Write($"\x1b[38;2;{cmyk.Cyan};{cmyk.Magenta};{cmyk.Yellow};{cmyk.Key}m");

    internal static void RgbBackgroundColor(Rgb rgb) =>
        Console.Write($"\x1b[48;2;{rgb.Red};{rgb.Green};{rgb.Blue}m");

    internal static void CmYkBackgroundColor(Cmyk cmyk) =>
        Console.Write($"\x1b[48;2;{cmyk.Cyan};{cmyk.Magenta};{cmyk.Yellow};{cmyk.Key}m");

    internal static void ResetColor() => Console.Write("\x1b[0m");

    internal static void PrintHex(Hex hex)
    {
        HexForegroundColor(hex);
        Console.WriteLine($"Hex: {hex.Value}");
        ResetColor();
    }

    internal static void PrintRgb(Rgb rgb)
    {
        RgbForegroundColor(rgb);
        Console.WriteLine($"Red: {rgb.Red}\n" +
                        $"Green: {rgb.Green}\n" +
                        $"Blue: {rgb.Blue}");
        ResetColor();
    }

    internal static void PrintCmyk(Cmyk cmyk)
    {
        CmYkForegroundColor(cmyk);
        Console.WriteLine($"Cyan: {cmyk.Cyan}\n" +
                                   $"Magenta: {cmyk.Magenta}\n" +
                                                          $"Yellow: {cmyk.Yellow}\n" +
                                                                                 $"Key: {cmyk.Key}");
        ResetColor();
    }

    internal static void EndProgram()
    {
        PrintLine();
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();

        //exit the program
        Environment.Exit(0);
    }

}
