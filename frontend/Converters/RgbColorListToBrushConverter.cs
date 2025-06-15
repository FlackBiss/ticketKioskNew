using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using Color = System.Windows.Media.Color;
using ColorConverter = System.Windows.Media.ColorConverter;

namespace Lastik.Converters
{
    public class RgbColorListToBrushConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string hexString && !string.IsNullOrWhiteSpace(hexString))
            {
                return (Color)ColorConverter.ConvertFromString("#FF00D0");
            }
            return Colors.Gray; // Возвращаем прозрачный цвет при ошибке
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
