using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT7G72_HFT_2023241.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class
    {
        protected UniversityDatabaseContext universityDatabaseContext;

        protected Repository(UniversityDatabaseContext universityDatabaseContext)
        {
            this.universityDatabaseContext = universityDatabaseContext;
        }

        public void Create(T entity)
        {
            universityDatabaseContext.Set<T>().Add(entity);
            universityDatabaseContext.SaveChanges();
        }

        public void Delete(int id)
        {
            universityDatabaseContext.Set<T>().Remove(Read(id));
            universityDatabaseContext.SaveChanges();
        }

        public IQueryable<T> ReadAll()
        {
            return universityDatabaseContext.Set<T>();
        }

        public abstract T Read(int id);
        public abstract void Update(T entity);

        protected static void CopyPropertyValues(T source, T target)
        {
            var type = typeof(T);
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                property.SetValue(target, property.GetValue(source));
            }
        }
    }
}
