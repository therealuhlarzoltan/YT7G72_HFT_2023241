﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Repository
{
    public class SubjectRepository : Repository<Subject>, IRepository<Subject>
    {
        public SubjectRepository(UniversityDatabaseContext universityDatabaseContext) : base(universityDatabaseContext) { }

        public override Subject Read(int id)
        {
            return universityDatabaseContext.Subjects.FirstOrDefault(s => s.SubjectId == id);
        }

        public override void Update(Subject entity)
        {
            var old = Read(entity.SubjectId);
            CopyPropertyValues(entity, old);
            universityDatabaseContext.SaveChanges();
        }
    }
}
