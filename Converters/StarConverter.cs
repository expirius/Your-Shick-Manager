using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.Converters
{
    /*
     * Если ИНДЕКС меньше или равен РЕЙТИНГУ, звезда ЗОЛОТАЯ
     * Если ИНДЕКС больше РЕЙТИНГА, звезда БЕЛАЯ
     */
    public class StarConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            int rating = (int)value;
            int starIndex = int.Parse(parameter.ToString());
            // пикчи золото или белый
            return starIndex <= rating ? "gold_startoilet.png" : "white_startoilet.png";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
