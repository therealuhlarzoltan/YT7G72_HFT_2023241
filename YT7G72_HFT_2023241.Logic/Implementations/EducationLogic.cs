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
            throw new NotImplementedException();
        }

        public void AddCurriculum(Curriculum curriculum)
        {
            curriculumRepository.Create(curriculum);
        }

        public void AddStudentToCurriculum(Student student, Curriculum curriculum)
        {
            throw new NotImplementedException();
        }

        public void AddSubject(Subject subject)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Course> GetAllCourses()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public Curriculum GetCurriculum(int id)
        {
            throw new NotImplementedException();
        }

        public Subject GetSubject(int id)
        {
            return subjectRepository.Read(id);
        }

        public void RegisterStudentForCourse(Student student, Course course)
        {
            throw new NotImplementedException();
        }

        public void RegisterStudentForSubject(Student student, Subject subject)
        {
            throw new NotImplementedException();
        }

        public void RemoveCourse(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveCurriculum(int id)
        {
            throw new NotImplementedException();
        }

        public void RemoveStudentFromCourse(Student student, Course course)
        {
            throw new NotImplementedException();
        }

        public void RemoveStudentFromCurriculum(Student student)
        {
            throw new NotImplementedException();
        }

        public void RemoveStudentFromSubject(Student student, Subject subject)
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
