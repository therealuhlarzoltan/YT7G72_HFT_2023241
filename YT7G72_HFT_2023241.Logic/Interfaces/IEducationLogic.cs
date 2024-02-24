using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using YT7G72_HFT_2023241.Models;
using Microsoft.AspNetCore.SignalR;

namespace YT7G72_HFT_2023241.Logic
{
    public interface IEducationLogic
    {
        void AddSubject(Subject subject);
        void RemoveSubject(int id);
        void UpdateSubject(Subject subject);
        Subject GetSubject(int id);
        IEnumerable<Subject> GetAllSubjects();
        void AddCourse(Course course);
        void RemoveCourse(int id);
        void UpdateCourse(Course course);
        Course GetCourse(int id);
        IEnumerable<Course> GetAllCourses();
        void RegisterStudentForSubject(int studentId, int subjectId, Action<Student, Subject> callBack = null);
        void RemoveStudentFromSubject(int studentId, int subjectId, Action<Student, Subject> callBack = null);
        void RegisterStudentForCourse(int studentId, int courseId, Action<Student, Course> callBack = null);
        void RemoveStudentFromCourse(int studentId, int courseId, Action<Student, Course> callBack = null);
        void ResetSemester(Action<Subject, Course> callBack = null);
        static bool ValidateObject<T>(T obj)
        {
            Type type = obj.GetType();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var attributes = property.GetCustomAttributes();
                foreach (var attribute in attributes)
                {
                    var requiredAttr = attribute as RequiredAttribute;
                    var lentgthAttr = attribute as StringLengthAttribute;
                    var rangeAttr = attribute as RangeAttribute;

                    if (requiredAttr != null)
                    {
                        if (property.GetValue(obj) == null)
                        {
                            return false;
                        }
                    }

                    if (lentgthAttr != null)
                    {
                        string propertyValue = (string)property.GetValue(obj);
                        if (propertyValue.Length > lentgthAttr.MaximumLength || propertyValue.Length < lentgthAttr.MinimumLength)
                        {
                            return false;
                        }
                    }

                    if (rangeAttr != null)
                    {
                        int propertyValue = (int)property.GetValue(obj);
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
