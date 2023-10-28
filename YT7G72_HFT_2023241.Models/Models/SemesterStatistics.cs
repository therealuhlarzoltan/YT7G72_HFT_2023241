using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT7G72_HFT_2023241.Models
{
    public class SemesterStatistics
    {
        public string Semester { get; set; }
        public double WeightedAvg { get; set; }
        public int NumberOfFailures { get; set; }
        public int NumberOfPasses { get; set; }

        public override bool Equals(object obj)
        {
            return obj is SemesterStatistics statistics &&
                   Semester == statistics.Semester &&
                   WeightedAvg == statistics.WeightedAvg &&
                   NumberOfFailures == statistics.NumberOfFailures &&
                   NumberOfPasses == statistics.NumberOfPasses;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Semester, WeightedAvg, NumberOfFailures, NumberOfPasses);
        }
    }

}
