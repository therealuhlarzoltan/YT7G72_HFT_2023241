using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using YT7G72_HFT_2023241.Logic;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.Repository;

namespace YT7G72_HFT_2023241.Test
{
    [TestFixture]
    internal class PersonLogicTest
    {
        Mock<IRepository<Student>> studentRepository;
        Mock<IRepository<Teacher>> teacherRepository;
        IPersonLogic personLogic;

        [SetUp]
        public void Init()
        {
            List<Student> students = new List<Student>()
            {
                new Student {StudentId = 1, FirstName = "Zoltán", LastName = "Uhlár", StudentCode = "YT7G72", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                new Student {StudentId = 2, FirstName = "Gergely", LastName = "Lucza", StudentCode = "BZ6QL7", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                new Student {StudentId = 3, FirstName = "Roland", LastName = "Kertész", StudentCode = "COH045", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                new Student {StudentId = 4, FirstName = "Roland", LastName = "Varga", StudentCode = "S8HE52", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                new Student {StudentId = 5, FirstName = "Bálint", LastName = "Nagy", StudentCode = "JKL123", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                new Student {StudentId = 6, FirstName = "István", LastName = "Kovács", StudentCode = "QWE456", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                new Student {StudentId = 7, FirstName = "Anna", LastName = "Tóth", StudentCode = "ASD789", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                new Student {StudentId = 8, FirstName = "Eszter", LastName = "Szabó", StudentCode = "XYZ012", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                new Student {StudentId = 9, FirstName = "Márta", LastName = "Papp", StudentCode = "UIO345", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                new Student {StudentId = 10, FirstName = "Katalin", LastName = "Molnár", StudentCode = "MNB678", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship}
        };

            List<Teacher> teachers = new List<Teacher>()
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
                        new Teacher() { TeacherId = 10, FirstName = "Attila", LastName = "Molnár", AcademicRank = AcademicRank.Technical_Assistant}
            };

            List<Grade> grades = new List<Grade>()
            {
                  new Grade() { GradeId = 1, StudentId = 1, Semester = "2022/23/2", Mark = 5, TeacherId = 6, SubjectId = 3},
                            new Grade() { GradeId = 2, StudentId = 1, Semester = "2022/23/1", Mark = 5, TeacherId = 1, SubjectId = 2},
                            new Grade() { GradeId = 3, StudentId = 1, Semester = "2023/24/1", Mark = 4, TeacherId = 1, SubjectId = 4},
                            new Grade() { GradeId = 4, StudentId = 2, Semester = "2023/24/1", Mark = 3, TeacherId = 1, SubjectId = 2},
                            new Grade() { GradeId = 5, StudentId = 2, Semester = "2022/23/2", Mark = 2, TeacherId = 5, SubjectId = 1},
                            new Grade() { GradeId = 6, StudentId = 2, Semester = "2022/23/1", Mark = 1, TeacherId = 6, SubjectId = 1},
                            new Grade() { GradeId = 7, StudentId = 6, Semester = "2022/23/1", Mark = 1, TeacherId = 6, SubjectId = 1},
                            new Grade() { GradeId = 8, StudentId = 4, Semester = "2022/23/1", Mark = 3, TeacherId = 7, SubjectId = 2}
            };

            List<Subject> subjects = new List<Subject>()
            {
                 new Subject() { SubjectId = 3, SubjectName = "Software Design and Development I", SubjectCode = "NIKCESZTF1", Credits = 6, CurriculumId = 1, TeacherId = 11, Requirement =  Requirement.WRITTEN_AND_SPOKEN_EXAM },
                        new Subject() { SubjectId = 2, SubjectName = "Software Design and Development II", SubjectCode = "NIKCESZTF2", Credits = 6,  CurriculumId = 1, TeacherId = 6, Requirement = Requirement.WRITTEN_AND_SPOKEN_EXAM, PreRequirementId = 3},
                        new Subject() { SubjectId = 1, SubjectName = "Web development using Java and Spring Boot", SubjectCode = "NIKCEJAVAWEB", Credits = 4, CurriculumId = 1, TeacherId = 1, Requirement = Requirement.SPOKEN_EXAM, PreRequirementId = 2 },
                        new Subject() { SubjectId = 4, SubjectName = "The Java Microservice Project", SubjectCode = "NICKEJAVAMSP", Credits = 3,  CurriculumId = 1, TeacherId = 1, Requirement = Requirement.MID_TERM_MARK, PreRequirementId = 2}
            };


           foreach (var student in students)
           {
                foreach (var grade in grades)
                {
                    if (grade.StudentId == student.StudentId)
                    {
                        student.Grades.Add(grade);
                    }
                }
           }

            foreach (var teacher in teachers)
            {
                foreach (var grade in grades)
                {
                    if (grade.TeacherId == teacher.TeacherId)
                    {
                        teacher.GivenGrades.Add(grade);
                    }
                }
            }

            foreach (var grade in grades)
            {
                foreach (var subject in subjects)
                {
                    if (grade.SubjectId == subject.SubjectId)
                    {
                        grade.Subject = subject;
                    }
                }
            }

            studentRepository = new Mock<IRepository<Student>>();
            teacherRepository = new Mock<IRepository<Teacher>>();
            studentRepository.Setup(r => r.ReadAll()).Returns(students.AsQueryable());
            teacherRepository.Setup(r => r.ReadAll()).Returns(teachers.AsQueryable());

            personLogic = new PersonLogic(studentRepository.Object, teacherRepository.Object);
        }

        [Test]
        public void StudentCreationTest()
        {
            var student1 = new Student() { FirstName = "", LastName = "", StudentCode = "asd" };
            try
            {
                personLogic.AddStudent(student1);
            }
            catch { }

            Assert.That(() => personLogic.AddStudent(student1), Throws.TypeOf<ArgumentException>());
            studentRepository.Verify(r => r.Create(student1), Times.Never);

            var student2 = new Student() { FirstName = "Zo", LastName = "U", StudentCode = "YT7G72", CurriculumId = 1 };
            try
            {
                personLogic.AddStudent(student2);
            }
            catch { }

            Assert.That(() => personLogic.AddStudent(student2), Throws.TypeOf<ArgumentException>());
            studentRepository.Verify(r => r.Create(student2), Times.Never);

            var student3 = new Student() { FirstName = "Zoltán", LastName = "Uhlár", StudentCode = "YT7G7Í", CurriculumId = 1 };
            try
            {
                personLogic.AddStudent(student3);
            }
            catch { }

            Assert.That(() => personLogic.AddStudent(student3), Throws.TypeOf<ArgumentException>());
            studentRepository.Verify(r => r.Create(student3), Times.Never);

            var student4 = new Student() { FirstName = "Zoltán", LastName = "Uhlár", StudentCode = "YT7G72", CurriculumId = 1 };
            try
            {
                personLogic.AddStudent(student4);
            }
            catch { }

            Assert.That(() => personLogic.AddStudent(student4), Throws.Nothing);
            studentRepository.Verify(r => r.Create(student4), Times.Exactly(2));

        }

        [Test]
        public void TeacherCreationTest()
        {
            var teacher1 = new Teacher() { FirstName = "", LastName = ""};
            try
            {
                personLogic.AddTeacher(teacher1);
            }
            catch { }

            Assert.That(() => personLogic.AddTeacher(teacher1), Throws.TypeOf<ArgumentException>());
            teacherRepository.Verify(r => r.Create(teacher1), Times.Never);

            var teacher2 = new Teacher() { FirstName = "Zo", LastName = "U", AcademicRank = AcademicRank.Associate_Professor };
            try
            {
                personLogic.AddTeacher(teacher2);
            }
            catch { }

            Assert.That(() => personLogic.AddTeacher(teacher2), Throws.TypeOf<ArgumentException>());
            teacherRepository.Verify(r => r.Create(teacher2), Times.Never);

            var teacher3 = new Teacher() { FirstName = "Zoltán", LastName = "Uhlár"};
            try
            {
                personLogic.AddTeacher(teacher3);
            }
            catch { }

            Assert.That(() => personLogic.AddTeacher(teacher3), Throws.Nothing);
            teacherRepository.Verify(r => r.Create(teacher3), Times.Exactly(2));

            var teacher4 = new Teacher() { FirstName = "Zoltán", LastName = "Uhlár", AcademicRank = AcademicRank.Associate_Professor };
            try
            {
                personLogic.AddTeacher(teacher4);
            }
            catch { }

            Assert.That(() => personLogic.AddTeacher(teacher4), Throws.Nothing);
            teacherRepository.Verify(r => r.Create(teacher4), Times.Exactly(2));

        }

        [Test]
        public void BestStudentsTest()
        {
            var result = personLogic.GetBestStudents();
            var expected = new List<Tuple<Student, double>> {
                Tuple.Create(
                    new Student {StudentId = 1, FirstName = "Zoltán", LastName = "Uhlár", StudentCode = "YT7G72", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    24d/5d
                    ),
                Tuple.Create(
                    new Student {StudentId = 4, FirstName = "Roland", LastName = "Varga", StudentCode = "S8HE52", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    3d
                    ),
                 Tuple.Create(
                    new Student {StudentId = 2, FirstName = "Gergely", LastName = "Lucza", StudentCode = "BZ6QL7", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship},
                    15d/7d
                    )
            };

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void BestTeachersTest()
        {
            var result = personLogic.GetBestTeachers();
            var expected = new List<Tuple<Teacher, double>> {
                Tuple.Create(
                    new Teacher() { TeacherId = 1, FirstName = "Gabriella", LastName = "Nagy", AcademicRank = AcademicRank.Senior_Lecturer},
                    4d
                    ),
                 Tuple.Create(
                   new Teacher() { TeacherId = 7, FirstName = "Zoltán", LastName = "Király", AcademicRank = AcademicRank.Associate_Professor},
                    3d
                    ),
                  Tuple.Create(
                    new Teacher() { TeacherId = 6, FirstName = "Sándor", LastName = "Szénási", AcademicRank = AcademicRank.Professor},
                    7d/3d
                    )
            };

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void BestTeachersByAcademicRankTest()
        {
            var result = personLogic.GetBestTeachersByAcademicRank(AcademicRank.Associate_Professor);
            var expected = new List<Tuple<Teacher, double>> {
                 Tuple.Create(
                   new Teacher() { TeacherId = 7, FirstName = "Zoltán", LastName = "Király", AcademicRank = AcademicRank.Associate_Professor},
                    3d
                    )
            };

            Assert.AreEqual(expected, result);

        }
    }
}
