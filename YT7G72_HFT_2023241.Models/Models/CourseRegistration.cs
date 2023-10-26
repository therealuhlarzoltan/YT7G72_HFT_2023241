using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace YT7G72_HFT_2023241.Models
{
    public class CourseRegistration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        public int StudentId { get; set; }
        [Required]
        public int CourseId { get; set; }
        [JsonIgnore]
        public virtual Student Student { get; set; }
        [JsonIgnore]
        public virtual Course Course { get; set; }
    }
}
