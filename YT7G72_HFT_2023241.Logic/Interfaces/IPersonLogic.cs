using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Logic
{
    public interface IPersonLogic
    {
        void AddTeacher(Teacher teacher);
        void RemoveTeacher(int id);
        void UpdateTeacher(Teacher teacher);
        Teacher GetTeacher(int id);
        IEnumerable<Teacher> GetTeachers();
        void AddStudent(Student student);
        void RemoveStudent(int id);
        void UpdateStudent(Student student);
        Student GetStudent(int id);
        IEnumerable<Student> GetStudents();
        IEnumerable<Tuple<Student, double>> GetBestStudents();
        IEnumerable<Tuple<Teacher, double>> GetBestTeachersByAcademicRank(AcademicRank academicRank);
        IEnumerable<Tuple<Teacher, double>> GetBestTeachers();
        string GetSchedule<T>(int id);
        static bool ValidatePerson<T>(T person)
        {
            Type type = person.GetType();
            var properties = type.GetProperties();

            if (type == typeof(Student))
            {
                string regEx = "^[A-Z0-9]{6}$";
                string propValue = (string)properties.Where(p => p.Name == "StudentCode").First().GetValue(person);
                if (!System.Text.RegularExpressions.Regex.IsMatch(propValue, regEx))
                {
                    return false;
                }
            }

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
