namespace ColorConverter.ValueObjects
{
    public abstract class ColorBase : IColor
    {
        public abstract bool IsValid();
        public override abstract string ToString();
    }
}
