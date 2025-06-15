using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace Lastik.Converters
{
    public class ItemInListConverter:IMultiValueConverter
    {

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return values[0] is IList list && list.Contains(values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
