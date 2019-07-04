using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Turbo.turbo
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {

            var r = new Regex("\\d{4}-\\d{2}-\\d{2}");
            if (!r.IsMatch((String)value))
            {
                //DateTime dt = (DateTime)value; DateTime.ParseExact(strDate, "yyyyMMdd", null);
                DateTime dt = DateTime.Parse((String)value);
                return dt.ToString("yyyy-MM-dd");
            }
            else
            {
                return value;
            }
            
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
