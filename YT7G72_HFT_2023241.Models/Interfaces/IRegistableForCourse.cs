using System.Collections.Generic;

namespace YT7G72_HFT_2023241.Models
{
    public interface IRegistableForCourse
    {
        ICollection<Course> RegisteredCourses { get; set; }
    }
}
