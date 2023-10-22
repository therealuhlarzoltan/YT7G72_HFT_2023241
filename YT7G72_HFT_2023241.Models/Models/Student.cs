using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Models
{
    public class Student : IRegistableForSubject, IRegistableForCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int StudentId { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        public string StudentCode { get; set; }
        public FinancialStatus FinancialStatus { get; set; }
        [Required]
        public int CurriculumId { get; set; }
        [JsonIgnore]
        public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; }
        [JsonIgnore]
        public virtual ICollection<SubjectRegistration> SubjectRegistrations { get; set; } = new HashSet<SubjectRegistration>();
        [JsonIgnore]
        public virtual ICollection<Subject> RegisteredSubjects { get; set; } = new HashSet<Subject>();
        [JsonIgnore]
        public virtual ICollection<Course> RegisteredCourses { get; set; } = new HashSet<Course>();
        [JsonIgnore]
        public virtual ICollection<Grade> Grades { get; set; } = new HashSet<Grade>();
        [JsonIgnore]
        public virtual Curriculum Curriculum { get; set; }

        public override string ToString()
        {
            return $"{FirstName} {LastName} -- {StudentCode}";
        }

    }
}
