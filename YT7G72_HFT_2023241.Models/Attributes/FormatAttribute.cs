using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT7G72_HFT_2023241.Models
{
    public class FormatAttribute : Attribute
    {
        public FormatAttribute(string formatDescription)
        {
            FormatDescription = formatDescription;
        }

        public string FormatDescription { get; set; }
    }
}
