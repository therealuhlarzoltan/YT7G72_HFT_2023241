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
    internal class EducationLogicTest
    {
        Mock<IRepository<Student>> studentRepository;
        Mock<IRepository<Subject>> subjectRepository;
        Mock<IRepository<Course>> courseRepository;
        IEducationLogic educationLogic;

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

            List<Course> courses = new List<Course>()
            {
                new Course() { CourseId = 1, CourseName = "JavaWeb EA", CourseCapacity = 350,  CourseType = CourseType.Lecture, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(16, 15, 0), LengthInMinutes = 90, TeacherId = 1, SubjectId = 1, Room = "BA.1.32.AudMax"},
                new Course() { CourseId = 2, CourseName = "JavaWeb Lab01", CourseCapacity = 15,  CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(12, 35, 0), LengthInMinutes = 90, TeacherId = 1, SubjectId = 1, Room = "BA.1.10"},
                new Course() { CourseId = 3, CourseName = "JavaWeb Lab02", CourseCapacity = 15,  CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(14, 25, 0), LengthInMinutes = 90, TeacherId = 1, SubjectId = 1, Room = "BA.1.10"},
                new Course() { CourseId = 9, CourseName = "Sztf2 EA", CourseCapacity = 350, CourseType = CourseType.Lecture, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(8, 0, 0), LengthInMinutes = 150, TeacherId = 6, SubjectId = 2, Room = "BA.1.32.AudMax"},
                new Course() { CourseId = 10, CourseName = "Sztf2 Lab01", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(8, 0, 0), LengthInMinutes = 150, TeacherId = 7, SubjectId = 2, Room = "BA.1.13"},
                new Course() { CourseId = 11, CourseName = "Sztf2 Lab02", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(12, 35, 0), LengthInMinutes = 150, TeacherId = 8, SubjectId = 2, Room = "BA.1.15"},
                new Course() { CourseId = 17, CourseName = "Sztf1 EA", CourseCapacity = 350, CourseType = CourseType.Lecture, DayOfWeek= DayOfWeek.Monday, StartTime = new TimeSpan(10, 45, 0), LengthInMinutes = 150, TeacherId = 12, SubjectId = 3, Room = "BA.1.32.AudMax"},
                new Course() { CourseId = 18, CourseName = "Sztf1 Lab01", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek= DayOfWeek.Tuesday, StartTime = new TimeSpan(10, 45, 0), LengthInMinutes = 150, TeacherId = 11, SubjectId = 3, Room = "BA.1.12"},
                new Course() { CourseId = 21, CourseName = "JavaMs EA", CourseCapacity = 15, CourseType = CourseType.ELearning, DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeSpan(17, 55, 0), LengthInMinutes = 100, TeacherId = 1, SubjectId = 4, Room = "eLearning"},
                new Course() { CourseId = 22, CourseName = "JavaMs Lab01", CourseCapacity = 15, CourseType = CourseType.ELearning, DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeSpan(10, 45, 0), LengthInMinutes = 150, TeacherId = 12, SubjectId = 4, Room = "BA.1.16"}
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

            foreach (var subject in subjects)
            {
                foreach (var course in courses)
                {
                    if (course.SubjectId == subject.SubjectId)
                    {
                        subject.SubjectCourses.Add(course);
                        course.Subject = subject;
                    }
                }
            }

            foreach (var subject in subjects)
            {
                if (subject.PreRequirementId != null)
                {
                    subject.PreRequirement = subjects.Where(s => s.SubjectId == subject.PreRequirementId).FirstOrDefault();
                }
            }

            studentRepository = new Mock<IRepository<Student>>();
            subjectRepository = new Mock<IRepository<Subject>>();
            courseRepository = new Mock<IRepository<Course>>();
            studentRepository.Setup(r => r.ReadAll()).Returns(students.AsQueryable());
            studentRepository.Setup(r => r.Read(It.IsAny<int>())).Returns((int id) => students.Where(s => s.StudentId == id).FirstOrDefault());
            subjectRepository.Setup(r => r.ReadAll()).Returns(subjects.AsQueryable());
            subjectRepository.Setup(r => r.Read(It.IsAny<int>())).Returns((int id) => subjects.Where(s => s.SubjectId == id).FirstOrDefault());
            courseRepository.Setup(r => r.ReadAll()).Returns(courses.AsQueryable());
            courseRepository.Setup(r => r.Read(It.IsAny<int>())).Returns((int id) => courses.Where(c => c.CourseId == id).FirstOrDefault());

            educationLogic = new EducationLogic(studentRepository.Object, courseRepository.Object, subjectRepository.Object);

            students[0].RegisteredSubjects.Add(subjects[0]);
            subjects[0].RegisteredStudents.Add(students[0]);
            students[0].RegisteredSubjects.Add(subjects[1]);
            subjects[1].RegisteredStudents.Add(students[0]);

        }

        [Test]
        public void SubjectRegistrationTest()
        {
            var stud = new Student { StudentId = 2, FirstName = "Gergely", LastName = "Lucza", StudentCode = "BZ6QL7", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship };
            var sub1 = new Subject() { SubjectId = 3, SubjectName = "Software Design and Development I", SubjectCode = "NIKCESZTF1", Credits = 6, CurriculumId = 1, TeacherId = 11, Requirement = Requirement.WRITTEN_AND_SPOKEN_EXAM };
            var sub2 = new Subject() { SubjectId = 2, SubjectName = "Software Design and Development II", SubjectCode = "NIKCESZTF2", Credits = 6, CurriculumId = 1, TeacherId = 6, Requirement = Requirement.WRITTEN_AND_SPOKEN_EXAM, PreRequirementId = 3 };
            var sub3 = new Subject() { SubjectId = 1, SubjectName = "Web development using Java and Spring Boot", SubjectCode = "NIKCEJAVAWEB", Credits = 4, CurriculumId = 1, TeacherId = 1, Requirement = Requirement.SPOKEN_EXAM, PreRequirementId = 2 };
            var sub4 = new Subject() { SubjectId = 4, SubjectName = "The Java Microservice Project", SubjectCode = "NICKEJAVAMSP", Credits = 3, CurriculumId = 1, TeacherId = 1, Requirement = Requirement.MID_TERM_MARK, PreRequirementId = 2 };

            try
            {
                educationLogic.RegisterStudentForSubject(stud.StudentId, sub2.SubjectId);
            }
            catch { }

            Assert.That(() => educationLogic.RegisterStudentForSubject(stud.StudentId, sub2.SubjectId), Throws.TypeOf<PreRequirementsNotMetException>());
            subjectRepository.Verify(r => r.Update(sub2), Times.Never);

            try
            {
                educationLogic.RegisterStudentForSubject(stud.StudentId, sub1.SubjectId);
            }
            catch { }

            Assert.That(() => educationLogic.RegisterStudentForSubject(stud.StudentId, sub1.SubjectId), Throws.Nothing);
            subjectRepository.Verify(r => r.Update(sub1), Times.Exactly(2));

            try
            {
                educationLogic.RegisterStudentForSubject(stud.StudentId, sub3.SubjectId);
            }
            catch { }

            Assert.That(() => educationLogic.RegisterStudentForSubject(stud.StudentId, sub3.SubjectId), Throws.Nothing);
            subjectRepository.Verify(r => r.Update(sub3), Times.Exactly(2));


            try
            {
                educationLogic.RegisterStudentForSubject(stud.StudentId, sub4.SubjectId);
            }
            catch { }

            Assert.That(() => educationLogic.RegisterStudentForSubject(stud.StudentId, sub4.SubjectId), Throws.Nothing);
            subjectRepository.Verify(r => r.Update(sub4), Times.Exactly(2));

        }


        [Test]
        public void CourseRegistrationTest()
        {
            var student = new Student { StudentId = 1, FirstName = "Zoltán", LastName = "Uhlár", StudentCode = "YT7G72", CurriculumId = 1, FinancialStatus = FinancialStatus.Full_State_Scholarship };
            var sub1 = new Subject() { SubjectId = 3, SubjectName = "Software Design and Development I", SubjectCode = "NIKCESZTF1", Credits = 6, CurriculumId = 1, TeacherId = 11, Requirement = Requirement.WRITTEN_AND_SPOKEN_EXAM };
            var course1 = new Course() { CourseId = 17, CourseName = "Sztf1 EA", CourseCapacity = 350, CourseType = CourseType.Lecture, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(10, 45, 0), LengthInMinutes = 150, TeacherId = 12, SubjectId = 3, Room = "BA.1.32.AudMax" };
            var sub2 = new Subject() { SubjectId = 2, SubjectName = "Software Design and Development II", SubjectCode = "NIKCESZTF2", Credits = 6, CurriculumId = 1, TeacherId = 6, Requirement = Requirement.WRITTEN_AND_SPOKEN_EXAM, PreRequirementId = 3 };
            var course2 = new Course() { CourseId = 10, CourseName = "Sztf2 Lab01", CourseCapacity = 15, CourseType = CourseType.Lab, DayOfWeek = DayOfWeek.Monday, StartTime = new TimeSpan(8, 0, 0), LengthInMinutes = 150, TeacherId = 7, SubjectId = 2, Room = "BA.1.13" };
            var course3 = new Course() { CourseId = 22, CourseName = "JavaMs Lab01", CourseCapacity = 15, CourseType = CourseType.ELearning, DayOfWeek = DayOfWeek.Tuesday, StartTime = new TimeSpan(10, 45, 0), LengthInMinutes = 150, TeacherId = 12, SubjectId = 4, Room = "BA.1.16" };

            try
            {
                educationLogic.RegisterStudentForCourse(student.StudentId, course1.CourseId);
            }
            catch { }

            Assert.That(() => educationLogic.RegisterStudentForCourse(student.StudentId, course1.CourseId), Throws.Nothing);
            courseRepository.Verify(r => r.Update(course1), Times.Exactly(2));

            try
            {
                educationLogic.RegisterStudentForCourse(student.StudentId, course2.CourseId);
            }
            catch { }

            Assert.That(() => educationLogic.RegisterStudentForCourse(student.StudentId, course2.CourseId), Throws.Nothing);
            courseRepository.Verify(r => r.Update(course2), Times.Exactly(2));

            try
            {
                educationLogic.RegisterStudentForCourse(student.StudentId, course3.CourseId);
            }
            catch { }

            Assert.That(() => educationLogic.RegisterStudentForCourse(student.StudentId, course3.CourseId), Throws.TypeOf<NotRegisteredForSubjectException>());
            courseRepository.Verify(r => r.Update(course3), Times.Never);



        }
        
    }
}
