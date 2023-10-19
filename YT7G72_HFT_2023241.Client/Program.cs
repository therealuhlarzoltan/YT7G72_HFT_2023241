using System;
using System.Linq;
using YT7G72_HFT_2023241.Logic;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.Repository;

namespace YT7G72_HFT_2023241.Client
{
    internal class Program
    {
        static UniversityDatabaseContext context = new UniversityDatabaseContext();
        static IRepository<Student> studentRepository = new StudentRepository(context);
        static IRepository<Teacher> teacherRepository = new TeacherRepository(context);
        static IRepository<Subject> subjectRepository = new SubjectRepository(context);
        static IRepository<Course> courseRepository = new CourseRepository(context);
        static IRepository<Curriculum> curriculumRepository = new CurriculumRepository(context);
        static IRepository<Grade> gradeRepository = new GradeRepository(context);
        static IPersonLogic personLogic = new PersonLogic(studentRepository, teacherRepository);
        static IEducationLogic educationLogic = new EducationLogic(studentRepository, courseRepository, subjectRepository, curriculumRepository);
        static IGradeLogic gradeLogic = new GradeLogic(gradeRepository);
        static void Main(string[] args)
        {
            
            Console.WriteLine("Database Created");
            
            Console.WriteLine("Repositories Created");
            
            Console.WriteLine("Logics Created");
            var curriculum = CreateCurriculum();
            educationLogic.AddCurriculum(curriculum);
            var student = CreateStudent();
            personLogic.AddStudent(student);

            

        }

        static Curriculum CreateCurriculum()
        {
            var curriculum = new Curriculum();
            Console.Write("Name: ");
            curriculum.CurriculumName = Console.ReadLine();
            Console.Write("Code: ");
            curriculum.CurriculumCode = Console.ReadLine();
            return curriculum;
        }

        static Student CreateStudent()
        {
            var student = new Student();
            Console.Write("First Name: ");
            student.FirstName = Console.ReadLine();
            Console.Write("Last Name: ");
            student.LastName = Console.ReadLine();
            Console.Write("Student Code: ");
            student.StudentCode = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Available Curriculums: ");
            foreach (var curriculum in educationLogic.GetAllCurriculums())
            {
                Console.WriteLine(curriculum);
            }
            Console.Write("Select Curriculum's Number: ");
            student.Curriculum = curriculumRepository.ReadAll().AsEnumerable().ElementAt(int.Parse(Console.ReadLine()) - 1);
            return student;
        }

        static Teacher CreateTeacher()
        {
            return null;
        }
    }
}
