using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace YT7G72_HFT_2023241.Models
{
    public class Grade
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int GradeId { get; set; }
        [Required]
        [StringLength(9, MinimumLength = 9)]
        [Format("e.g. 2023/24/1")]
        public string Semester { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [Required]
        public int TeacherId { get; set; }
        [JsonIgnore]
        public virtual Student Student { get; set; }
        [JsonIgnore]
        public virtual Subject Subject { get; set; }
        [JsonIgnore]
        public virtual Teacher Teacher { get; set; }
        [Required]
        [Format("Integer between 1 and 5")]
        [Range(1, 5)]
        public int Mark { get; set; }
    }
}
