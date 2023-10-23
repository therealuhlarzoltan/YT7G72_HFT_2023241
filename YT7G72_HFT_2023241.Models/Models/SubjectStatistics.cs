using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT7G72_HFT_2023241.Models
{
    public class SubjectStatistics
    {
        public Subject Subject { get; set; }
        public int NumberOfRegistrations { get; set; }
        public double PassPerRegistrationRatio { get; set; }
        public double Avg {  get; set; }
    }
}
