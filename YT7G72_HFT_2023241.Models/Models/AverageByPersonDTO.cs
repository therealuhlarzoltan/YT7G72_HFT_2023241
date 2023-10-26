using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT7G72_HFT_2023241.Models
{
    public class AverageByPersonDTO<T> where T : IRegistableForCourse, IRegistableForSubject
    {
        public AverageByPersonDTO(T person, double average) 
        {
            Person = person;
            Average = average;
        }
        public T Person { get; set; }
        public double Average { get; set; }

        public override string ToString()
        {
            return $"{Person.GetType().Name}: {Person}, Average: {Math.Round(Average, 2)}";
        }
    }
}
