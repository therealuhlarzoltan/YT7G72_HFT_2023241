using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using YT7G72_HFT_2023241.Logic;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Endpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EducationController : Controller
    {
        private IEducationLogic educationLogic;

        public EducationController(IEducationLogic educationLogic)
        {
            this.educationLogic = educationLogic;
        }

        [Route("Subjects")]
        [HttpGet]
        public IEnumerable<Subject> GetAllSubjects()
        {
            return educationLogic.GetAllSubjects();
        }

        [Route("Subjects/{id}")]
        [HttpGet]
        public Subject GetSubject(int id)
        {
            return educationLogic.GetSubject(id);
        }

        [Route("Subjects")]
        [HttpPut]
        public void UpdateSubject([FromBody] Subject subject)
        {
            educationLogic.UpdateSubject(subject);
        }

        [Route("Subjects/{id}")]
        [HttpDelete]
        public void DeleteSubject(int id)
        {
            educationLogic.RemoveSubject(id);
        }

        [Route("Courses")]
        [HttpGet]
        public IEnumerable<Course> GetAllCourses()
        {
            return educationLogic.GetAllCourses();
        }

        [Route("Courses/{id}")]
        [HttpGet]
        public Course GetCourse(int id)
        {
            return educationLogic.GetCourse(id);
        }

        [Route("Courses")]
        [HttpPut]
        public void UpdateCourse([FromBody] Course course)
        {
            educationLogic.UpdateCourse(course);
        }

        [Route("Courses/{id}")]
        [HttpDelete]
        public void DeleteCourse(int id)
        {
            educationLogic.RemoveCourse(id);
        }
    }
}
