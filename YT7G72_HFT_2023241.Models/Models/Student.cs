using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace YT7G72_HFT_2023241.Models
{
    public class Student
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(50)]
        public string LastName { get; set; }
        [Required]
        public string StudentCode { get; set; }
        public FinancialStatus FinancialStatus { get; set; }
        public virtual ICollection<Student> RegisteredSubjects { get; set; } = new HashSet<Student>();
        public virtual ICollection<Course> EnrolledCourses { get; set; } = new HashSet<Course>();


    }
}
