namespace ColorConverter.Extensions;

public static class ColorComponentExtensions
{
    public static bool IsValidComponent(this int color) => color >= 0 && color <= 255;

    public static bool IsValidComponent(this double color) => color >= 0 && color <= 100;

}
