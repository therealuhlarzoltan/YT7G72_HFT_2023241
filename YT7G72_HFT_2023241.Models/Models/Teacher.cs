using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Models
{
    public class Teacher : IRegistableForSubject, IRegistableForCourse
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
        public AcademicRank AcademicRank { get; set; }
        [JsonIgnore]
        public virtual ICollection<Course> RegisteredCourses { get; set; } = new HashSet<Course>();
        [JsonIgnore]
        public virtual ICollection<Subject> RegisteredSubjects { get; set; } = new HashSet<Subject>();

        public override string ToString()
        {
            return $"{FirstName} {LastName} -- {AcademicRank}";
        }
    }
}
