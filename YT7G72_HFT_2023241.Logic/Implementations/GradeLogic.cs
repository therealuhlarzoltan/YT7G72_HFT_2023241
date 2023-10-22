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
            throw new NotImplementedException();
        }

        public void GetGrade(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<SemesterStatistics> GetSemesterStatistics()
        {
            throw new NotImplementedException();
        }

        public SemesterStatistics GetSemesterStatistics(string semester)
        {
            throw new NotImplementedException();
        }

        public void GradeStudent(int studentId, int courseId, int mark, string semester)
        {
            throw new NotImplementedException();
        }

        public void RemoveGrade(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateGrade(Grade grade)
        {
            throw new NotImplementedException();
        }
    }
}
