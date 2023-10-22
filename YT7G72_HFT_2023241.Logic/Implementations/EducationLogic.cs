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
        private IRepository<Curriculum> curriculumRepository;

        public EducationLogic(IRepository<Student> studentRepository, IRepository<Course> courseRepository, IRepository<Subject> subjectRepository, IRepository<Curriculum> curriculumRepository)
        {
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
            this.subjectRepository = subjectRepository;
            this.curriculumRepository = curriculumRepository;
        }

        public void AddCourse(Course course)
        {
            courseRepository.Create(course);
        }

        public void AddCurriculum(Curriculum curriculum)
        {
            curriculumRepository.Create(curriculum);
        }

        public void AddStudentToCurriculum(int studentId, int curriculumId)
        {
            var student = studentRepository.Read(studentId);
            if (student == null)
            {
                throw new ObjectNotFoundException(studentId, typeof(Student));
            }

            var curriculum = curriculumRepository.Read(curriculumId);
            if (curriculum == null)
            {
                throw new ObjectNotFoundException(curriculumId, typeof(Curriculum));
            }

            student.Curriculum = curriculum;
            studentRepository.Update(student);
        }

        public void AddSubject(Subject subject)
        {
            subjectRepository.Create(subject);
        }

        public IEnumerable<Course> GetAllCourses()
        {
            return courseRepository.ReadAll();
        }

        public IEnumerable<Curriculum> GetAllCurriculums()
        {
            return curriculumRepository.ReadAll();
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

        public Curriculum GetCurriculum(int id)
        {
            var curriculum = curriculumRepository.Read(id);
            if (curriculum == null)
            {
                throw new ObjectNotFoundException(id, typeof(Curriculum));
            }
            return curriculum;
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

            if (student.CurriculumId != subject.Curriculum.Id)
            {
                throw new PreRequirementsNotMetException(student, subject);
            }

            if (subject.PreRequirement != null)
            {
                var newestGrade = student.Grades.Where(grade => grade.SubjectId == subjectId)
                    .OrderByDescending(grade => int.Parse(grade.Semester.Split('/')[0]))
                    .OrderByDescending(grade => int.Parse(grade.Semester.Split('/')[1]))
                    .OrderByDescending(grade => int.Parse(grade.Semester.Split('/')[2]))
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
            throw new NotImplementedException();
        }

        public void RemoveCurriculum(int id)
        {
            throw new NotImplementedException();
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

        public void RemoveStudentFromCurriculum(Student student)
        {
            throw new NotImplementedException();
        }

        public void RemoveStudentFromSubject(int studentId, int subjectId)
        {
            throw new NotImplementedException();
        }

        public void RemoveSubject(int id)
        {
            throw new NotImplementedException();
        }

        public void ResetSemester()
        {
            throw new NotImplementedException();
        }

        public void UpdateCourse(Course course)
        {
            throw new NotImplementedException();
        }

        public void UpdateCurriculum(Curriculum curriculum)
        {
            throw new NotImplementedException();
        }

        public void UpdateSubject(Subject subject)
        {
            throw new NotImplementedException();
        }
    }
}
