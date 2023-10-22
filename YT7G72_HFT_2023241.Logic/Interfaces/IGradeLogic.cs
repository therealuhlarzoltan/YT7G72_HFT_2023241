using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Logic
{
    public interface IGradeLogic
    {
        //grade CRUD logic
        void GradeStudent(int studentId, int courseId, int mark, string semester);
        void RemoveGrade(int id);
        void UpdateGrade(Grade grade);
        void GetGrade(int id);
        IEnumerable<Grade> GetAllGrades();
        //grade non-CRUD logic
        IEnumerable<SemesterStatistics> GetSemesterStatistics();
        SemesterStatistics GetSemesterStatistics(string semester);

    }
}
