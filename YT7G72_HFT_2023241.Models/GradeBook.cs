using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT7G72_HFT_2023241.Models
{
    internal class GradeBook
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [StringLength(9)]
        public string Semester { get; set; }
        [Required]
        public virtual Student Student { get; set; }
        [Required]
        public virtual Course Course { get; set; }
        [Required]
        [Range(1, 5)]
        public int Grade {  get; set; }
    }
}
