using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Repository
{
    public class UniversityDatabaseContext : DbContext
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Curriculum> Curriculums { get; set; }
        public DbSet<Grade> GradeBook { get; set; }

        public UniversityDatabaseContext()
        {
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            if (!builder.IsConfigured)
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\uhlar\source\repos\YT7G72_HFT_2023241\YT7G72_HFT_2023241.Repository\Database\UniversityDatabase.mdf;Integrated Security=True;MultipleActiveResultSets=True";
                builder.UseLazyLoadingProxies().UseSqlServer(connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //creating Student entity
            builder.Entity<Student>()
                .HasAlternateKey(s => s.StudentCode);

            builder.Entity<Student>()
                .Property(s => s.FinancialStatus)
                .HasDefaultValue(FinancialStatus.WITHOUT_SCHOLARSHIP);

            //creating Teacher entity
            builder.Entity<Teacher>()
                .Property(t => t.AcademicRank)
                .HasDefaultValue(AcademicRank.TEACHERS_ASSISTANT);

            builder.Entity<Subject>();
            builder.Entity<Course>();
            builder.Entity<Curriculum>();
        }
    }
}
