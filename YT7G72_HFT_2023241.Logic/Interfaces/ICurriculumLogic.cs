using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Logic.Interfaces
{
    public interface ICurriculumLogic
    {
        //CRUD methods
        IEnumerable<Curriculum> GetCurriculums();
        Curriculum GetCurriculum(int id);
        void RemoveCurriculum(int id);
        void AddCurriculum(Curriculum curriculum);
        void UpdateCurriculum(Curriculum curriculum);
        
    }
}
