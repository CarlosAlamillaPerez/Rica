namespace bepensa_socio_selecto_models.Attributes;

public class CssClassAttribute : Attribute
{
    public string Name { get; } = null!;

    public CssClassAttribute(string className)
    {
        Name = className;
    }
}