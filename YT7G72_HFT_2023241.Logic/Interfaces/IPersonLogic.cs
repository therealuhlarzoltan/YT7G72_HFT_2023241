﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
        IEnumerable<Tuple<Teacher, double>> GetBestTeachersByAcademicRank(AcademicRank academicRank);
        IEnumerable<Tuple<Teacher, double>> GetBestTeachers();
        string GetSchedule<T>(int id);
        static bool ValidatePerson<T>(T person)
        {
            Type type = person.GetType();
            var properties = type.GetProperties();
            foreach ( var property in properties )
            {
                var attributes = property.GetCustomAttributes();
                foreach ( var attribute in attributes )
                {
                    var requiredAttr = attribute as RequiredAttribute;
                    var lentgthAttr = attribute as StringLengthAttribute;
                    var rangeAttr = attribute as RangeAttribute;
                    
                    if (requiredAttr != null)
                    {
                        if (property.GetValue(person) == null)
                        {
                            return false;
                        }
                    }

                    if (lentgthAttr != null)
                    {
                        string propertyValue = (string)property.GetValue(person);
                        if (propertyValue.Length > lentgthAttr.MaximumLength || propertyValue.Length < lentgthAttr.MinimumLength)
                        {
                            return false;
                        }
                    }

                    if (rangeAttr != null)
                    {
                        int propertyValue = (int)property.GetValue(person);
                        if (propertyValue > (int)rangeAttr.Maximum || propertyValue < (int)rangeAttr.Minimum)
                        {
                            return false;
                        }
                    }


                }

            }
            return true;
        }

        

    }
}
