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
    private static IValidatable? currentColor;
    private static readonly string appTitle = "Colour Converter";
    private static readonly string appConsoleTitle = "\r\n ██████╗ ██████╗ ██╗      ██████╗ ██╗   ██╗██████╗      ██████╗ ██████╗ ███╗   ██╗██╗   ██╗███████╗██████╗ ████████╗███████╗██████╗ \r\n██╔════╝██╔═══██╗██║     ██╔═══██╗██║   ██║██╔══██╗    ██╔════╝██╔═══██╗████╗  ██║██║   ██║██╔════╝██╔══██╗╚══██╔══╝██╔════╝██╔══██╗\r\n██║     ██║   ██║██║     ██║   ██║██║   ██║██████╔╝    ██║     ██║   ██║██╔██╗ ██║██║   ██║█████╗  ██████╔╝   ██║   █████╗  ██████╔╝\r\n██║     ██║   ██║██║     ██║   ██║██║   ██║██╔══██╗    ██║     ██║   ██║██║╚██╗██║╚██╗ ██╔╝██╔══╝  ██╔══██╗   ██║   ██╔══╝  ██╔══██╗\r\n╚██████╗╚██████╔╝███████╗╚██████╔╝╚██████╔╝██║  ██║    ╚██████╗╚██████╔╝██║ ╚████║ ╚████╔╝ ███████╗██║  ██║   ██║   ███████╗██║  ██║\r\n ╚═════╝ ╚═════╝ ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝     ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝  ╚═══╝  ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝╚═╝  ╚═╝\r\n                                                                                                                                    \r\n";
    private static readonly string appDescription = "This program converts colors between HEX, RGB, and CMYK formats.";

    internal static void Main()
    {
        try
        {
            ColorConsoleExtensions.StartProgram(appTitle, appConsoleTitle, appDescription);

            while (true)
            {
                var mainOptions = new Dictionary<int, string>
        {
            { 1, "Convert" },
            { 2, "Color tables" }
        };
                char inputChar = "Do you want to convert or show color tables: ".GetInputWithPrompt(mainOptions);
                ColorConsoleExtensions.PrintRandomColorLine();

                if (int.TryParse(inputChar.ToString(), out int inputNumber))
                {
                    switch (inputNumber)
                    {
                        case 1:
                            InitialConvert();
                            break;
                        case 2:
                            InitialColorTable();
                            break;
                        default:
                            Console.Error.WriteLine("Invalid input");
                            continue;
                    }
                    break;
                }
                else
                {
                    Console.Error.WriteLine("Invalid input");
                }
            }

            ConsoleExtensions.EndProgram();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    internal static void InitialColorTable()
    {
        while (true)
        {
            char inputChar = "Show colour table for: ".GetColorOptionInputWithPrompt();
            ColorConsoleExtensions.PrintRandomColorLine();

            if (int.TryParse(inputChar.ToString(), out int inputColor) && Enum.IsDefined(typeof(ColorOption), inputColor))
            {
                switch ((ColorOption)inputColor)
                {
                    case ColorOption.HEX:
                        Hex.PrintColorGrid();
                        break;
                    case ColorOption.RGB:
                        Rgb.PrintColorGrid();
                        break;
                    case ColorOption.CMYK:
                        Cmyk.PrintColorGrid();
                        break;
                    default:
                        Console.Error.WriteLine("Invalid input");
                        continue;
                }
                break; // Exit the loop if a valid option is chosen
            }
            else
            {
                Console.Error.WriteLine("Invalid input");
            }
        }
        AnotherColorTable();
    }


    internal static void InitialConvert()
    {
        Console.WriteLine(); // Move to the next line

        char inputChar = "What type do you want to convert: ".GetColorOptionInputWithPrompt();
        ColorConsoleExtensions.PrintRandomColorLine();

        if (int.TryParse(inputChar.ToString(), out int inputColor) && Enum.IsDefined(typeof(ColorOption), inputColor))
        {
            chosenFromColor = (ColorOption)inputColor;
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

        var options = conversionOptions.ToDictionary(kvp => (int)kvp.Key, kvp => kvp.Key.ToString());
        char inputChar = $"Convert {typeof(T).Name} to: ".GetInputWithPrompt(options);
        ColorConsoleExtensions.PrintRandomColorLine();

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
        char inputChar = "Do you want to convert the color to another format? ".GetInputWithYesNoPrompt();
        ColorConsoleExtensions.PrintRandomColorLine();
        if (char.ToLower(inputChar) == 'y')
        {
            Convert();
        }
        else
        {
            Console.WriteLine();
            inputChar = "Do you want to do another conversion? ".GetInputWithYesNoPrompt();
            ColorConsoleExtensions.PrintRandomColorLine();

            if (char.ToLower(inputChar) == 'y')
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

    internal static void AnotherColorTable()
    {
        char inputChar = "Do you want to show another color table? ".GetInputWithYesNoPrompt();
        ColorConsoleExtensions.PrintRandomColorLine();
        if (char.ToLower(inputChar) == 'y')
        {
            InitialColorTable();
        }
        else
        {
            ConsoleExtensions.EndProgram();
        }
    }
}