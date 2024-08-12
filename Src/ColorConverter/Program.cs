using ColorConverter.Enums;
using ColorConverter.Extensions;
using ColorConverter.ValueObjects;
using Common.Extensions;
using System.Text;

namespace ColorConverter;

internal class Program
{
    private static ColorOption chosenFromColor;
    private static ColorOption chosenToColor;
    private static IValidatable currentColor;
    private static readonly string appTitle = "Colou Converter";
    private static readonly string appConsoleTitle = "\r\n ██████╗ ██████╗ ██╗      ██████╗ ██╗   ██╗██████╗      ██████╗ ██████╗ ███╗   ██╗██╗   ██╗███████╗██████╗ ████████╗███████╗██████╗ \r\n██╔════╝██╔═══██╗██║     ██╔═══██╗██║   ██║██╔══██╗    ██╔════╝██╔═══██╗████╗  ██║██║   ██║██╔════╝██╔══██╗╚══██╔══╝██╔════╝██╔══██╗\r\n██║     ██║   ██║██║     ██║   ██║██║   ██║██████╔╝    ██║     ██║   ██║██╔██╗ ██║██║   ██║█████╗  ██████╔╝   ██║   █████╗  ██████╔╝\r\n██║     ██║   ██║██║     ██║   ██║██║   ██║██╔══██╗    ██║     ██║   ██║██║╚██╗██║╚██╗ ██╔╝██╔══╝  ██╔══██╗   ██║   ██╔══╝  ██╔══██╗\r\n╚██████╗╚██████╔╝███████╗╚██████╔╝╚██████╔╝██║  ██║    ╚██████╗╚██████╔╝██║ ╚████║ ╚████╔╝ ███████╗██║  ██║   ██║   ███████╗██║  ██║\r\n ╚═════╝ ╚═════╝ ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝     ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝  ╚═══╝  ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝╚═╝  ╚═╝\r\n                                                                                                                                    \r\n";
    private static readonly string appDescription = "This program converts colors between HEX, RGB, and CMYK formats.";

    internal static void Main()
    {
        try
        {
            ColorConsoleExtensions.StartProgram(appTitle,appConsoleTitle, appDescription);

            InitialConvert();

            ConsoleExtensions.EndProgram();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }


    internal static void InitialConvert()
    {
        Console.WriteLine(); // Move to the next line

        Console.Write("What type do you want to convert: (1) HEX, (2) RGB, (3) CMYK ");
        char inputChar = Console.ReadKey().KeyChar;
        Console.WriteLine();
        ColorConsoleExtensions.PrintRandomColorLine();

        if (int.TryParse(inputChar.ToString(), out int inputNumber) && Enum.IsDefined(typeof(ColorOption), inputNumber))
        {
            chosenFromColor = (ColorOption)inputNumber;
            currentColor = null; // Reset current color
            Convert();
        }
        else
        {
            Console.Error.WriteLine("Invalid input");
            InitialConvert();
        }
    }

    private static void Convert()
    {
        var conversionMap = new Dictionary<ColorOption, Action>
        {
            {
                ColorOption.HEX, () => ConvertColor<Hex>(Hex.EnterValues, new Dictionary<ColorOption, Func<Hex, IValidatable>>
                {
                    { ColorOption.RGB, hex => hex.ToRgb() },
                    { ColorOption.CMYK, hex => hex.ToCmyk() }
                })
            },
            {
                ColorOption.RGB, () => ConvertColor<Rgb>(Rgb.EnterValues, new Dictionary<ColorOption, Func<Rgb, IValidatable>>
                {
                    { ColorOption.HEX, rgb => rgb.ToHex() },
                    { ColorOption.CMYK, rgb => rgb.ToCmyk() }
                })
            },
            {
                ColorOption.CMYK, () => ConvertColor<Cmyk>(Cmyk.EnterValues, new Dictionary<ColorOption, Func<Cmyk, IValidatable>>
                {
                    { ColorOption.HEX, cmyk => cmyk.ToHex() },
                    { ColorOption.RGB, cmyk => cmyk.ToRgb() }
                })
            }
        };

        if (conversionMap.TryGetValue(chosenFromColor, out var convertAction))
        {
            convertAction();
        }
        else
        {
            Console.Error.WriteLine("Invalid input");
            InitialConvert();
        }
    }

    internal static void ConvertColor<T>(Func<T> enterValuesFunc, Dictionary<ColorOption, Func<T, IValidatable>> conversionOptions) where T : IValidatable
    {
        if (currentColor == null)
        {
            currentColor = enterValuesFunc();
            if (!currentColor.IsValid())
            {
                Console.Error.WriteLine($"Invalid {typeof(T).Name} values.");
                return;
            }
        }

        Console.Write($"Convert {typeof(T).Name} to: {string.Join(", ", conversionOptions.Keys.Select(k => $"({(int)k}) {k}"))}");
        ColorConsoleExtensions.PrintRandomColorLine();

        char inputChar = Console.ReadKey().KeyChar;
        Console.WriteLine();
        if (int.TryParse(inputChar.ToString(), out int inputNumber) && conversionOptions.ContainsKey((ColorOption)inputNumber))
        {
            chosenToColor = (ColorOption)inputNumber;
            TryPrintConversion(() => conversionOptions[chosenToColor]((T)currentColor), chosenToColor);
        }
        else
        {
            Console.Error.WriteLine("Invalid input");
            ConvertColor(enterValuesFunc, conversionOptions); // Retry conversion
        }

        AnotherConversion();
    }

    // Generic method to handle conversion and printing
    internal static void TryPrintConversion<T>(Func<T> conversionFunc, ColorOption conversionType) where T : IValidatable
    {
        T result = conversionFunc();
        if (result.IsValid())
        {
            Console.WriteLine($"This is the {conversionType} color:");
            ColorConsoleExtensions.PrintColor(result);
        }
        else
        {
            Console.Error.WriteLine($"Failed to convert to {conversionType}.");
        }
    }

    internal static void AnotherConversion()
    {
        Console.Write("Do you want to convert the color to another format? (Y/N) ");
        if (char.ToLower(Console.ReadKey().KeyChar) == 'y')
        {
            Console.WriteLine();
            Convert();
        }
        else
        {
            Console.WriteLine();
            Console.Write("Do you want to do another conversion? (Y/N) ");
            if (char.ToLower(Console.ReadKey().KeyChar) == 'y')
            {
                Console.WriteLine();
                InitialConvert();
            }
            else
            {
                ConsoleExtensions.EndProgram();
            }
        }
    }
}
