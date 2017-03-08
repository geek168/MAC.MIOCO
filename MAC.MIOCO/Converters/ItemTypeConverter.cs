using MAC.MIOCO;
using MAC.MIOCO.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace MAC.MIOCO.Converters
{
    public class ItemTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ret = "";
            var type = int.Parse(value.ToString());
            var item = ItemTypeModel.ItemTypeModelList.FirstOrDefault(s => s.Type == type);
            if (item != null)
            {
                ret = item.ItemTypeName;
            }
            return ret;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
