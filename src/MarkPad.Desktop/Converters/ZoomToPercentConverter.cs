using System;
using System.Globalization;
using Avalonia.Data.Converters;

namespace MarkPad.Desktop.Converters;

/// <summary>
/// Converts between zoom decimal value (0.5-3.0) and percentage value (50-300)
/// </summary>
public class ZoomToPercentConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double zoomValue)
        {
            return zoomValue * 100;
        }
        return 100.0;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is double percentValue)
        {
            return percentValue / 100.0;
        }
        return 1.0;
    }
}
