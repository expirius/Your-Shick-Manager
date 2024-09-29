using MFASeeker.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFASeeker.Converters
{
    public class RaitingPNGConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is (double tmp))
            {
                return ToiletIconProvider.GetIconName(tmp);
            }
            return @"defaultrank_toilet.svg";
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
