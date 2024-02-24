using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Diagnostics;
using YT7G72_HFT_2023241.Logic;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Endpoint.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EducationController : Controller
    {
        private IEducationLogic educationLogic;
        private readonly IHubContext<SignalRHub> hub;

        public EducationController(IEducationLogic educationLogic, IHubContext<SignalRHub> hub)
        {
            this.educationLogic = educationLogic;
            this.hub = hub;
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
        [HttpPost]
        public void CreateSubject([FromBody] Subject subject)
        {
            educationLogic.AddSubject(subject);
            hub.Clients.All.SendAsync("SubjectCreated", subject);
        }

        [Route("Subjects")]
        [HttpPut]
        public void UpdateSubject([FromBody] Subject subject)
        {
            educationLogic.UpdateSubject(subject);
            hub.Clients.All.SendAsync("SubjectUpdated", subject);
        }

        [Route("Subjects/{id}")]
        [HttpDelete]
        public void DeleteSubject(int id)
        {
            var subject = educationLogic.GetSubject(id);
            educationLogic.RemoveSubject(id);
            hub.Clients.All.SendAsync("SubjectDeleted", subject);
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
        [HttpPost]
        public void CreateCourse([FromBody] Course course)
        {
            educationLogic.AddCourse(course);
            hub.Clients.All.SendAsync("CourseCreated", course);
        }

        [Route("Courses")]
        [HttpPut]
        public void UpdateCourse([FromBody] Course course)
        {
            educationLogic.UpdateCourse(course);
            hub.Clients.All.SendAsync("CourseUpdated", course);
        }

        [Route("Courses/{id}")]
        [HttpDelete]
        public void DeleteCourse(int id)
        {
            var course = educationLogic.GetCourse(id);
            educationLogic.RemoveCourse(id);
            hub.Clients.All.SendAsync("CourseDeleted", course);
        }

        [Route("Subjects/{subjectId}/Register/{studentId}")]
        [HttpPost]
        public void RegisterForSubject(int subjectId, int studentId)
        {
            educationLogic.RegisterStudentForSubject(studentId, subjectId, (stud, sub) => {
                hub.Clients.All.SendAsync("StudentUpdated", stud);
                hub.Clients.All.SendAsync("SubjectUpdated", sub);
            });
        }

        [Route("Subjects/{subjectId}/Register/{studentId}")]
        [HttpDelete]
        public void UnregisterFromSubject(int subjectId, int studentId)
        {
            educationLogic.RemoveStudentFromSubject(studentId, subjectId, (stud, sub) => {
                hub.Clients.All.SendAsync("StudentUpdated", stud);
                hub.Clients.All.SendAsync("SubjectUpdated", sub);
            });
        }

        [Route("Courses/{courseId}/Register/{studentId}")]
        [HttpPost]
        public void RegisterForCourse(int courseId, int studentId)
        {
            educationLogic.RegisterStudentForCourse(studentId, courseId, (s, c) => {
                hub.Clients.All.SendAsync("StudentUpdated", s);
                hub.Clients.All.SendAsync("CourseUpdated", c);
            });
        }

        [Route("Courses/{courseId}/Register/{studentId}")]
        [HttpDelete]
        public void UnregisterFromCourse(int courseId, int studentId)
        {
            educationLogic.RemoveStudentFromCourse(studentId, courseId, (s, c) => {
                hub.Clients.All.SendAsync("StudentUpdated", s);
                hub.Clients.All.SendAsync("CourseUpdated", c);
            });
        }

        [Route("Semester/Reset")]
        [HttpPost]
        public void ResetSemester()
        {
            educationLogic.ResetSemester((s, c) => {
                if (s != null) hub.Clients.All.SendAsync("SubjectUpdated", s);
                if (c != null) hub.Clients.All.SendAsync("CourseUpdated", c);
            }); ;
        }
    }
}
