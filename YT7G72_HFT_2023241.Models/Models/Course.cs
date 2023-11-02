using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
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
        [Format("String between 4 and 50 characters")]
        public string CourseName { get; set; }
        [Required]
        public int CourseCapacity { get; set; }
        public CourseType CourseType { get; set; }
        [Required]
        public DayOfWeek DayOfWeek { get; set; }
        [Required]
        [Format("HH:MM")]
        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan StartTime { get; set; }
        [Required]
        [StringLength (50, MinimumLength = 4)]
        [Format("String between 4 and 50 characters")]
        public string Room { get; set; }
        [Required]
        public int LengthInMinutes { get; set; }
        public int? TeacherId { get; set; }
        [Required]
        public int SubjectId { get; set; }
        [JsonIgnore]
        public virtual Subject Subject { get; set; }
        public virtual Teacher Teacher { get; set; }
        [JsonIgnore]
        public virtual ICollection<CourseRegistration> CourseRegistrations { get; set; } = new HashSet<CourseRegistration>();
        [JsonIgnore]
        public virtual ICollection<Student> EnrolledStudents { get; set; } = new HashSet<Student>();

        public override bool Equals(object obj)
        {
            return obj is Course course &&
                   CourseId == course.CourseId &&
                   CourseName == course.CourseName &&
                   CourseCapacity == course.CourseCapacity &&
                   CourseType == course.CourseType &&
                   DayOfWeek == course.DayOfWeek &&
                   StartTime.Equals(course.StartTime) &&
                   Room == course.Room &&
                   TeacherId == course.TeacherId &&
                   SubjectId == course.SubjectId;
        }

        public override int GetHashCode()
        {
            HashCode hash = new HashCode();
            hash.Add(CourseId);
            hash.Add(CourseName);
            hash.Add(CourseCapacity);
            hash.Add(CourseType);
            hash.Add(DayOfWeek);
            hash.Add(StartTime);
            hash.Add(Room);
            hash.Add(TeacherId);
            hash.Add(SubjectId);
            return hash.ToHashCode();
        }

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
