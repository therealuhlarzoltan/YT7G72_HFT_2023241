using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            var relevantGrades = gradeRepository.ReadAll().Where(grade => grade.Semester ==  semester);
            if (!relevantGrades.Any())
            {
                return new SemesterStatistics() { Semester = semester, NumberOfFailures = 0, NumberOfPasses = 0, WeightedAvg = double.NaN };
            }
            int failures = relevantGrades.Count(grade => grade.Mark == 1);
            int passes = relevantGrades.Count(grade => grade.Mark > 1);
            double average = (double)relevantGrades.Sum(grade => grade.Mark * grade.Subject.Credits) / (double)relevantGrades.Sum(grade => grade.Subject.Credits);
            return new SemesterStatistics() {  Semester = semester, NumberOfFailures = failures, NumberOfPasses= passes, WeightedAvg = average };
        }

        public void AddGrade(Grade grade)
        {
            var old = gradeRepository.ReadAll().FirstOrDefault(g => g.StudentId == grade.StudentId 
            && g.SubjectId == grade.SubjectId && g.Semester == grade.Semester);
            if (old != null)
            {
                grade.GradeId = old.GradeId;
                gradeRepository.Update(grade);
            }
            else
            {
                gradeRepository.Create(grade);
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
            var old = gradeRepository.Read(grade.GradeId);
            if (old == null)
                throw new ObjectNotFoundException(grade.GradeId, typeof(Grade));
            gradeRepository.Update(grade);
        }

        public SubjectStatistics GetSubjectStatistics(int subjectId)
        {
            var grades = gradeRepository.ReadAll().Where(grade => grade.SubjectId == subjectId);
            try
            {
                var result = new SubjectStatistics()
                {
                    Subject = grades.FirstOrDefault()?.Subject,
                    NumberOfRegistrations = grades.Count(),
                    PassPerRegistrationRatio = (double)grades.Count(g => g.Mark > 1) / (double)(grades.Count()),
                    Avg = grades.Average(g => g.Mark)
                };
                return result;
            }
            catch (DivideByZeroException)
            {
                return new SubjectStatistics()
                {
                    Subject = grades.FirstOrDefault()?.Subject,
                    NumberOfRegistrations = grades.Count(),
                    PassPerRegistrationRatio = double.NaN,
                    Avg = double.NaN
                };
            }
        }
    }
}
