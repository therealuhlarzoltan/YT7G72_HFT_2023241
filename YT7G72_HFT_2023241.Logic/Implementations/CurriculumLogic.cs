using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Logic.Interfaces;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.Repository;

namespace YT7G72_HFT_2023241.Logic.Implementations
{
    public class CurriculumLogic : ICurriculumLogic
    {
        private IRepository<Curriculum> curriculumRepository;

        public CurriculumLogic(IRepository<Curriculum> curriculumRepository)
        {
            this.curriculumRepository = curriculumRepository;
        }

        public void AddCurriculum(Curriculum curriculum)
        {
            curriculumRepository.Create(curriculum);
        }

        public Curriculum GetCurriculum(int id)
        {
            var curriculum = curriculumRepository.Read(id);
            if (curriculum == null)
            {
                throw new ObjectNotFoundException(id, typeof(Curriculum));
            }
            return curriculum;
        }

        public IEnumerable<Curriculum> GetCurriculums()
        {
            return curriculumRepository.ReadAll();
        }

        public void RemoveCurriculum(int id)
        {
            try
            {
                curriculumRepository.Delete(id);
            }
            catch (ArgumentNullException)
            {
                throw new ObjectNotFoundException(id, typeof(Curriculum));
            }
        }

        public void UpdateCurriculum(Curriculum curriculum)
        {
            var old = curriculumRepository.Read(curriculum.CurriculumId);
            if (old == null)
                throw new ObjectNotFoundException(curriculum.CurriculumId, typeof(Curriculum));
            curriculumRepository.Update(curriculum);
        }
    }
}
