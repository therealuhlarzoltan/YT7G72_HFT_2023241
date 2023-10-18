using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models.Enums;

namespace YT7G72_HFT_2023241.Models
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string SubjectName { get; set; }
        [Required]
        [StringLength(50)]
        public string SubjectCode { get; set; }
        [Required]
        public int Credits { get; set; }
        public Requirement Requirement { get; set; }
        public virtual Subject PreRequirement { get; set; }
        public virtual ICollection<Student> RegisteredStudents { get; set; } = new HashSet<Student>();
        public virtual ICollection<Course> SubjectCourses { get; set; } = new HashSet<Course>();
    }
}
