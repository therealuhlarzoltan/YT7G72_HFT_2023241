using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.Repository;

namespace YT7G72_HFT_2023241.Logic
{
    public class PersonLogic : IPersonLogic
    {
        private IRepository<Student> studentRepository;
        private IRepository<Teacher> teacherRepository;

        public PersonLogic(IRepository<Student> studentRepository, IRepository<Teacher> teacherRepository)
        {
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
        }

        public void AddStudent(Student student)
        {
            
            studentRepository.Create(student);
        }

        public void AddTeacher(Teacher teacher)
        {
            
            teacherRepository.Create(teacher);
        }

        public IEnumerable<Student> GetBestStudents()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Teacher> GetBestTeachers()
        {
            throw new NotImplementedException();
        }

        public Student GetStudent(int id)
        {
            var student = studentRepository.Read(id);
            if (student == null)
                throw new ObjectNotFoundException(id, typeof(Student));
            return student;
        }

        public IEnumerable<Student> GetStudents()
        {
            return studentRepository.ReadAll();
        }

        public Teacher GetTeacher(int id)
        {
            var teacher = teacherRepository.Read(id);
            if (teacher == null)
                throw new ObjectNotFoundException(id, typeof(Teacher));
            return teacher;
        }

        public IEnumerable<Teacher> GetTeachers()
        {
            return teacherRepository.ReadAll();
        }

        public void RemoveStudent(int id)
        {
            try
            {
                studentRepository.Delete(id);
            }
            catch (ArgumentNullException) 
            {
                throw new ObjectNotFoundException(id, typeof(Student));
            }
        }

        public void RemoveTeacher(int id)
        {
            try
            {
                teacherRepository.Delete(id);
            }
            catch(ArgumentNullException)
            {
                throw new ObjectNotFoundException(id, typeof(Teacher));
            }
        }

        public void UpdateStudent(Student student)
        {
            var originalStudent = studentRepository.Read(student.StudentId);
            if (originalStudent == null)
                throw new ObjectNotFoundException(student.StudentId, typeof(Student));
            studentRepository.Update(student);
        }

        public void UpdateTeacher(Teacher teacher)
        {
            var originalTeacher = teacherRepository.Read(teacher.TeacherId);
            if (originalTeacher == null)
                throw new ObjectNotFoundException(teacher.TeacherId, typeof(Teacher));
            teacherRepository.Update(teacher);
        }

        public string GetSchedule<T>(int id)
        {
            Type type = typeof(T);
            string schedule = string.Empty;

            if (type == typeof(Student))
            {
                var student = studentRepository.Read(id);
                var coursesGroupedbyDay = student.RegisteredCourses.GroupBy(course => course.DayOfWeek);
                foreach (var day in Enum.GetNames(typeof(DayOfWeek)))
                {
                    schedule += $"{day}:\n\n";
                    foreach (var courseGroup in coursesGroupedbyDay.Where(group => group.Key == (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day)))
                    {
                        var orderedCourseGroup = courseGroup.OrderBy(g => g.StartTime);
                        foreach (var course in orderedCourseGroup)
                        {
                            schedule += $"\t{course.Subject.SubjectName} - {course.CourseName};\n";
                            schedule += $"\t\tCourse Type: {course.CourseType}\n";
                            schedule += $"\t\tTeacher: {course.Teacher.FirstName} {course.Teacher.LastName}\n";
                            schedule += $"\t\tRoom: {course.Room}\n";
                            schedule += $"\t\tTime: {course.StartTime} - {course.StartTime + new TimeSpan( 0, course.LengthInMinutes, 0)}\n";
                            schedule += $"\t\tCapacity: {course.EnrolledStudents.Count}/{course.CourseCapacity}\n";
                        }

                    }
                }
            }
            else if (type == typeof(Teacher))
            {
                var teacher = teacherRepository.Read(id);
                var coursesGroupedbyDay = teacher.RegisteredCourses.GroupBy(course => course.DayOfWeek);
                foreach (var day in Enum.GetNames(typeof(DayOfWeek)))
                {
                    schedule += $"{day}:\n\n";
                    foreach (var courseGroup in coursesGroupedbyDay.Where(group => group.Key == (DayOfWeek)Enum.Parse(typeof(DayOfWeek), day)))
                    {
                        var orderedCourseGroup = courseGroup.OrderBy(g => g.StartTime);
                        foreach (var course in orderedCourseGroup)
                        {
                            schedule += $"\t{course.Subject.SubjectName} - {course.CourseName};\n";
                            schedule += $"\t\tCourse Type: {course.CourseType}\n";
                            schedule += $"\t\tRoom: {course.Room}\n";
                            schedule += $"\t\tTime: {course.StartTime} - {course.StartTime + new TimeSpan(0, course.LengthInMinutes, 0)}\n";
                            schedule += $"\t\tCapacity: {course.EnrolledStudents.Count}/{course.CourseCapacity}\n";
                        }

                    }
                }
            }

            return schedule;
        }
    }
}
