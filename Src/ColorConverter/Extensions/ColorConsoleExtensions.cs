using ColorConverter.Enums;
using ColorConverter.ValueObjects;
using Common.Enums;
using Common.Extensions;
using static System.Net.Mime.MediaTypeNames;

namespace ColorConverter.Extensions
{
    public static class ColorConsoleExtensions
    {
        private static readonly ABaseConsoleExtensions baseConsoleExtensions = new BaseConsoleExtensions();

        public static void PrintColorLine(int red, int green, int blue)
        {
            SetForegroundColor(red, green, blue);
            Console.WriteLine(baseConsoleExtensions.CustomLine());
            Console.ResetColor();
        }

        public static void PrintRandomColorLine() =>
            PrintRandomColor(baseConsoleExtensions.CustomLine());

        public static void PrintCustomRandomColorLine(int length = 32, LineType lineType = LineType.Dashed, int lineThickness = 1) =>
            PrintRandomColor(baseConsoleExtensions.CustomLine(length, lineType, lineThickness));

        public static void PrintRandomColor(string text, bool colorForeground = true, bool colorBackground = false)
        {
            var allColors = Enum.GetValues(typeof(ConsoleColor));
            var excludedColors = new ConsoleColor[] { ConsoleColor.Black, ConsoleColor.White, ConsoleColor.DarkGray, ConsoleColor.Gray };
            var availableColors = allColors.Cast<ConsoleColor>().Where(color => !excludedColors.Contains(color)).ToArray();

            Random random = new Random();
            ConsoleColor lastForegroundColor = availableColors[random.Next(availableColors.Length)];
            ConsoleColor lastBackgroundColor = availableColors[random.Next(availableColors.Length)];

            foreach (char c in text)
            {
                ConsoleColor newForegroundColor = lastForegroundColor;
                ConsoleColor newBackgroundColor = lastBackgroundColor;

                if (colorForeground)
                {
                    do
                    {
                        newForegroundColor = availableColors[random.Next(availableColors.Length)];
                    } while (newForegroundColor == lastForegroundColor);
                    Console.ForegroundColor = newForegroundColor;
                    lastForegroundColor = newForegroundColor;
                }

                if (colorBackground)
                {
                    do
                    {
                        newBackgroundColor = availableColors[random.Next(availableColors.Length)];
                    } while (newBackgroundColor == lastBackgroundColor || newBackgroundColor == newForegroundColor);
                    Console.BackgroundColor = newBackgroundColor;
                    lastBackgroundColor = newBackgroundColor;
                }

                Console.Write(c); // Print each character individually with a different color
            }

            Console.ResetColor(); // Reset to default color after printing
        }

        public static void SetForegroundColor(int red, int green, int blue) =>
            Console.Write($"\x1b[38;2;{red};{green};{blue}m");

        public static void SetBackgroundColor(int red, int green, int blue) =>
            Console.Write($"\x1b[48;2;{red};{green};{blue}m");

        internal static void HexForegroundColor(Hex hex)
        {
            Rgb rgb = hex.ToRgb();
            SetForegroundColor(rgb.Red, rgb.Green, rgb.Blue);
        }

        internal static void RgbForegroundColor(Rgb rgb) =>
            SetForegroundColor(rgb.Red, rgb.Green, rgb.Blue);

        internal static void CmYkForegroundColor(Cmyk cmyk)
        {
            Rgb rgb = cmyk.ToRgb();
            SetForegroundColor(rgb.Red, rgb.Green, rgb.Blue);
        }

        internal static void RgbBackgroundColor(Rgb rgb) =>
            SetBackgroundColor(rgb.Red, rgb.Green, rgb.Blue);

        internal static void CmYkBackgroundColor(Cmyk cmyk)
        {
            Rgb rgb = cmyk.ToRgb();
            SetBackgroundColor(rgb.Red, rgb.Green, rgb.Blue);
        }

        public static void ResetColor() => Console.Write("\x1b[0m");

        internal static void PrintHex(Hex hex)
        {
            HexForegroundColor(hex);
            Console.WriteLine($"Hex: {hex.Value}");
            ResetColor();
        }

        internal static void PrintRgb(Rgb rgb)
        {
            RgbForegroundColor(rgb);
            Console.WriteLine(
                $"Red: {rgb.Red}\n" +
                $"Green: {rgb.Green}\n" +
                $"Blue: {rgb.Blue}"
            );
            ResetColor();
        }

        internal static void PrintCmyk(Cmyk cmyk)
        {
            CmYkForegroundColor(cmyk);
            Console.WriteLine(
                $"Cyan: {cmyk.Cyan}\n" +
                $"Magenta: {cmyk.Magenta}\n" +
                $"Yellow: {cmyk.Yellow}\n" +
                $"Key: {cmyk.Key}"
            );
            ResetColor();
        }

        public static void PrintColor(IValidatable color)
        {
            var printActions = new Dictionary<Type, Action<IValidatable>>
            {
                { typeof(Hex), c => PrintHex((Hex)c) },
                { typeof(Rgb), c => PrintRgb((Rgb)c) },
                { typeof(Cmyk), c => PrintCmyk((Cmyk)c) }
            };

            if (printActions.TryGetValue(color.GetType(), out var printAction))
            {
                printAction(color);
            }
            else
            {
                throw new ArgumentException("Unsupported color type");
            }
        }

        public static void StartProgram(string appTitle, string appConsoleTitle, string appDescription)
        {
            Console.Title = appTitle;
            PrintRandomColor(baseConsoleExtensions.SetCenteredText(appConsoleTitle), true, false);
            PrintRandomColor(baseConsoleExtensions.SetCenteredText(appDescription), true, false);

            Console.WriteLine();
            PrintCustomRandomColorLine(0, LineType.Solid, 1);
            Console.WriteLine();
        }

        public static char GetColorOptionInputWithPrompt(this string promptMessage) =>
            ConsoleExtensions.GetInputWithPrompt(promptMessage, Enum.GetValues(typeof(ColorOption))
                                   .Cast<ColorOption>()
                                   .ToDictionary(e => (int)e, e => e.ToString()));
    }
}
