using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace YT7G72_HFT_2023241.Models
{
    public class Teacher : IRegistableForSubject, IRegistableForCourse
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TeacherId { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string LastName { get; set; }
        public AcademicRank AcademicRank { get; set; }
        [JsonIgnore]
        public virtual ICollection<Course> RegisteredCourses { get; set; } = new HashSet<Course>();
        [JsonIgnore]
        public virtual ICollection<Subject> RegisteredSubjects { get; set; } = new HashSet<Subject>();
        [JsonIgnore]
        public virtual ICollection<Grade> GivenGrades { get; set; } = new HashSet<Grade>();

        public override string ToString()
        {
            return $"{FirstName} {LastName} -- {AcademicRank}";
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            var teacher = obj as Teacher;
            if (teacher == null) return false;
            return this.TeacherId == teacher.TeacherId
                && this.FirstName == teacher.FirstName
                && this.LastName == teacher.LastName
                && this.AcademicRank == teacher.AcademicRank;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TeacherId, FirstName, LastName, AcademicRank);
        }
    }
}
 