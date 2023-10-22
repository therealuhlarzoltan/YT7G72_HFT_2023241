using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Logic.Exceptions
{
    public class NotEnrolledInCourseException : Exception
    {
        public Student Student { get; }
        public Subject Subject { get; }

        public NotEnrolledInCourseException(Student student, Subject subject) : base()
        {
            Student = student;
            Subject = subject;
        }
    }
}
