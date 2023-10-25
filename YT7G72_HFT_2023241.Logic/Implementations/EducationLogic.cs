using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.Repository;

namespace YT7G72_HFT_2023241.Logic
{
    public class EducationLogic : IEducationLogic
    {
        private IRepository<Student> studentRepository;
        private IRepository<Course> courseRepository;
        private IRepository<Subject> subjectRepository;

        public EducationLogic(IRepository<Student> studentRepository, IRepository<Course> courseRepository, IRepository<Subject> subjectRepository)
        {
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
            this.subjectRepository = subjectRepository;
        }

        public void AddCourse(Course course)
        {
            courseRepository.Create(course);
        }


        public void AddSubject(Subject subject)
        {
            bool isValid = IEducationLogic.ValidateObject<Subject>(subject);
            if (!isValid)
            {
                throw new ArgumentException("Invalid argument(s) provided!");
            }
            try
            {
                subjectRepository.Create(subject);
            }
            catch (Exception) {
                throw new ArgumentException("Failed to update database, most likely due to foreign key constraint violation");
            }
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return courseRepository.ReadAll();
        }

        public IEnumerable<Subject> GetAllSubjects()
        {
            return subjectRepository.ReadAll();
        }

        public Course GetCourse(int id)
        {
            var course = courseRepository.Read(id);
            if (course == null)
            {
                throw new ObjectNotFoundException(id, typeof(Course));
            }
            return course;
        }

        public Subject GetSubject(int id)
        {
            var subject = subjectRepository.Read(id);
            if (subject == null)
            {
                throw new ObjectNotFoundException(id, typeof(Subject));
            }
            return subject;
        }

        public void RegisterStudentForCourse(int studentId, int courseId)
        {
            var student = studentRepository.Read(studentId);
            if (student == null)
            {
                throw new ObjectNotFoundException(studentId, typeof(Student));
            }

            var course = courseRepository.Read(courseId);
            if (course == null)
            {
                throw new ObjectNotFoundException(courseId, typeof(Course));
            }

            if (!course.Subject.RegisteredStudents.Contains(student))
            {
                throw new NotRegisteredForSubjectException(student, course.Subject);
            }

            if (course.EnrolledStudents.Count < course.CourseCapacity)
            {
                course.EnrolledStudents.Add(student);
                courseRepository.Update(course);
            } 
            else
            {
                throw new CourseIsFullException(course);
            }
        }

        public void RegisterStudentForSubject(int studentId, int subjectId)
        {
            var student = studentRepository.Read(studentId);
            if (student == null)
            {
                throw new ObjectNotFoundException(studentId, typeof(Student));
            }

            var subject = subjectRepository.Read(subjectId);
            if (subject == null)
            {
                throw new ObjectNotFoundException(subjectId, typeof(Subject));
            }

            if (student.CurriculumId != subject.CurriculumId)
            {
                throw new PreRequirementsNotMetException(student, subject);
            }

            if (subject.PreRequirement != null)
            {
                var newestGrade = student.Grades.Where(grade => grade.SubjectId ==  subject.PreRequirementId)
                    .OrderByDescending(grade => int.Parse(grade.Semester.Split('/')[0]))
                    .ThenByDescending(grade => int.Parse(grade.Semester.Split('/')[1]))
                    .ThenByDescending(grade => int.Parse(grade.Semester.Split('/')[2]))
                    .FirstOrDefault();
                if (newestGrade == null || newestGrade.Mark == 1)
                    throw new PreRequirementsNotMetException(student, subject);
                subject.RegisteredStudents.Add(student);
                subjectRepository.Update(subject);
            } 
            else
            {
                subject.RegisteredStudents.Add(student);
                subjectRepository.Update(subject);
            }
        }

        public void RemoveCourse(int id)
        {
            try
            {
                courseRepository.Delete(id);
            }
            catch (ArgumentNullException)
            {
                throw new ObjectNotFoundException(id, typeof(Course));
            }
            
        }

        public void RemoveStudentFromCourse(int studentId, int courseId)
        {
            var student = studentRepository.Read(studentId);
            if (student == null)
            {
                throw new ObjectNotFoundException(studentId, typeof(Student));
            }

            var course = courseRepository.Read(courseId);
            if (course == null)
            {
                throw new ObjectNotFoundException(courseId, typeof(Course));
            }

            if (course.EnrolledStudents.Contains(student))
            {
                course.EnrolledStudents.Remove(student);
                courseRepository.Update(course);
            }
        }

        public void RemoveStudentFromSubject(int studentId, int subjectId)
        {
            var student = studentRepository.Read(studentId);
            var subject = subjectRepository.Read(subjectId);
            if (student == null)
                throw new ObjectNotFoundException(studentId, typeof(Student));
            if (subject == null)
                throw new ObjectNotFoundException(subjectId, typeof(Subject));
            if (!subject.RegisteredStudents.Contains(student))
                throw new NotRegisteredForSubjectException(student, subject);
            subject.RegisteredStudents.Remove(student);
            subjectRepository.Update(subject);
        }

        public void RemoveSubject(int id)
        {
            try
            {
                subjectRepository.Delete(id);
            }
            catch (ArgumentNullException)
            {
                throw new ObjectNotFoundException(id, typeof(Subject));
            }
        }

        public void UpdateCourse(Course course)
        {
            var old = courseRepository.Read(course.CourseId);
            if (old == null)
                throw new ObjectNotFoundException(course.CourseId, typeof(Course));
            bool isValid = IEducationLogic.ValidateObject<Course>(course);
            if (!isValid)
            {
                throw new ArgumentException("Invalid argument(s) provided!");
            }
            try
            {
                courseRepository.Update(course);
            }
            catch (Exception)
            {
                throw new ArgumentException("Failed to update database, most likely due to foregin key constraint violation");
            }
        }


        public void UpdateSubject(Subject subject)
        {
            var old = subjectRepository.Read(subject.SubjectId);
            if (old == null)
                throw new ObjectNotFoundException(subject.SubjectId, typeof(Subject));
            bool isValid = IEducationLogic.ValidateObject<Subject>(subject);
            if (!isValid)
            {
                throw new ArgumentException("Invalid argument(s) provided!");
            }
            try
            {
                subjectRepository.Update(subject);
            }
            catch (Exception)
            {
                throw new ArgumentException("Failed to update database, most likely due to foregin key constraint violation");
            }
        }

        public void ResetSemester()
        {
            foreach (var subject in subjectRepository.ReadAll())
            {
                foreach (var course in subject.SubjectCourses)
                {
                    course.CourseRegistrations.Clear();
                    courseRepository.Update(course);
                }
                subject.SubjectRegistrations.Clear();
                subjectRepository.Update(subject);
            }
        }
    }
}
