using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using YT7G72_HFT_2023241.Logic;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Endpoint.Controllers
{
    [ApiController]
    [Route("/Grades")]
    public class GradeController : Controller
    {
        private IGradeLogic gradeLogic;
        private readonly IHubContext<SignalRHub> hub;


        public GradeController(IGradeLogic gradeLogic, IHubContext<SignalRHub> hub)
        {
            this.gradeLogic = gradeLogic;
            this.hub = hub;
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
            var newGrade = gradeLogic.GetAllGrades().OrderByDescending(g => g.GradeId).First();
            hub.Clients.All.SendAsync("GradeCreated", newGrade);
        }

        [HttpPut]
        public void Edit([FromBody] Grade grade)
        {
            gradeLogic.UpdateGrade(grade);
            var newGrade = gradeLogic.GetGrade(grade.GradeId);
            hub.Clients.All.SendAsync("GradeUpdated", newGrade);
        }


        [HttpDelete("{id}")]
        public void Delete(int id)
        {
           var grade = gradeLogic.GetGrade(id);
           gradeLogic.RemoveGrade(id);
            hub.Clients.All.SendAsync("GradeDeleted", grade);
        }

        [Route("Semester/Statistics/{year}")]
        [HttpGet]
        public SemesterStatistics GetSemesterStatistics(string year)
        {
            year = year.Replace("-", "/");
            return gradeLogic.GetSemesterStatistics(year);
        }

        [Route("Semester/Statistics")]
        [HttpGet]
        public IEnumerable<SemesterStatistics> GetAllSemesterStatistics()
        {
            return gradeLogic.GetSemesterStatistics();
        }

        [Route("Subjects/Statistics/{id}")]
        [HttpGet]
        public SubjectStatistics GetSubjectStatistics(int id)
        {
            return gradeLogic.GetSubjectStatistics(id);
        }
    }
}
