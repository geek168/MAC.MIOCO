using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MAC.MIOCO.Converters
{
    public class ItemSizeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = "无";
            switch (value.ToString())
            {
                case "1":
                    ret = "XS";
                    break;
                case "2":
                    ret = "S";
                    break;
                case "3":
                    ret = "M";
                    break;
                case "4":
                    ret = "L";
                    break;
                case "5":
                    ret = "XL";
                    break;
                case "6":
                    ret = "XXL";
                    break;
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
