using System.Globalization;

namespace EscolaApp.Converters;

/// <summary>Retorna true quando a string vinculada não é nula/vazia. Útil para IsVisible de mensagens de erro.</summary>
public class IsNotEmptyConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => value is string s && !string.IsNullOrWhiteSpace(s);

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
