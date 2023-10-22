﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Logic
{
    public interface IEducationLogic
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
        void RegisterStudentForSubject(int studentId, int subjectId);
        void RemoveStudentFromSubject(int studentId, int subjectId);
        //course registration logic
        void RegisterStudentForCourse(int studentId, int courseId);
        void RemoveStudentFromCourse(int studentId, int courseId);
        //curriculum registration logic
        void AddStudentToCurriculum(int studentId, int curriculumId);
        void RemoveStudentFromCurriculum(Student student);
        //semester reset logic
        void ResetSemester();

    }
}
