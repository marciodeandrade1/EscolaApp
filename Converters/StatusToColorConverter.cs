using System.Globalization;
using EscolaApp.Models;

namespace EscolaApp.Converters;

/// <summary>
/// Converte o StatusMatricula em uma cor de fundo para o badge.
/// Use "Background" ou "Text" como ConverterParameter.
/// </summary>
public class StatusToColorConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not StatusMatricula status)
            return Colors.Gray;

        var modo = parameter as string ?? "Background";

        return (status, modo) switch
        {
            (StatusMatricula.Ativa, "Background") => Color.FromArgb("#E4F9F6"),
            (StatusMatricula.Ativa, "Text") => Color.FromArgb("#12C2A9"),

            (StatusMatricula.Concluida, "Background") => Color.FromArgb("#EDEEFF"),
            (StatusMatricula.Concluida, "Text") => Color.FromArgb("#5B5FEF"),

            (StatusMatricula.Cancelada, "Background") => Color.FromArgb("#FDE7E8"),
            (StatusMatricula.Cancelada, "Text") => Color.FromArgb("#E5484D"),

            _ => Colors.Gray
        };
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotSupportedException();
}
