using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using YT7G72_HFT_2023241.Logic;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Endpoint.Controllers
{
    [ApiController]
    [Route("/Grades")]
    public class GradeController : Controller
    {
        private IGradeLogic gradeLogic;
        
        public GradeController(IGradeLogic gradeLogic)
        {
            this.gradeLogic = gradeLogic;
        }

        [HttpGet("{id}")]
        public Grade Get(int id)
        {
            return gradeLogic.GetGrade(id);
        }

        [HttpGet]
        public IEnumerable<Grade> GetAll()
        {
            return gradeLogic.GetAllGrades();
        }

        [HttpPost]
        public void Create([FromBody] Grade grade)
        {
            gradeLogic.AddGrade(grade);
        }

        [HttpPut]
        public void Edit([FromBody] Grade grade)
        {
            gradeLogic.UpdateGrade(grade);
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
           gradeLogic.RemoveGrade(id);
        }

        [Route("/Semester/Statistics/{year}")]
        [HttpGet]
        public SemesterStatistics GetSemesterStatistics(string year)
        {
            return gradeLogic.GetSemesterStatistics(year);
        }

        [Route("/Semester/Statistics")]
        [HttpGet]
        public IEnumerable<SemesterStatistics> GetAllSemesterStatistics()
        {
            return gradeLogic.GetSemesterStatistics();
        }

        [Route("/Subjects/Statistics/{id}")]
        [HttpGet]
        public SubjectStatistics GetSubjectStatistics(int id)
        {
            return gradeLogic.GetSubjectStatistics(id);
        }
    }
}
