﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Logic
{
    public interface IPersonLogic
    {
        //teacher CRUD logic
        void AddTeacher(Teacher teacher);
        void RemoveTeacher(int id);
        void UpdateTeacher(Teacher teacher);
        Teacher GetTeacher(int id);
        IEnumerable<Teacher> GetTeachers();
        //student CRUD logic
        void AddStudent(Student student);
        void RemoveStudent(int id);
        void UpdateStudent(Student student);
        Student GetStudent(int id);
        IEnumerable<Student> GetStudents();
        //non-CRUD logic
        IEnumerable<Tuple<Student, double>> GetBestStudents();
        IEnumerable<Tuple<Teacher, double>> GetBestLecturers();
        IEnumerable<Tuple<Teacher, double>> GetBestTeachers();
        string GetSchedule<T>(int id);

        

    }
}
