using System;

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
