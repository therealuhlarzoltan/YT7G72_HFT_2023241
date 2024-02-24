using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using YT7G72_HFT_2023241.Logic;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Endpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : Controller
    {
        private IPersonLogic personLogic;
        private readonly IHubContext<SignalRHub> hub;

        public PeopleController(IPersonLogic personLogic, IHubContext<SignalRHub> hub)
        {
            this.personLogic = personLogic;
            this.hub = hub;
        }

        [Route("Teachers")]
        [HttpGet]
        public IEnumerable<Teacher> GetAllTeachers()
        {
            return personLogic.GetTeachers();
        }

        [Route("Teachers/{id}")]
        [HttpGet]
        public Teacher GetTeacher(int id)
        {
            return personLogic.GetTeacher(id);
        }

        [Route("Teachers")]
        [HttpPost]
        public void AddTeacher([FromBody] Teacher teacher)
        {
            personLogic.AddTeacher(teacher);
            hub.Clients.All.SendAsync("TeacherCreated", teacher);
        }

        [Route("Teachers")]
        [HttpPut]
        public void UpdateTeacher([FromBody] Teacher teacher)
        {
            personLogic.UpdateTeacher(teacher);
            hub.Clients.All.SendAsync("TeacherUpdated", teacher);
        }

        [Route("Teachers/{id}")]
        [HttpDelete]
        public void Delete(int id)
        {
            var teacher = personLogic.GetTeacher(id);
            personLogic.RemoveTeacher(id);
            hub.Clients.All.SendAsync("TeacherDeleted", teacher);
        }

        [Route("Students")]
        [HttpGet]
        public IEnumerable<Student> GetAllStudents()
        {
            return personLogic.GetStudents();
        }

        [Route("Students/{id}")]
        [HttpGet]
        public Student GetStudent(int id)
        {
            return personLogic.GetStudent(id);
        }

        [Route("Students")]
        [HttpPost]
        public void AddStudent([FromBody] Student student)
        {
            personLogic.AddStudent(student);
            hub.Clients.All.SendAsync("StudentCreated", student);
        }

        [Route("Students")]
        [HttpPut]
        public void UpdateStudent([FromBody] Student student)
        {
            personLogic.UpdateStudent(student);
            hub.Clients.All.SendAsync("StudentUpdated", student);
        }

        [Route("Students/{id}")]
        [HttpDelete]
        public void DeleteStudent(int id)
        {
            var student = personLogic.GetStudent(id);
            personLogic.RemoveStudent(id);
            hub.Clients.All.SendAsync("StudentDeleted", student);
        }

        [Route("Students/Best")]
        [HttpGet]
        public IEnumerable<AverageByPersonDTO<Student>> GetBestStudents()
        {
            var result = personLogic.GetBestStudents();
            return result.Select(r => new AverageByPersonDTO<Student>(r.Item1, r.Item2));
        }

        [Route("Teachers/Best")]
        [HttpGet]
        public IEnumerable<AverageByPersonDTO<Teacher>> GetBestTeachers()
        {
            var result = personLogic.GetBestTeachers();
            return result.Select(r => new AverageByPersonDTO<Teacher>(r.Item1, r.Item2));
        }

        [Route("Teachers/Best/{academicRank}")]
        [HttpGet]
        public IEnumerable<AverageByPersonDTO<Teacher>> GetBestTeachersByAcademicRank(int academicRank)
        {
            var result = personLogic.GetBestTeachersByAcademicRank((AcademicRank)academicRank);
            return result.Select(r => new AverageByPersonDTO<Teacher>(r.Item1, r.Item2));
        }

        [Route("Students/Schedule/{id}")]
        [HttpGet]
        public string GetStudentSchedule(int id)
        {
            return personLogic.GetSchedule<Student>(id);
        }

        [Route("Teachers/Schedule/{id}")]
        [HttpGet]
        public string GetTeacherSchedule(int id)
        {
            return personLogic.GetSchedule<Teacher>(id);
        }

    }
}
