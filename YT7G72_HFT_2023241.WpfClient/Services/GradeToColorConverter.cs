using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace YT7G72_HFT_2023241.WpfClient.Services
{
    public class GradeToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double dval)
            {
                if (dval <= 2)
                    return Brushes.Red;
                if (dval <= 3)
                    return Brushes.OrangeRed;
                if (dval <= 4)
                    return Brushes.Yellow;

                return Brushes.Green;

            }
            else
            {
                return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Brush brush)
            {
                if (brush == Brushes.Red)
                {
                    return 2;
                }
                if (brush == Brushes.OrangeRed)
                {
                    return 3;
                }
                if (brush == Brushes.Yellow)
                {
                    return 4;
                }
                return Brushes.Green;
            }
            else
            {
                return 0;
            }
        }
    }
}
