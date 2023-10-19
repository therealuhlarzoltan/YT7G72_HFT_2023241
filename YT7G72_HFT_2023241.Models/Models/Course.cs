using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace YT7G72_HFT_2023241.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CourseName { get; set; }
        [Required]
        [StringLength(50)]
        public string CourseCode { get; set; }
        public int CourseCapacity { get; set; }
        public CourseType CourseType { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        [Required]
        [Range(0, 24)]
        public int StartingHour { get; set; }
        [Required]
        public int LengthInMinutes { get; set; }
        [Required]
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
        [JsonIgnore]
        public virtual ICollection<Student> EnrolledStudents { get; set; } = new HashSet<Student>();
        
    }
}
