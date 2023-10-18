using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Repository
{
    internal class SubjectRepository : Repository<Subject>, IRepository<Subject>
    {
        public SubjectRepository(UniversityDatabaseContext universityDatabaseContext) : base(universityDatabaseContext) { }

        public override Subject Read(int id)
        {
            return universityDatabaseContext.Subjects.FirstOrDefault(s => s.Id == id);
        }

        public override void Update(Subject entity)
        {
            var old = Read(entity.Id);
            CopyPropertyValues(entity, old);
            universityDatabaseContext.SaveChanges();
        }
    }
}
