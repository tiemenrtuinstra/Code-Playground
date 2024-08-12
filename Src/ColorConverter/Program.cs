using ColorConverter.Enums;
using ColorConverter.Extensions;
using ColorConverter.ValueObjects;

namespace ColorConverter;

internal struct ConversionOperation
{
    public Func<IValidatable> ConversionFunc;
    public Action<IValidatable> PrintFunc;
    public ColorOption ColorOption;

    public ConversionOperation(Func<IValidatable> conversionFunc,
        Action<IValidatable> printFunc, ColorOption colorOption)
    {
        ConversionFunc = conversionFunc;
        PrintFunc = printFunc;
        ColorOption = colorOption;
    }
}

internal class Program
{
    internal static void Main()
    {
        try
        {
            Console.Title = "Colour Converter";
            string text = "\r\n ██████╗ ██████╗ ██╗      ██████╗ ██╗   ██╗██████╗      ██████╗ ██████╗ ███╗   ██╗██╗   ██╗███████╗██████╗ ████████╗███████╗██████╗ \r\n██╔════╝██╔═══██╗██║     ██╔═══██╗██║   ██║██╔══██╗    ██╔════╝██╔═══██╗████╗  ██║██║   ██║██╔════╝██╔══██╗╚══██╔══╝██╔════╝██╔══██╗\r\n██║     ██║   ██║██║     ██║   ██║██║   ██║██████╔╝    ██║     ██║   ██║██╔██╗ ██║██║   ██║█████╗  ██████╔╝   ██║   █████╗  ██████╔╝\r\n██║     ██║   ██║██║     ██║   ██║██║   ██║██╔══██╗    ██║     ██║   ██║██║╚██╗██║╚██╗ ██╔╝██╔══╝  ██╔══██╗   ██║   ██╔══╝  ██╔══██╗\r\n╚██████╗╚██████╔╝███████╗╚██████╔╝╚██████╔╝██║  ██║    ╚██████╗╚██████╔╝██║ ╚████║ ╚████╔╝ ███████╗██║  ██║   ██║   ███████╗██║  ██║\r\n ╚═════╝ ╚═════╝ ╚══════╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═╝     ╚═════╝ ╚═════╝ ╚═╝  ╚═══╝  ╚═══╝  ╚══════╝╚═╝  ╚═╝   ╚═╝   ╚══════╝╚═╝  ╚═╝\r\n                                                                                                                                    \r\n";

            ConsoleExtensions.PrintColoredText(text);

            Convert();

            ConsoleExtensions.EndProgram();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    internal static void Convert()
    {
        Console.WriteLine(); // Move to the next line

        Console.WriteLine("What type do you want to convert: (1) HEX, (2) RGB, (3) YMCK");
        ConsoleExtensions.PrintLine();

        char inputChar = Console.ReadKey().KeyChar;
        if (int.TryParse(inputChar.ToString(), out int inputNumber) && Enum.IsDefined(typeof(ColorOption), inputNumber))
        {
            ColorOption selectedOption = (ColorOption)inputNumber;
            switch (selectedOption)
            {
                case ColorOption.HEX:
                    ConvertHex();
                    break;
                case ColorOption.RGB:
                    ConvertRgb();
                    break;
                case ColorOption.CMYK:
                    ConvertCmyk();
                    break;
                default:
                    Console.Error.WriteLine("Invalid input");
                    Convert();
                    break;
            }
        }
        else
        {
            Console.Error.WriteLine("Invalid input");
            Convert();
        }
    }

    internal static void ConvertCmyk()
    {
        Cmyk cmyk = Cmyk.EnterValues();
        if (!cmyk.IsValid())
        {
            Console.Error.WriteLine("Invalid CMYK values.");
            return;
        }

        Console.WriteLine("Convert CMYK to: (1) HEX, (2) RGB");
        ConsoleExtensions.PrintLine();

        char inputChar = Console.ReadKey().KeyChar;
        if (int.TryParse(inputChar.ToString(), out int inputNumber))
        {
            if (Enum.IsDefined(typeof(ColorOption), inputNumber))
            {
                ColorOption selectedOption = (ColorOption)inputNumber;
                switch (selectedOption)
                {
                    case ColorOption.HEX:
                        TryPrintConversion(cmyk.ToHex, ConsoleExtensions.PrintHex, ColorOption.HEX);
                        break;
                    case ColorOption.RGB:
                        TryPrintConversion(cmyk.ToRgb, ConsoleExtensions.PrintCmyk, ColorOption.RGB);
                        break;
                    default:
                        Console.Error.WriteLine("Invalid option");
                        ConvertRgb(); // Retry conversion
                        break;
                }
            }
            else
            {
                Console.Error.WriteLine("Invalid enum value");
                ConvertRgb(); // Retry conversion
            }
        }
        else
        {
            Console.Error.WriteLine("Invalid input");
            ConvertRgb(); // Retry conversion
        }
    }

    internal static void ConvertRgb()
    {
        Rgb rgb = Rgb.EnterValues();
        if (!rgb.IsValid())
        {
            Console.Error.WriteLine("Invalid RGB values.");
            return;
        }

        Console.WriteLine("Convert RGB to: (1) HEX, (2) CMYK");
        ConsoleExtensions.PrintLine();

        char inputChar = Console.ReadKey().KeyChar;
        if (int.TryParse(inputChar.ToString(), out int inputNumber))
        {
            inputNumber = inputNumber == 2 ? 3 : inputNumber; //remap the value to match the enum

            if (Enum.IsDefined(typeof(ColorOption), inputNumber))
            {
                ColorOption selectedOption = (ColorOption)inputNumber;
                switch (selectedOption)
                {
                    case ColorOption.HEX:
                        TryPrintConversion(rgb.ToHex, ConsoleExtensions.PrintHex, ColorOption.RGB);
                        break;
                    case ColorOption.CMYK:
                        TryPrintConversion(rgb.ToCmyk, ConsoleExtensions.PrintCmyk, ColorOption.CMYK);
                        break;
                    default:
                        Console.Error.WriteLine("Invalid option");
                        ConvertRgb(); // Retry conversion
                        break;
                }
            }
            else
            {
                Console.Error.WriteLine("Invalid enum value");
                ConvertRgb(); // Retry conversion
            }
        }
        else
        {
            Console.Error.WriteLine("Invalid input");
            ConvertRgb(); // Retry conversion
        }
    }

    internal static void ConvertHex()
    {
        Hex hex = Hex.EnterValue();
        if (!hex.IsValid())
        {
            Console.Error.WriteLine("Invalid HEX value.");
            return;
        }

        Console.WriteLine("Convert HEX to: (1) RGB, (2) CMYK");
        ConsoleExtensions.PrintLine();

        char inputChar = Console.ReadKey().KeyChar;
        if (int.TryParse(inputChar.ToString(), out int inputNumber))
        {
            inputNumber = inputNumber == 2 ? 3 : inputNumber; //remap the value to match the enum
            inputNumber = inputNumber == 1 ? 2 : inputNumber; //remap the value to match the enum

            if (Enum.IsDefined(typeof(ColorOption), inputNumber))
            {
                ColorOption selectedOption = (ColorOption)inputNumber;
                switch (selectedOption)
                {
                    case ColorOption.HEX:
                        Console.WriteLine("HEX value: " + hex.ToString());
                        break;
                    case ColorOption.RGB:
                        TryPrintConversion(hex.ToRgb, ConsoleExtensions.PrintRgb, ColorOption.RGB);
                        break;
                    case ColorOption.CMYK:
                        TryPrintConversion(hex.ToCmyk, ConsoleExtensions.PrintCmyk, ColorOption.CMYK);
                        break;
                    default:
                        Console.Error.WriteLine("Invalid option");
                        ConvertHex(); // Retry conversion
                        break;
                }
            }
            else
            {
                Console.Error.WriteLine("Invalid enum value");
                ConvertHex(); // Retry conversion
            }
        }
        else
        {
            Console.Error.WriteLine("Invalid input");
            ConvertHex(); // Retry conversion
        }

        AnotherConversion();
    }

    // Generic method to handle conversion and printing
    internal static void TryPrintConversion<T>(Func<T>
        conversionFunc, Action<T> printFunc, ColorOption conversionType) where T : IValidatable
    {
        T result = conversionFunc();
        if (result.IsValid())
        {
            Console.WriteLine($"This is the {conversionType} colour:");
            printFunc(result);
        }
        else
        {
            Console.Error.WriteLine($"Failed to convert to {conversionType}.");
        }
    }

    internal static void AnotherConversion()
    {
        Console.WriteLine("Do you want to do another conversion? (Y/N)");
        if (char.ToLower(Console.ReadKey().KeyChar) == 'y')
        {
            Convert();
        }
        else
        {
            ConsoleExtensions.EndProgram();
        }
    }

}
