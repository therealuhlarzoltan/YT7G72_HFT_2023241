using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Logic
{
    public class CourseIsFullException : Exception
    {
        public Course Course { get; }

        public CourseIsFullException(Course course) : base() 
        {
            Course = course;
        }
    }
}
