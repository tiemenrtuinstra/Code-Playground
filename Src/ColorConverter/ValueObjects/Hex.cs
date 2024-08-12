using ColorConverter.Extensions;

namespace ColorConverter.ValueObjects;

internal class Hex : IValidatable
{
    internal string Value { get; set; }

    internal Hex(string value)
    {
        Value = value;
    }

    public override string ToString()
    {
        return Value;
    }

    public bool IsValid()
    {
        try
        {
            return Value.Length == 6 && Value.All(c => char.IsDigit(c) || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'));
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException("Invalid HEX value.", ex);
        }
    }

    internal static Hex EnterValue()
    {
        Console.WriteLine("Enter HEX value: ");
        string input = Console.ReadLine();
        var hex = new Hex(input);
        if (!hex.IsValid())
        {
            Console.Error.WriteLine("Invalid HEX value.");
            return EnterValue();
        }
        return hex;
    }

    internal Rgb ToRgb()
    {
        Console.WriteLine("Starting HEX to RGB conversion...");
        int red = HexPairToDecimal(Value.Substring(0, 2));
        int green = HexPairToDecimal(Value.Substring(2, 2));
        int blue = HexPairToDecimal(Value.Substring(4, 2));

        return new Rgb(red, green, blue);
    }

    internal Cmyk ToCmyk()
    {
        Console.WriteLine("Starting HEX to CMYK conversion...");
        ConsoleExtensions.PrintLine();

        // Convert HEX to RGB

        Console.WriteLine($"HEX value: {Value}");
        ConsoleExtensions.PrintLine();
        int red = HexPairToDecimal(Value.Substring(0, 2));
        int green = HexPairToDecimal(Value.Substring(2, 2));
        int blue = HexPairToDecimal(Value.Substring(4, 2));

        Console.WriteLine($"RGB values - Red: {red}, Green: {green}, Blue: {blue}");
        ConsoleExtensions.PrintLine();
        // Convert RGB to CMYK
        double r = red / 255.0;
        double g = green / 255.0;
        double b = blue / 255.0;
        Console.WriteLine($"Normalized RGB values - R: {r}, G: {g}, B: {b}");
        ConsoleExtensions.PrintLine();
        double k = 1 - Math.Max(Math.Max(r, g), b);
        Console.WriteLine($"K (Black) value: {k}");
        ConsoleExtensions.PrintLine();
        double c = (k == 1) ? 0 : (1 - r - k) / (1 - k);
        double m = (k == 1) ? 0 : (1 - g - k) / (1 - k);
        double y = (k == 1) ? 0 : (1 - b - k) / (1 - k);
        Console.WriteLine($"CMYK values before normalization - C: {c}, M: {m}, Y: {y}, K: {k}");
        ConsoleExtensions.PrintLine();
        // Convert 0-1 range to 0-100 range for percentages
        c *= 100;
        m *= 100;
        y *= 100;
        k *= 100;

        Console.WriteLine($"CMYK values as percentages - C: {c}%, M: {m}%, Y: {y}%, K: {k}%");
        ConsoleExtensions.PrintLine();
        return new Cmyk(c, m, y, k);
    }

    private int HexPairToDecimal(string hexPair)
    {
        Console.WriteLine($"Converting hex pair '{hexPair}' to decimal...");

        int value = 0;
        for (int i = 0; i < hexPair.Length; i++)
        {
            int digitValue = HexDigitToDecimal(hexPair[i]);
            int power = hexPair.Length - i - 1;
            Console.WriteLine($"Hex digit '{hexPair[i]}' to decimal: {digitValue}, multiplied by 16^{power}");
            value += digitValue * (int)Math.Pow(16, power);
        }
        Console.WriteLine($"Decimal value of hex pair '{hexPair}': {value}");
        ConsoleExtensions.PrintLine();
        return value;
    }

    private int HexDigitToDecimal(char hexDigit)
    {
        Console.WriteLine($"Converting hex digit '{hexDigit}' to decimal...");
        if (hexDigit >= '0' && hexDigit <= '9')
        {
            int value = hexDigit - '0';
            Console.WriteLine($"Decimal value: {value}");
            return value;
        }
        else if (hexDigit >= 'A' && hexDigit <= 'F')
        {
            int value = 10 + (hexDigit - 'A');
            Console.WriteLine($"Decimal value: {value}");
            return value;
        }
        else if (hexDigit >= 'a' && hexDigit <= 'f')
        {
            int value = 10 + (hexDigit - 'a');
            Console.WriteLine($"Decimal value: {value}");
            return value;
        }
        throw new ArgumentException($"Invalid hex digit '{hexDigit}'");
    }
}
