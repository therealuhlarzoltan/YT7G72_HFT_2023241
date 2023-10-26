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
        public DbSet<SubjectRegistration> SubjectRegistrations { get; set; }

        public UniversityDatabaseContext()
        {
            this.Database.EnsureDeleted();
            this.Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder builder)
        {
            builder.EnableSensitiveDataLogging();
            if (!builder.IsConfigured)
            {
                string connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB; AttachDbFilename = C:\Users\uhlar\source\repos\YT7G72_HFT_2023241\YT7G72_HFT_2023241.Repository\Database\UniversityDatabase.mdf;Integrated Security=True;Initial Catalog=UniversityDatabase;MultipleActiveResultSets=True";
                builder
                    .UseLazyLoadingProxies()
                    .UseSqlServer(connectionString);
                    //.UseInMemoryDatabase("UniversityDatabase");
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            //creating Student entity
            builder.Entity<Student>()
                .HasAlternateKey(s => s.StudentCode);

            builder.Entity<Student>()
                .Property(s => s.FinancialStatus)
                .HasDefaultValue(FinancialStatus.Without_Scholarship);

            builder.Entity<Student>()
                .HasMany(e => e.RegisteredSubjects)
                .WithMany(e => e.RegisteredStudents)
                .UsingEntity<SubjectRegistration>(
                    l => l.HasOne(e => e.Subject).WithMany(e => e.SubjectRegistrations).OnDelete(DeleteBehavior.NoAction),
                    r => r.HasOne(e => e.Student).WithMany(e => e.SubjectRegistrations).OnDelete(DeleteBehavior.NoAction)
                );
        

            builder.Entity<Student>()
                .HasMany(s => s.RegisteredCourses)
                .WithMany(c => c.EnrolledStudents)
                .UsingEntity<CourseRegistration>(
                    //courseReg => courseReg.HasOne(courseReg => courseReg.Course).WithMany().HasForeignKey(courseReg => courseReg.CourseId).OnDelete(DeleteBehavior.Cascade),
                    //courseReg => courseReg.HasOne(courseReg => courseReg.Student).WithMany().HasForeignKey(courseReg => courseReg.StudentId).OnDelete(DeleteBehavior.Cascade)
                    l => l.HasOne(e => e.Course).WithMany(e => e.CourseRegistrations).OnDelete(DeleteBehavior.NoAction),
                    r => r.HasOne(e => e.Student).WithMany(e => e.CourseRegistrations).OnDelete(DeleteBehavior.NoAction)
                );


            //creating Teacher entity
            builder.Entity<Teacher>()
                .Property(t => t.AcademicRank)
                .HasDefaultValue(AcademicRank.Teachers_Assistant);

            builder.Entity<Teacher>()
                .HasMany(t => t.RegisteredCourses)
                .WithOne(c => c.Teacher)
                .HasForeignKey(t => t.TeacherId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<Teacher>()
                .HasMany(t => t.RegisteredSubjects)
                .WithOne(s => s.Teacher)
                .HasForeignKey(t => t.TeacherId)
                .IsRequired();


            //creating Subject entity
            builder.Entity<Subject>()
                .HasMany(s => s.SubjectCourses)
                .WithOne(c => c.Subject)
                .HasForeignKey(c => c.SubjectId);

            builder.Entity<Subject>()
                .HasOne(s => s.PreRequirement)
                .WithOne()
                .HasForeignKey<Subject>(s => s.PreRequirementId)
                .IsRequired(false);


            //creating Grade entity
            builder.Entity<Grade>()
                .HasOne(grade => grade.Student)
                .WithMany(student => student.Grades)
                .HasForeignKey(student => student.StudentId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<Grade>()
                .HasOne(grade => grade.Subject)
                .WithMany(subject => subject.Grades)
                .HasForeignKey(grade => grade.SubjectId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            builder.Entity<Grade>()
                .HasOne(grade => grade.Teacher)
                .WithMany(teacher => teacher.GivenGrades)
                .HasForeignKey(grade => grade.TeacherId)
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            
            //creating Curriculum entity
            builder.Entity<Curriculum>()
                .HasMany(c => c.CurriculumSubjects)
                .WithOne(s => s.Curriculum)
                .HasForeignKey(s => s.CurriculumId)
                .IsRequired();

            builder.Entity<Curriculum>()
                .HasMany(c => c.CurriculumStudents)
                .WithOne(s => s.Curriculum)
                .HasForeignKey(s => s.CurriculumId)
                .OnDelete(DeleteBehavior.NoAction);

            //prepopulating database
            builder.Entity<Curriculum>()
                .HasData(new Curriculum[] {
                    new Curriculum {CurriculumId = 1, CurriculumName = "Computer Engineering BSc E3", 
                        CurriculumCode = "OENIKCEBE3"},
                    new Curriculum {CurriculumId = 2, CurriculumName = "Computer Engineering MSc F", 
                        CurriculumCode = "OENIKCEMF"}
                });

            builder.Entity<Student>()
                .HasData(new Student[] { 
                    //NIK BSc Student With Scholarhsip
                    new Student {StudentId = 1, FirstName = "Zoltán", LastName = "Uhlár", StudentCode = "YT7G72", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 2, FirstName = "Gergely", LastName = "Lucza", StudentCode = "BZ6QL7", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 3, FirstName = "Roland", LastName = "Kertész", StudentCode = "COH045", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 4, FirstName = "Roland", LastName = "Varga", StudentCode = "S8HE52", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 5, FirstName = "Bálint", LastName = "Nagy", StudentCode = "JKL123", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 6, FirstName = "István", LastName = "Kovács", StudentCode = "QWE456", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 7, FirstName = "Anna", LastName = "Tóth", StudentCode = "ASD789", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 8, FirstName = "Eszter", LastName = "Szabó", StudentCode = "XYZ012", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 9, FirstName = "Márta", LastName = "Papp", StudentCode = "UIO345", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 10, FirstName = "Katalin", LastName = "Molnár", StudentCode = "MNB678", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 11, FirstName = "Ferenc", LastName = "Kis", StudentCode = "QAZ741", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 12, FirstName = "Péter", LastName = "Németh", StudentCode = "WSX852", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 13, FirstName = "László", LastName = "Balogh", StudentCode = "EDC963", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 14, FirstName = "Krisztina", LastName = "Takács", StudentCode = "RFV074", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 15, FirstName = "János", LastName = "Horváth", StudentCode = "TGB185", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 16, FirstName = "Zsuzsanna", LastName = "Farkas", StudentCode = "YHN296", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 17, FirstName = "Gábor", LastName = "Pataki", StudentCode = "UJM307", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 18, FirstName = "Beáta", LastName = "Mészáros", StudentCode = "IKL418", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 19, FirstName = "Tamás", LastName = "Kiss", StudentCode = "OLP529", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 20, FirstName = "Erika", LastName = "Gulyás", StudentCode = "PLK630", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    //NIK BSc Students Without Scholarship
                     new Student {StudentId = 31, FirstName = "Gábor", LastName = "Kovács", StudentCode = "QWE678", CurriculumId = 1},
                     new Student {StudentId = 32, FirstName = "Ágnes", LastName = "Németh", StudentCode = "WSX901", CurriculumId = 1},
                     new Student {StudentId = 33, FirstName = "Orsolya", LastName = "Tóth", StudentCode = "ASD234", CurriculumId = 1},
                     new Student {StudentId = 34, FirstName = "Márton", LastName = "Nagy", StudentCode = "JKL567", CurriculumId = 1},
                     new Student {StudentId = 35, FirstName = "György", LastName = "Balogh", StudentCode = "RFV890", CurriculumId = 1},
                     new Student {StudentId = 36, FirstName = "Hanna", LastName = "Varga", StudentCode = "S8HE12", CurriculumId = 1},
                     new Student {StudentId = 37, FirstName = "Krisztina", LastName = "Kiss", StudentCode = "OLP456", CurriculumId = 1},
                     new Student {StudentId = 38, FirstName = "Mátyás", LastName = "Papp", StudentCode = "UIO789", CurriculumId = 1},
                     new Student {StudentId = 39, FirstName = "Anna", LastName = "Kertész", StudentCode = "COH012", CurriculumId = 1},
                     new Student {StudentId = 40, FirstName = "Zoltán", LastName = "Farkas", StudentCode = "YHN345", CurriculumId = 1},
                    //NIK MSc Students With Scholarship
                    new Student {StudentId = 41, FirstName = "Béla", LastName = "Kovács", StudentCode = "QRE678", CurriculumId = 2, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 42, FirstName = "Klára", LastName = "Tóth", StudentCode = "ASD901", CurriculumId = 2, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 43, FirstName = "Péter", LastName = "Nagy", StudentCode = "JKL234", CurriculumId = 2,FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 44, FirstName = "László", LastName = "Molnár", StudentCode = "MKB567", CurriculumId = 2, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 45, FirstName = "Györgyi", LastName = "Balogh", StudentCode = "RFV963", CurriculumId = 2, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 46, FirstName = "András", LastName = "Papp", StudentCode = "UIO123", CurriculumId = 2, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 47, FirstName = "Viktória", LastName = "Kiss", StudentCode = "OLO456", CurriculumId = 2, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 48, FirstName = "Ferenc", LastName = "Takács", StudentCode = "TKB789", CurriculumId = 2, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 49, FirstName = "Katalin", LastName = "Farkas", StudentCode = "YHN017", CurriculumId = 2, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    new Student {StudentId = 50, FirstName = "Bence", LastName = "Kertész", StudentCode = "COG345", CurriculumId = 2, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    //NIK MSc Students Without Scholarship
                    new Student {StudentId = 51, FirstName = "Béla", LastName = "Erdei", StudentCode = "QREO78", CurriculumId = 2},
                    new Student {StudentId = 52, FirstName = "Klára", LastName = "Veres", StudentCode = "AFD901", CurriculumId = 2},
                    new Student {StudentId = 53, FirstName = "Péter", LastName = "Mátrai", StudentCode = "JKL944", CurriculumId = 2},
                    new Student {StudentId = 54, FirstName = "András", LastName = "Barabás", StudentCode = "MNB667", CurriculumId = 2},
                    new Student {StudentId = 55, FirstName = "Györgyi", LastName = "Lendvai", StudentCode = "RTVF90", CurriculumId = 2},
                    new Student {StudentId = 56, FirstName = "András", LastName = "Papp", StudentCode = "UINQ23", CurriculumId = 2},
                    new Student {StudentId = 57, FirstName = "Viktória", LastName = "Zsigmond", StudentCode = "OLOU66", CurriculumId = 2},
                    new Student {StudentId = 58, FirstName = "Ferenc", LastName = "Suszter", StudentCode = "TKMC79", CurriculumId = 2},
                    new Student {StudentId = 59, FirstName = "Kornélia", LastName = "Szántó", StudentCode = "YHNR17", CurriculumId = 2},
                    new Student {StudentId = 60, FirstName = "Ivett", LastName = "Havasi", StudentCode = "COJ395", CurriculumId = 2}
                });

            builder.Entity<Teacher>().HasData(
                    new Teacher[]
                    {
                        new Teacher() { TeacherId = 1, FirstName = "Gabriella", LastName = "Nagy", AcademicRank = AcademicRank.Senior_Lecturer},
                        new Teacher() { TeacherId = 2, FirstName = "Ákos", LastName = "Hajnal", AcademicRank = AcademicRank.Senior_Lecturer},
                        new Teacher() { TeacherId = 3, FirstName = "Gábor", LastName = "Kovács", AcademicRank = AcademicRank.Teachers_Assistant},
                        new Teacher() { TeacherId = 4, FirstName = "Péter", LastName = "Balogh", AcademicRank = AcademicRank.Technical_Assistant},
                        new Teacher() { TeacherId = 5, FirstName = "Miklós", LastName = "Sipos", AcademicRank = AcademicRank.Technical_Assistant},
                        new Teacher() { TeacherId = 6, FirstName = "Sándor", LastName = "Szénási", AcademicRank = AcademicRank.Professor},
                        new Teacher() { TeacherId = 7, FirstName = "Zoltán", LastName = "Király", AcademicRank = AcademicRank.Associate_Professor},
                        new Teacher() { TeacherId = 8, FirstName = "Balázs", LastName = "Gáspár", AcademicRank = AcademicRank.Teachers_Assistant},
                        new Teacher() { TeacherId = 9, FirstName = "Balázs", LastName = "Tusor", AcademicRank = AcademicRank.Senior_Lecturer},
                        new Teacher() { TeacherId = 10, FirstName = "Attila", LastName = "Molnár", AcademicRank = AcademicRank.Technical_Assistant},
                        new Teacher() { TeacherId = 11, FirstName = "Szabolcs", LastName = "Sergyán", AcademicRank = AcademicRank.Associate_Professor},
                        new Teacher() { TeacherId = 12, FirstName = "Zoltán", LastName = "Vámossy", AcademicRank = AcademicRank.Professor},
                        new Teacher() { TeacherId = 13, FirstName = "Kata", LastName = "Egres", AcademicRank = AcademicRank.Teachers_Assistant}

                    }
                );

            builder.Entity<Subject>().HasData(
                    new object[] {
                        new Subject() { SubjectId = 3, SubjectName = "Software Design and Development I", SubjectCode = "NIKCESZTF1", Credits = 6, CurriculumId = 1, TeacherId = 11, Requirement =  Requirement.WRITTEN_AND_SPOKEN_EXAM },
                        new Subject() { SubjectId = 2, SubjectName = "Software Design and Development II", SubjectCode = "NIKCESZTF2", Credits = 6,  CurriculumId = 1, TeacherId = 6, Requirement = Requirement.WRITTEN_AND_SPOKEN_EXAM, PreRequirementId = 3},
                        new Subject() { SubjectId = 1, SubjectName = "Web development using Java and Spring Boot", SubjectCode = "NIKCEJAVAWEB", Credits = 4, CurriculumId = 1, TeacherId = 1, Requirement = Requirement.SPOKEN_EXAM, PreRequirementId = 2 },         
                        new Subject() { SubjectId = 4, SubjectName = "The Java Microservice Project", SubjectCode = "NICKEJAVAMSP", Credits = 3,  CurriculumId = 1, TeacherId = 1, Requirement = Requirement.MID_TERM_MARK, PreRequirementId = 2}
                        
                    }
                );

            builder.Entity<Course>().HasData(
                    new object[]
                    {
                        //JavaWeb data
                        new Course() { CourseId = 1, CourseName = "JavaWeb EA", CourseCapacity = 350,  CourseType = CourseType.Lecture, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(16, 15, 0), LengthInMinutes = 90, TeacherId = 1, SubjectId = 1, Room = "BA.1.32.AudMax"},
                        new Course() { CourseId = 2, CourseName = "JavaWeb Lab01", CourseCapacity = 15,  CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(12, 35, 0), LengthInMinutes = 90, TeacherId = 1, SubjectId = 1, Room = "BA.1.10"},
                        new Course() { CourseId = 3, CourseName = "JavaWeb Lab02", CourseCapacity = 15,  CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(14, 25, 0), LengthInMinutes = 90, TeacherId = 1, SubjectId = 1, Room = "BA.1.10"},
                        new Course() { CourseId = 4, CourseName = "JavaWeb Lab03", CourseCapacity = 15,  CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(12, 35, 0), LengthInMinutes = 90, TeacherId = 3, SubjectId = 1, Room = "BA.1.13"},
                        new Course() { CourseId = 5, CourseName = "JavaWeb Lab04", CourseCapacity = 15,  CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(14, 25, 0), LengthInMinutes = 90, TeacherId = 3, SubjectId = 1, Room = "BA.1.13"},
                        new Course() { CourseId = 6, CourseName = "JavaWeb Lab05", CourseCapacity = 15,  CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Wednesday, StartTime = new TimeSpan(12, 35, 0), LengthInMinutes = 90, TeacherId = 2, SubjectId = 1, Room = "BA.1.11"},
                        new Course() { CourseId = 7, CourseName = "JavaWeb Lab06", CourseCapacity = 15,  CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Wednesday, StartTime = new TimeSpan(14, 25, 0), LengthInMinutes = 90, TeacherId = 2, SubjectId = 1, Room = "BA.1.11"},
                        new Course() { CourseId = 8, CourseName = "JavaWeb Lab07", CourseCapacity = 15,  CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Wednesday, StartTime = new TimeSpan(16, 15, 0), LengthInMinutes = 90, TeacherId = 2, SubjectId = 1, Room = "BA.1.11"},

                        //Software Design and Development II data
                        new Course() { CourseId = 9, CourseName = "Sztf2 EA", CourseCapacity = 350, CourseType = CourseType.Lecture, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(8, 0, 0), LengthInMinutes = 150, TeacherId = 6, SubjectId = 2, Room = "BA.1.32.AudMax"},
                        new Course() { CourseId = 10, CourseName = "Sztf2 Lab01", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(8, 0, 0), LengthInMinutes = 150, TeacherId = 7, SubjectId = 2, Room = "BA.1.13"},
                        new Course() { CourseId = 11, CourseName = "Sztf2 Lab02", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(12, 35, 0), LengthInMinutes = 150, TeacherId = 8, SubjectId = 2, Room = "BA.1.15"},
                        new Course() { CourseId = 12, CourseName = "Sztf2 Lab03", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(14, 25, 0), LengthInMinutes = 150, TeacherId = 9, SubjectId = 2, Room = "BA.1.14"},
                        new Course() { CourseId = 13, CourseName = "Sztf2 Lab04", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(15, 20, 0), LengthInMinutes = 150, TeacherId = 8, SubjectId = 2, Room = "BA.1.15"},
                        new Course() { CourseId = 14, CourseName = "Sztf2 Lab05", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeSpan(14, 25, 0), LengthInMinutes = 150, TeacherId = 9, SubjectId = 2, Room = "BA.1.13"},
                        new Course() { CourseId = 15, CourseName = "Sztf2 Lab06", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeSpan(17, 05, 0), LengthInMinutes = 150, TeacherId = 9, SubjectId = 2, Room = "BA.1.13"},
                        new Course() { CourseId = 16, CourseName = "Sztf2 Lab07", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(8, 0, 0), LengthInMinutes = 150, TeacherId = 7, SubjectId = 2, Room = "BA.1.13"},

                        //Software Design and Development I data
                        new Course() { CourseId = 17, CourseName = "Sztf1 EA", CourseCapacity = 350, CourseType = CourseType.Lecture, DayOfWeek= DayOfWeek.Monday, StartTime = new TimeSpan(10, 45, 0), LengthInMinutes = 150, TeacherId = 12, SubjectId = 3, Room = "BA.1.32.AudMax"},
                        new Course() { CourseId = 18, CourseName = "Sztf1 Lab01", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek= DayOfWeek.Tuesday, StartTime = new TimeSpan(10, 45, 0), LengthInMinutes = 150, TeacherId = 11, SubjectId = 3, Room = "BA.1.12"},
                        new Course() { CourseId = 19, CourseName = "Sztf1 Lab02", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek= DayOfWeek.Tuesday, StartTime = new TimeSpan(10, 45, 0), LengthInMinutes = 150, TeacherId = 12, SubjectId = 3, Room = "BA.1.15"},
                        new Course() { CourseId = 20, CourseName = "Sztf1 Lab03", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek= DayOfWeek.Tuesday, StartTime = new TimeSpan(10, 45, 0), LengthInMinutes = 150, TeacherId = 13, SubjectId = 3, Room = "BA.1.13"},
                    
                        //The Java Microservice Project Data
                        new Course() { CourseId = 21, CourseName = "JavaMs EA", CourseCapacity = 15, CourseType = CourseType.ELearning, DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeSpan(17, 55, 0), LengthInMinutes = 100, TeacherId = 1, SubjectId = 4, Room = "eLearning"},
                        new Course() { CourseId = 22, CourseName = "JavaMs Lab01", CourseCapacity = 15, CourseType = CourseType.ELearning, DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeSpan(10, 45, 0), LengthInMinutes = 150, TeacherId = 12, SubjectId = 4, Room = "BA.1.16"}
                    }
                );

            builder.Entity<SubjectRegistration>().HasData(
                        new object[]
                        {
                            new { Id = 1, StudentId = 1, SubjectId = 1 },
                            new { Id = 2, StudentId = 3, SubjectId = 1},
                            new { Id = 3, StudentId = 7, SubjectId = 1},
                            new { Id = 4, StudentId = 17, SubjectId = 1},
                            new { Id = 5, StudentId = 1, SubjectId = 4 }
                        }
                );

            builder.Entity<CourseRegistration>().HasData(
                        new object[]
                        {
                            //JavaWeb registrations
                            new { Id = 1, StudentId = 1, CourseId = 1},
                            new { Id = 2, StudentId = 3, CourseId = 1},
                            new { Id = 3, StudentId = 7, CourseId = 1},
                            new { Id = 4, StudentId = 17, CourseId = 1},
                            new { Id = 5, StudentId = 1, CourseId = 3},
                            new { Id = 6, StudentId = 3, CourseId = 3},
                            new { Id = 7, StudentId = 7, CourseId = 3},
                            new { Id = 8, StudentId = 17, CourseId = 5},
                            new { Id = 9, StudentId = 1, CourseId = 21},
                            new { Id = 10, StudentId = 1, CourseId = 22}
                        }
                );

            builder.Entity<Grade>().HasData(
                        new Grade[]
                        {
                            new Grade() { GradeId = 1, StudentId = 1, Semester = "2022/23/2", Mark = 5, TeacherId = 6, SubjectId = 3},
                            new Grade() { GradeId = 2, StudentId = 1, Semester = "2022/23/1", Mark = 5, TeacherId = 11, SubjectId = 2},
                            new Grade() { GradeId = 3, StudentId = 1, Semester = "2023/24/1", Mark = 4, TeacherId = 12, SubjectId = 4},
                            new Grade() { GradeId = 4, StudentId = 2, Semester = "2023/24/1", Mark = 3, TeacherId = 11, SubjectId = 2},
                            new Grade() { GradeId = 5, StudentId = 2, Semester = "2022/23/2", Mark = 2, TeacherId = 12, SubjectId = 1},
                            new Grade() { GradeId = 6, StudentId = 2, Semester = "2022/23/1", Mark = 1, TeacherId = 12, SubjectId = 1},
                            new Grade() { GradeId = 7, StudentId = 6, Semester = "2022/23/1", Mark = 1, TeacherId = 12, SubjectId = 1},
                            new Grade() { GradeId = 8, StudentId = 4, Semester = "2022/23/1", Mark = 2, TeacherId = 12, SubjectId = 2}

                        }
                );



        }
    }
}
