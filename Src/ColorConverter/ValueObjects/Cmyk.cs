using ColorConverter.Extensions;

namespace ColorConverter.ValueObjects;

internal class Cmyk : ColorBase
{
    public enum ColorComponent
    {
        Cyan,
        Magenta,
        Yellow,
        Key // Black
    }

    public double Cyan { get; set; } // Changed to public
    public double Magenta { get; set; } // Changed to public
    public double Yellow { get; set; } // Changed to public
    public double Key { get; set; } // Changed to public

    public Cmyk(double cyan, double magenta, double yellow, double key, bool displayConsole = true) // Constructor made public
    {
        Cyan = cyan;
        Magenta = magenta;
        Yellow = yellow;
        Key = key;
        DisplayConsole = displayConsole;
    }

    public override string ToString() => $"Cyan: {Cyan}, Magenta: {Magenta}, Yellow: {Yellow}, Key (Black): {Key}";

    public override bool IsValid()
    {
        try
        {
            return Cyan.IsValidComponent() && Magenta.IsValidComponent() && Yellow.IsValidComponent() && Key.IsValidComponent();
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException("Invalid CMYK value.", ex);
        }
    }

    internal static Cmyk EnterValues()
    {
        Console.WriteLine("Enter CMYK values (0-100)");
        double cyan = EnterComponentValue(ColorComponent.Cyan);
        double magenta = EnterComponentValue(ColorComponent.Magenta);
        double yellow = EnterComponentValue(ColorComponent.Yellow);
        double key = EnterComponentValue(ColorComponent.Key);

        Cmyk cmyk = new(cyan, magenta, yellow, key);
        // ConsoleHelper.PrintCmyk(cmyk); // This line needs to be adjusted based on ConsoleHelper's accessibility
        return cmyk;
    }

    private static double EnterComponentValue(ColorComponent componentName)
    {
        double value;
        while (true)
        {
            Console.Write($"Enter the amount for {componentName}: (0 - 100) ");
            if (double.TryParse(Console.ReadLine(), out value) && value >= 0 && value <= 100)
            {
                Console.WriteLine();
                break; // Exit the loop if input is valid
            }
            Console.Error.WriteLine("Value must be between 0 and 100.");
        }
        return value;
    }

    internal Rgb ToRgb()
    {
        Console.WriteLine($"Starting conversion from CMYK to RGB.");
        Console.WriteLine($"Original CMYK values: {this.ToString()}");

        // Ensure CMYK values are in the range of 0 to 1
        this.Cyan /= 100.0;
        this.Magenta /= 100.0;
        this.Yellow /= 100.0;
        this.Key /= 100.0;
        Console.WriteLine($"Normalized CMYK values: C={this.Cyan}, M={this.Magenta}, Y={this.Yellow}, K={this.Key}");

        // InitialConvert CMYK to CMY
        double cCmy = this.Cyan * (1 - this.Key) + this.Key;
        double mMgy = this.Magenta * (1 - this.Key) + this.Key;
        double yCmy = this.Yellow * (1 - this.Key) + this.Key;
        Console.WriteLine($"Converted to CMY: C={cCmy}, M={mMgy}, Y={yCmy}");

        // InitialConvert CMY to RGB
        int r = (int)((1 - cCmy) * 255);
        int g = (int)((1 - mMgy) * 255);
        int b = (int)((1 - yCmy) * 255);
        Console.WriteLine($"Converted to RGB before clamping: R={r}, G={g}, B={b}");

        // Clamp the values to the 0-255 range
        r = Math.Clamp(r, 0, 255);
        g = Math.Clamp(g, 0, 255);
        b = Math.Clamp(b, 0, 255);
        Console.WriteLine($"Final RGB values after clamping: R={r}, G={g}, B={b}");

        // Assuming Rgb class has a constructor that takes r, g, b as parameters
        return new Rgb(r, g, b);
    }

    internal Hex ToHex()
    {
        Console.WriteLine($"Starting conversion from CMYK to HEX.");

        Console.WriteLine($"InitialConvert to RGB");
        Rgb rgb = ToRgb();
        Console.WriteLine($"Converted to RGB: R={rgb.Red}, G={rgb.Green}, B={rgb.Blue}");

        Console.WriteLine($"InitialConvert to HEX");
        Hex hex = rgb.ToHex();

        if (!hex.IsValid())
        {
            throw new ArgumentException("Invalid HEX value.");
        }
        return hex;
    }

    public static void PrintColorGrid(int step = 10)
    {
        for (int c = 0; c <= 100; c += step)
        {
            for (int m = 0; m <= 100; m += step)
            {
                for (int y = 0; y <= 100; y += step)
                {
                    for (int k = 0; k <= 100; k += step)
                    {
                        TableCell(c, m, y, k);
                    }
                }
            }
        }
    }

    private static void TableCell(int c, int m, int y,int k)
    {
        Cmyk cmyk = new(c, m, y, k);
        var rgb = cmyk.ToRgb();
        ColorConsoleExtensions.SetBackgroundColor(rgb.Red, rgb.Green, rgb.Blue);
        Console.Write($" {c:D3},{m:D3},{y:D3},{k:D3} ");
        Console.ResetColor();
        Console.Write(" ");
    }
}
