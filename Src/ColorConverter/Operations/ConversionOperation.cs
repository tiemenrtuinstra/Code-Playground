using ColorConverter.Enums;

namespace ColorConverter.Operations;

internal struct ConversionOperation
{
    public Func<IValidatable> conversionFunc;
    public Action<IValidatable> printFunc;
    public ColorOption colorOption;

    public ConversionOperation(Func<IValidatable> conversionFunc,
        Action<IValidatable> printFunc, ColorOption colorOption)
    {
        this.conversionFunc = conversionFunc;
        this.printFunc = printFunc;
        this.colorOption = colorOption;
    }
}