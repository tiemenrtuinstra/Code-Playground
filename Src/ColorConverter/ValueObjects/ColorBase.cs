namespace ColorConverter.ValueObjects;

public abstract class ColorBase : IColor
{
    internal bool DisplayConsole { get; set; }

    public abstract bool IsValid();
    public override abstract string ToString();
    public bool HasConsole() => DisplayConsole;
}
