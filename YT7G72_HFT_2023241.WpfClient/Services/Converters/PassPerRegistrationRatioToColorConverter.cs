using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace YT7G72_HFT_2023241.WpfClient.Services.Converters
{
    public class PassPerRegistrationRatioToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double dvalue)
            {
                if (dvalue < 0.2)
                    return Brushes.Red;
                if (dvalue < 0.4)
                    return Brushes.OrangeRed;
                if (dvalue < 0.6)
                    return Brushes.Yellow;
                if (dvalue < 0.8)
                    return Brushes.BlueViolet;

                return Brushes.Green;
            }
            else
            {
                return Brushes.Red;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
