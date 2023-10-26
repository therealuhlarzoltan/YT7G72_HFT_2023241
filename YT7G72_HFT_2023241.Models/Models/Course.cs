using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using System.ComponentModel;
using System.Formats.Asn1;
using System.Net.Http.Json;
using System.Text.Json;

namespace YT7G72_HFT_2023241.Models
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CourseId { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 4)]
        public string CourseName { get; set; }
        [Required]
        public int CourseCapacity { get; set; }
        public CourseType CourseType { get; set; }
        [Required]
        public DayOfWeek DayOfWeek { get; set; }
        [Required]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan StartTime { get; set; }
        [Required]
        public string Room { get; set; }
        [Required]
        public int LengthInMinutes { get; set; }
        [Required]
        public int TeacherId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [JsonIgnore]
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
        [JsonIgnore]
        public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; } = new HashSet<CourseRegistration>();
        [JsonIgnore]
        public virtual ICollection<Student> EnrolledStudents { get; set; } = new HashSet<Student>();

        public override string ToString()
        {
            return $"Course: {CourseName}; Teacher: {Teacher}";
        }

        public class TimeSpanConverter : JsonConverter<TimeSpan>
        {
            public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    if (TimeSpan.TryParse(reader.GetString(), out var timeSpan))
                    {
                        return timeSpan;
                    }
                }
                return TimeSpan.Zero;
            }

            public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }
    }
}
