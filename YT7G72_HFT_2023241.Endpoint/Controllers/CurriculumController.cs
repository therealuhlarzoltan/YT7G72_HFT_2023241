using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using YT7G72_HFT_2023241.Logic.Interfaces;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Endpoint.Controllers
{
    [ApiController]
    [Route("/Curriculums")]
    public class CurriculumController : Controller
    {
        private ICurriculumLogic curriculumLogic;
        private readonly IHubContext<SignalRHub> hub;

        public CurriculumController(ICurriculumLogic curriculumLogic, IHubContext<SignalRHub> hub)
        {
            this.curriculumLogic = curriculumLogic;
            this.hub = hub;
        }

        [HttpGet("{id}")]
        public Curriculum Get(int id)
        {
            return curriculumLogic.GetCurriculum(id);
        }

        [HttpGet]
        public IEnumerable<Curriculum> GetAll()
        {
            return curriculumLogic.GetCurriculums();
        }

        [HttpPost]
        public void Create([FromBody] Curriculum curriculum)
        {
            curriculumLogic.AddCurriculum(curriculum);
            hub.Clients.All.SendAsync("CurriculumCreated", curriculum);
        }

        [HttpPut]
        public void Edit([FromBody] Curriculum curriculum)
        {
            curriculumLogic.UpdateCurriculum(curriculum);
            hub.Clients.All.SendAsync("CurriculumUpdated", curriculum);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var curriculum = curriculumLogic.GetCurriculum(id);
            curriculumLogic.RemoveCurriculum(id);
            hub.Clients.All.SendAsync("CurriculumDeleted", curriculum);
        }
    }
}
