using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT7G72_HFT_2023241.Models
{
    internal class Curriculum
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string CurriculumName { get; set; }
        [Required]
        [StringLength(50)]
        public string CurriculumCode { get; set; }
        public virtual ICollection<Subject> CurriculumSubjects { get; set; } = new HashSet<Subject>();
        public virtual ICollection<Student> CurriculumStudents { get; set; } = new HashSet<Student>();
    }
}
