using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;
using System.Text.Json.Serialization;

namespace YT7G72_HFT_2023241.Models
{
    public class Subject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubjectId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string SubjectName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string SubjectCode { get; set; }
        [Required]
        public int Credits { get; set; }
        [Required]
        public Requirement Requirement { get; set; }
        [JsonIgnore]
        public virtual Subject PreRequirement { get; set; }
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public int CurriculumId { get; set; }
        public int? PreRequirementId { get; set; }
        [JsonIgnore]
        public virtual ICollection<Student> RegisteredStudents { get; set; } = new HashSet<Student>();
        [JsonIgnore]
        public virtual ICollection<SubjectRegistration> SubjectRegistrations { get; set; } = new HashSet<SubjectRegistration>();
        [JsonIgnore]
        public virtual ICollection<Course> SubjectCourses { get; set; } = new HashSet<Course>();
        [JsonIgnore]
        public virtual Teacher Teacher { get; set; }
        [JsonIgnore]
        public virtual Curriculum Curriculum { get; set; }
        [JsonIgnore]
        public virtual ICollection<Grade> Grades { get; set; } = new HashSet<Grade>();

        public override string ToString()
        {
            return $"{SubjectCode} - {SubjectName}";
        }
    }
}
