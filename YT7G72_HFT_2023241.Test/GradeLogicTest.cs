using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Logic;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.Repository;

namespace YT7G72_HFT_2023241.Test
{
    [TestFixture]
    internal class GradeLogicTest
    {
        Mock<IRepository<Grade>> gradeRepository;
        IGradeLogic gradeLogic;

        [SetUp]
        public void Init()
        {
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

            gradeRepository = new Mock<IRepository<Grade>>();
            gradeRepository.Setup(r => r.ReadAll()).Returns(grades.AsQueryable());

            gradeLogic = new GradeLogic(gradeRepository.Object);
        }

        [Test]
        public void GradeCreationTest()
        {
            var grade1 = new Grade() { Semester = "", Mark = 0 };
            try
            {
                gradeLogic.AddGrade(grade1);
            }
            catch { }

            Assert.That(() =>gradeLogic.AddGrade(grade1), Throws.TypeOf<ArgumentException>());
            gradeRepository.Verify(r => r.Create(grade1), Times.Never);

            var grade2 = new Grade() { Semester = "2023/24/1", Mark = -3 };
            try
            {
                gradeLogic.AddGrade(grade2);
            }
            catch { }

            Assert.That(() => gradeLogic.AddGrade(grade2), Throws.TypeOf<ArgumentException>());
            gradeRepository.Verify(r => r.Create(grade2), Times.Never);

            var grade3 = new Grade() { Semester = "2023/24/11", Mark = 3 };
            try
            {
                gradeLogic.AddGrade(grade3);
            }
            catch { }

            Assert.That(() => gradeLogic.AddGrade(grade3), Throws.TypeOf<ArgumentException>());
            gradeRepository.Verify(r => r.Create(grade3), Times.Never);

            var grade4 = new Grade() { Semester = "2023-24/1", Mark = 3 };
            try
            {
                gradeLogic.AddGrade(grade4);
            }
            catch { }

            Assert.That(() => gradeLogic.AddGrade(grade4), Throws.TypeOf<ArgumentException>());
            gradeRepository.Verify(r => r.Create(grade4), Times.Never);

            var grade5 = new Grade() { Semester = "2023/24/1", Mark = 4 };
            try
            {
                gradeLogic.AddGrade(grade5);
            }
            catch { }

            Assert.That(() => gradeLogic.AddGrade(grade5), Throws.Nothing);
            gradeRepository.Verify(r => r.Create(grade5), Times.Exactly(2));
        }

        [TestCaseSource(nameof(SubjectStatisticsSource))]
        public void SubjectStatisticsTest(int subjectId, SubjectStatistics expected)
        {
            var result = gradeLogic.GetSubjectStatistics(subjectId);
            Assert.AreEqual(expected, result);
        }

        [TestCaseSource(nameof(SemesterStatisticsSource))]
        public void SemesterStatisticsTest(string semester, SemesterStatistics expected)
        {
            var result = gradeLogic.GetSemesterStatistics(semester);
            Assert.AreEqual(expected, result);
        }

        static IEnumerable<TestCaseData> SubjectStatisticsSource()
        {
            return new List<TestCaseData>
            {
                new TestCaseData(new object[] { 1, new SubjectStatistics() { Subject = new Subject() { SubjectId = 1, SubjectName = "Web development using Java and Spring Boot", SubjectCode = "NIKCEJAVAWEB", Credits = 4, CurriculumId = 1, TeacherId = 1, Requirement = Requirement.SPOKEN_EXAM, PreRequirementId = 2 }, NumberOfRegistrations = 3, Avg = 4d/3d, PassPerRegistrationRatio = 1d/3d }}),
                new TestCaseData(new object[] { 2, new SubjectStatistics() { Subject = new Subject() { SubjectId = 2, SubjectName = "Software Design and Development II", SubjectCode = "NIKCESZTF2", Credits = 6, CurriculumId = 1, TeacherId = 6, Requirement = Requirement.WRITTEN_AND_SPOKEN_EXAM, PreRequirementId = 3 }, NumberOfRegistrations = 3, Avg = 11d/3d, PassPerRegistrationRatio = 1d }}),
                new TestCaseData(new object[] { 3, new SubjectStatistics() { Subject = new Subject() { SubjectId = 3, SubjectName = "Software Design and Development I", SubjectCode = "NIKCESZTF1", Credits = 6, CurriculumId = 1, TeacherId = 11, Requirement = Requirement.WRITTEN_AND_SPOKEN_EXAM }, NumberOfRegistrations = 1, Avg = 5d, PassPerRegistrationRatio = 1d }}),
                new TestCaseData(new object[] { 4, new SubjectStatistics() { Subject = new Subject() { SubjectId = 4, SubjectName = "The Java Microservice Project", SubjectCode = "NICKEJAVAMSP", Credits = 3, CurriculumId = 1, TeacherId = 1, Requirement = Requirement.MID_TERM_MARK, PreRequirementId = 2 }, NumberOfRegistrations = 1, Avg = 4d, PassPerRegistrationRatio = 1d }}),
                new TestCaseData(new object[] { 5, new SubjectStatistics() { Subject = null, Avg = -1, NumberOfRegistrations = 0, PassPerRegistrationRatio = -1} })
            };
        }

        static IEnumerable<TestCaseData> SemesterStatisticsSource()
        {
            return new List<TestCaseData> {
                new TestCaseData(null, new SemesterStatistics() { Semester = null, WeightedAvg = -1, NumberOfFailures = -1, NumberOfPasses = -1} ),
                new TestCaseData("2023/24/1", new SemesterStatistics() { Semester = "2023/24/1", WeightedAvg = 30d/9d, NumberOfFailures = 0, NumberOfPasses = 2} ),
                new TestCaseData("2023-24-1", new SemesterStatistics() { Semester = "2023-24-1", WeightedAvg = -1, NumberOfFailures = -1, NumberOfPasses = -1} )
            };
        }
    }
}
