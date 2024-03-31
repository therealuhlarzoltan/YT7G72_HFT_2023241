using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace YT7G72_HFT_2023241.WpfClient.Services.Converters
{
    public class MarkToGradeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int ival)
            {
                switch (ival)
                {
                    case 2:
                        return "Sufficient";
                    case 3:
                        return "Satisfactory";
                    case 4:
                        return "Good";
                    case 5:
                        return "Excellent";
                    default:
                        return "Insufficient";
                }
            }
            else
            {
                return "Insufficient";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string sval)
            {
                switch (sval.ToLower())
                {
                    case "sufficient":
                        return 2;
                    case "satisfactory":
                        return 3;
                    case "good":
                        return 4;
                    case "excellent":
                        return 5;
                    default:
                        return 1;
                }
            }
            else
            {
                return 1;
            }
        }
    }
}
