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
        [Format("String between 2 and 50 characters")]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [Format("String between 2 and 50 characters")]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }
        [Required]
        [Format("String with 6 characters, containing only English upper case letters and numbers")]
        [StringLength(6, MinimumLength = 6)]
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

        public override bool Equals(object obj)
        {
            if (obj == null) return false ;
            var student = obj as Student;
            if (student == null) return false ;
            return this.StudentId == student.StudentId
                && this.StudentCode == student.StudentCode
                && this.FirstName == student.FirstName
                && this.LastName == student.LastName;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(StudentId, StudentCode, FirstName, LastName);
        }

    }
}
