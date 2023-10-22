using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Repository
{
    public class CurriculumRepository : Repository<Curriculum>, IRepository<Curriculum>
    {
        public CurriculumRepository(UniversityDatabaseContext universityDatabaseContext) : base(universityDatabaseContext) { }

        public override Curriculum Read(int id)
        {
            return universityDatabaseContext.Curriculums.FirstOrDefault(c => c.CurriculumId == id);
        }

        public override void Update(Curriculum entity)
        {
            var old = Read(entity.CurriculumId);
            CopyPropertyValues(entity, old);
            universityDatabaseContext.SaveChanges();
        }
    }
}
