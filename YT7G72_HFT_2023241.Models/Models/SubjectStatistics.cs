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

        public override bool Equals(object obj)
        {
            return obj is SubjectStatistics statistics &&
                   EqualityComparer<Subject>.Default.Equals(Subject, statistics.Subject) &&
                   NumberOfRegistrations == statistics.NumberOfRegistrations &&
                   PassPerRegistrationRatio == statistics.PassPerRegistrationRatio &&
                   Avg == statistics.Avg;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Subject, NumberOfRegistrations, PassPerRegistrationRatio, Avg);
        }

        public override string ToString()
        {
            return $"{Subject};{NumberOfRegistrations};{PassPerRegistrationRatio};{Avg}";
        }
    }
}
