using System;
using System.Collections.Generic;
using System.Linq;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.Repository;

namespace YT7G72_HFT_2023241.Logic
{
    public class GradeLogic : IGradeLogic
    {
        private IRepository<Grade> gradeRepository;

        public GradeLogic(IRepository<Grade> gradeRepository)
        {
            this.gradeRepository = gradeRepository;
        }

        public IEnumerable<Grade> GetAllGrades()
        {
            return gradeRepository.ReadAll();
        }

        public Grade GetGrade(int id)
        {
            var grade = gradeRepository.Read(id);
            if (grade == null)
                throw new ObjectNotFoundException(id, typeof(Grade));
            return grade;
        }

        public IEnumerable<SemesterStatistics> GetSemesterStatistics()
        {
            var resultSet = new List<SemesterStatistics>();
            var semesterGroups = gradeRepository.ReadAll().Select(grade => grade.Semester).AsEnumerable().GroupBy(semester => semester)
                .OrderBy(semesterGroup => semesterGroup.Count());
            foreach (var group in semesterGroups)
            {
                resultSet.Add(GetSemesterStatistics(group.Key));
            }
            return resultSet.OrderBy(stat => int.Parse(stat.Semester.Split('/')[0])).
                ThenBy(stat => int.Parse(stat.Semester.Split('/')[1])).ThenBy(stat => int.Parse(stat.Semester.Split('/')[2]));
        }

        public SemesterStatistics GetSemesterStatistics(string semester)
        {
            var relevantGrades = gradeRepository.ReadAll().Where(grade => grade.Semester ==  semester && grade.Subject.Credits > 0);
            if (!relevantGrades.Any())
            {
                return new SemesterStatistics() { Semester = semester, NumberOfFailures = -1, NumberOfPasses = -1, WeightedAvg = -1 };
            }
            int failures = relevantGrades.Count(grade => grade.Mark == 1);
            int passes = relevantGrades.Count(grade => grade.Mark > 1);
            double average = (double)relevantGrades.Sum(grade => grade.Mark * grade.Subject.Credits) / (double)relevantGrades.Sum(grade => grade.Subject.Credits);
            return new SemesterStatistics() {  Semester = semester, NumberOfFailures = failures, NumberOfPasses= passes, WeightedAvg = average };
        }

        public void AddGrade(Grade grade)
        {
            bool isValid = IGradeLogic.ValidateGrade(grade);
            if (!isValid)
            {
                throw new ArgumentException("Invalid argument(s) provided!");
            }
            var old = gradeRepository.ReadAll().FirstOrDefault(g => g.StudentId == grade.StudentId 
            && g.SubjectId == grade.SubjectId && g.Semester == grade.Semester);
            if (old != null)
            {
                grade.GradeId = old.GradeId;
                try
                {
                    gradeRepository.Update(grade);
                }
                catch (Exception)
                {
                    throw new ArgumentException("Failed to update database");
                }
            }
            else
            {
                try
                {
                    gradeRepository.Create(grade);
                }
                catch (Exception)
                {
                    throw new ArgumentException("Failed to update database");
                }
            }
        }

        public void RemoveGrade(int id)
        {
            try
            {
                gradeRepository.Delete(id);
            }
            catch (ArgumentNullException) 
            {
                throw new ObjectNotFoundException(id, typeof(Grade));
            }
        }

        public void UpdateGrade(Grade grade)
        {
            bool isValid = IGradeLogic.ValidateGrade(grade);
            if (!isValid)
            {
                throw new ArgumentException("Invalid argument(s) provided!");
            }
            var old = gradeRepository.Read(grade.GradeId);
            if (old == null)
                throw new ObjectNotFoundException(grade.GradeId, typeof(Grade));
            try
            {
                gradeRepository.Update(grade);
            }
            catch (Exception)
            {
                throw new ArgumentException("Failed to update database");
            }
        }

        public SubjectStatistics GetSubjectStatistics(int subjectId)
        {
            var grades = gradeRepository.ReadAll().Where(grade => grade.SubjectId == subjectId);
            
            
            var result = new SubjectStatistics()
            {
                Subject = grades.FirstOrDefault() == null ? null : grades.FirstOrDefault().Subject,
                NumberOfRegistrations = grades.Count(),
                PassPerRegistrationRatio = double.IsNaN((double)grades.Count(g => g.Mark > 1) / (double)grades.Count()) ? -1 : (double)grades.Count(g => g.Mark > 1) / (double)grades.Count(),
                Avg = grades.FirstOrDefault() == null ? -1 : grades.Average(g => g.Mark)
            };
            return result;
        }
    }
}
