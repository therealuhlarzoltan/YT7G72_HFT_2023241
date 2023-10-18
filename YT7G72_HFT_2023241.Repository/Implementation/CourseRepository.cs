using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Repository
{
    public class CourseRepository : Repository<Course>, IRepository<Course>
    {
        public CourseRepository(UniversityDatabaseContext universtiyDatabaseContext) : base(universtiyDatabaseContext) { }

        public override Course Read(int id)
        {
            return universityDatabaseContext.Courses.FirstOrDefault(c => c.Id == id);
        }

        public override void Update(Course entity)
        {
            var old = Read(entity.Id);
            CopyPropertyValues(entity, old);
            universityDatabaseContext.SaveChanges();
        }
    }
}
