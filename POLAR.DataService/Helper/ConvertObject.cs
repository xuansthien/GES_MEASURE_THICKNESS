using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace POLAR.DataService.Helper
{
    public class ConvertObject : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (values != null)
            {
                return values.ToArray();
            }
            return " ";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, System.Globalization.CultureInfo culture)
        {
            //string[] values = null;
            //if (value != null)
            //    return values = value.ToString().Split(' ');
            //return values;
            throw new NotImplementedException();
        }

    }
}
