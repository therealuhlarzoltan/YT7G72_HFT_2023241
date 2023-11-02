using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Logic.Interfaces
{
    public interface ICurriculumLogic
    {
        IEnumerable<Curriculum> GetCurriculums();
        Curriculum GetCurriculum(int id);
        void RemoveCurriculum(int id);
        void AddCurriculum(Curriculum curriculum);
        void UpdateCurriculum(Curriculum curriculum);
        static bool ValidateCurriculum(Curriculum curriculum)
        {
            Type type = curriculum.GetType();
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
                        if (property.GetValue(curriculum) == null)
                        {
                            return false;
                        }
                    }

                    if (lentgthAttr != null)
                    {
                        string propertyValue = (string)property.GetValue(curriculum);
                        if (propertyValue.Length > lentgthAttr.MaximumLength || propertyValue.Length < lentgthAttr.MinimumLength)
                        {
                            return false;
                        }
                    }

                    if (rangeAttr != null)
                    {
                        int propertyValue = (int)property.GetValue(curriculum);
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
