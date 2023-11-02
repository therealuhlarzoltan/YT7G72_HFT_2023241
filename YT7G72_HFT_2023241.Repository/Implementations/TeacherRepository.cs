using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Repository
{
    public class TeacherRepository : Repository<Teacher>, IRepository<Teacher>
    {
        public TeacherRepository(UniversityDatabaseContext universityDatabaseContext) : base(universityDatabaseContext) { }

        public override Teacher Read(int id)
        {
            return universityDatabaseContext.Teachers.FirstOrDefault(t => t.TeacherId == id);
        }

        public override void Update(Teacher entity)
        {
            var old = Read(entity.TeacherId);
            CopyPropertyValues(entity, old);
            universityDatabaseContext.SaveChanges();

        }
    }
}
