using ColorConverter.Extensions;

namespace ColorConverter.ValueObjects;

internal class Rgb : ColorBase
{
    public enum ColorComponent
    {
        Red,
        Green,
        Blue
    }

    internal int Red { get; set; }
    internal int Green { get; set; }
    internal int Blue { get; set; }

    internal Rgb(int red, int green, int blue, bool displayConsole = true)
    {
        Red = red;
        Green = green;
        Blue = blue;
        DisplayConsole = displayConsole;
    }

    public override string ToString() => $"Red: {Red}, Green: {Green}, Blue: {Blue}";

    public override bool IsValid()
    {
        try
        {
            return Red.IsValidComponent() && Green.IsValidComponent() && Blue.IsValidComponent();
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException("Invalid RGB value.", ex);
        }
    }

    internal static Rgb EnterValues()
    {
        Console.WriteLine("Enter RGB values (0-255) ");
        int red = EnterComponentValue(ColorComponent.Red);
        int green = EnterComponentValue(ColorComponent.Green);
        int blue = EnterComponentValue(ColorComponent.Blue);

        return new Rgb(red, green, blue);
    }

    private static int EnterComponentValue(ColorComponent componentName)
    {
        int value;
        while (true)
        {
            Console.Write($"Enter the amount for {componentName}: (0 - 255)");
            if (int.TryParse(Console.ReadLine(), out value) && value >= 0 && value <= 255)
            {
                Console.WriteLine();
                break;
            }
            Console.Error.WriteLine("Value must be between 0 and 255.");
        }
        return value;
    }

    internal Cmyk ToCmyk()
    {
        Console.WriteLine("Original RGB value");
        Console.WriteLine(this);

        Console.WriteLine("Converting RGB to CMYK...");

        // InitialConvert RGB to CMY
        Console.WriteLine("Step 1: InitialConvert RGB to CMY");
        double c = 1 - (Red / 255.0);
        double m = 1 - (Green / 255.0);
        double y = 1 - (Blue / 255.0);
        Console.WriteLine($"CMY values: C={c}, M={m}, Y={y}");

        // Find K
        Console.WriteLine("Step 2: Calculate the K value (the smallest value among C, M, Y)");
        double k = Math.Min(c, Math.Min(m, y));
        Console.WriteLine($"K value: {k}");

        if (k == 1.0) // All colors are 0, which means the color is black
        {
            Console.WriteLine("All RGB values are 0, so the color is black. Setting CMY values to 0.");
            c = 0;
            m = 0;
            y = 0;
        }
        else
        {
            // InitialConvert CMY to CMYK
            Console.WriteLine("Step 3: InitialConvert CMY to CMYK by adjusting C, M, Y based on K");
            c = (c - k) / (1 - k);
            m = (m - k) / (1 - k);
            y = (y - k) / (1 - k);
        }

        // InitialConvert to percentages
        Console.WriteLine("Step 4: InitialConvert the CMYK values to percentages");
        c *= 100;
        m *= 100;
        y *= 100;
        k *= 100;
        Console.WriteLine($"CMYK values: C={c}%, M={m}%, Y={y}%, K={k}%");

        return new Cmyk(c, m, y, k);
    }

    internal Hex ToHex()
    {
        // InitialConvert each RGB component to its hexadecimal representation
        string hexRed = DecimalToHexadecimal(Red);
        string hexGreen = DecimalToHexadecimal(Green);
        string hexBlue = DecimalToHexadecimal(Blue);

        // Concatenate the hexadecimal strings
        return new Hex(hexRed + hexGreen + hexBlue);
    }

    // Converts a decimal number (0-255) to a 2-digit hexadecimal string
    internal string DecimalToHexadecimal(int decimalValue)
    {
        // Ensure the value is within the RGB range
        if (decimalValue < 0 || decimalValue > 255)
        {
            throw new ArgumentOutOfRangeException(nameof(decimalValue), "Value must be between 0 and 255.");
        }

        // Calculate the hexadecimal digits
        int highDigit = decimalValue / 16;
        int lowDigit = decimalValue % 16;

        // InitialConvert digits to their hexadecimal representation
        string hexDigits = "0123456789ABCDEF";
        char highChar = hexDigits[highDigit];
        char lowChar = hexDigits[lowDigit];

        return $"{highChar}{lowChar}";
    }

    public static void PrintColorGrid(int step = 5)
    {

        for (int r = 0; r < 256; r += step)
        {
            for (int g = 0; g < 256; g += step)
            {
                for (int b = 0; b < 256; b += step)
                {
                    TableCell(r, g, b);
                }
            }
        }
    }

    private static void TableCell(int r, int g, int b)
    {
        ColorConsoleExtensions.SetBackgroundColor(r, g, b);
        Console.Write($" {r:D3},{g:D3},{b:D3} ");
        Console.ResetColor();
        Console.Write(" ");
    }
}
