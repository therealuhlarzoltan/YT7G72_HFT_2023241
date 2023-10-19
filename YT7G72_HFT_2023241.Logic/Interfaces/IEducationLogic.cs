using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Logic
{
    internal interface IEducationLogic
    {
        //subject CRUD logic
        void AddSubject(Subject subject);
        void RemoveSubject(int id);
        void UpdateSubject(Subject subject);
        Subject GetSubject(int id);
        IEnumerable<Subject> GetAllSubjects();
        //course CRUD logic
        void AddCourse(Course course);
        void RemoveCourse(int id);
        void UpdateCourse(Course course);
        Course GetCourse(int id);
        IEnumerable<Course> GetAllCourses();
        //curriculum CRUD logic
        void AddCurriculum(Curriculum curriculum);
        void RemoveCurriculum(int id);
        void UpdateCurriculum(Curriculum curriculum);
        Curriculum GetCurriculum(int id);
        IEnumerable<Curriculum> GetAllCurriculums();
        //subject registration logic
        void RegisterStudentForSubject(Student student, Subject subject);
        void RemoveStudentFromSubject(Student student, Subject subject);
        //course registration logic
        void RegisterStudentForCourse(Student student, Course course);
        void RemoveStudentFromCourse(Student student, Course course);
        //curriculum registration logic
        void AddStudentToCurriculum(Student student, Curriculum curriculum);
        void RemoveStudentFromCurriculum(Student student);
        //semester reset logic
        void ResetSemester();

    }
}
