using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT7G72_HFT_2023241.Repository
{
    internal interface IRepository<T> where T : class
    {
        IQueryable<T> ReadAll();
        T Read(int id);
        void Create(T entity);
        void Update(T entity);
        void Delete(int id);
    }
}
