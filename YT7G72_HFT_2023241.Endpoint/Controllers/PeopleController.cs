using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections;
using System.Collections.Generic;
using YT7G72_HFT_2023241.Logic;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Endpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PeopleController : Controller
    {
        private IPersonLogic personLogic;

        public PeopleController(IPersonLogic personLogic)
        {
            this.personLogic = personLogic;
        }

        [Route("/Teachers")]
        [HttpGet]
        public IEnumerable<Teacher> GetAllTeachers()
        {
            return personLogic.GetTeachers();
        }

        [Route("/Teachers")]
        [HttpGet("{id}")]
        public Teacher GetTeacher(int id)
        {
            return personLogic.GetTeacher(id);
        }

        [Route("/Teachers")]
        [HttpPost]
        public void AddTeacher([FromBody] Teacher teacher)
        {
            personLogic.AddTeacher(teacher);
        }

        [Route("/Teachers")]
        [HttpPut]
        public void UpdateTeacher([FromBody] Teacher teacher)
        {
            personLogic.UpdateTeacher(teacher);
        }

        [Route("/Teachers")]
        [HttpDelete("{id}")]
        public void DeleteTeacher(int id)
        {
            personLogic.RemoveTeacher(id);
        }

        [Route("/Students")]
        [HttpGet]
        public IEnumerable<Student> GetAllStudents()
        {
            return personLogic.GetStudents();
        }

        [Route("/Students")]
        [HttpGet("{id}")]
        public Student GetStudent(int id)
        {
            return personLogic.GetStudent(id);
        }

        [Route("/Students")]
        [HttpPost]
        public void AddStudent([FromBody] Student student)
        {
            personLogic.AddStudent(student);
        }

        [Route("/Students")]
        [HttpPut]
        public void UpdateStudent([FromBody] Student student)
        {
            personLogic.UpdateStudent(student);
        }

        [Route("/Students")]
        [HttpDelete("{id}")]
        public void DeleteStudent(int id)
        {
            personLogic.RemoveStudent(id);
        }



    }
}
