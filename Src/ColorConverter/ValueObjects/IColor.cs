namespace ColorConverter.ValueObjects;

public interface IColor : IValidatable
{
    string ToString();
    bool IsValid();
}