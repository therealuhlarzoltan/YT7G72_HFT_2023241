using ConsoleTools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        const int COLUMN_WIDTH = 16;
        const int SEPARATOR_WIDTH = 2;
        static void Main(string[] args)
        {
            ConsoleMenu menu = null;

            var studentSubmenu = new ConsoleMenu(args, level: 1);
            studentSubmenu
            .Add("List", () => { menu.CloseMenu(); studentSubmenu.CloseMenu(); List<Student>(); })
            .Add("Create", () => Create<Student>())
            .Add("Delete", () => { menu.CloseMenu(); Delete<Student>(); menu.Show(); })
            .Add("Update", () => Update<Student>())
            .Add("Get Registered Subjects", () => { menu.CloseMenu(); studentSubmenu.CloseMenu(); GetSubjects<Student>(); })
            .Add("Exit", ConsoleMenu.Close);

            var teacherSubmenu = new ConsoleMenu(args, level: 1);
            teacherSubmenu
            .Add("List", () => { menu.CloseMenu(); teacherSubmenu.CloseMenu(); List<Teacher>(); })
            .Add("Create", () => Create<Teacher>())
            .Add("Delete", () => Delete<Teacher>())
            .Add("Update", () => Update<Teacher>());

            var subjectSubmenu = new ConsoleMenu(args, level: 1);
            subjectSubmenu
            .Add("List", () => { menu.CloseMenu(); subjectSubmenu.CloseMenu(); List<Subject>(); })
            .Add("Create", () => Create<Subject>())
            .Add("Delete", () => Delete<Subject>())
            .Add("Update", () => Update<Subject>())
            .Add("Exit", ConsoleMenu.Close);

            var courseSubmenu = new ConsoleMenu(args, level: 1);
            courseSubmenu
            .Add("List", () => { menu.CloseMenu(); courseSubmenu.CloseMenu(); List<Course>(); })
            .Add("Create", () => Create<Course>())
            .Add("Delete", () => Delete<Course>())
            .Add("Update", () => Update<Course>())
            .Add("Exit", ConsoleMenu.Close);

            var gradeSubmenu = new ConsoleMenu(args, level: 1);
            gradeSubmenu
            .Add("List", () => { menu.CloseMenu(); courseSubmenu.CloseMenu(); List<Grade>(); })
            .Add("Create", () => Create<Grade>())
            .Add("Delete", () => Delete<Grade>())
            .Add("Update", () => Update<Grade>())
            .Add("Exit", ConsoleMenu.Close);

            menu = new ConsoleMenu(args, level: 0);
            menu
            .Add("Students", () => studentSubmenu.Show())
            .Add("Teachers", () => teacherSubmenu.Show())
            .Add("Subjects", () => subjectSubmenu.Show())
            .Add("Courses", () => courseSubmenu.Show())
            .Add("Grades", () => gradeSubmenu.Show())
            .Add("Subject Registration", () => Console.Write("asd"))
            .Add("Course Registration", () => Console.Write("asd"))
            .Add("Exit", ConsoleMenu.Close);

            menu.Show();



        }

        static void List<T>() where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            IEnumerable<object> querySet = null;
            

            if (type == typeof(Student))
            {
                querySet = personLogic.GetStudents();
            }
            else if (type == typeof(Teacher))
            {
                querySet = personLogic.GetTeachers();
            }
            else if (type == typeof(Subject))
            {
                querySet = educationLogic.GetAllSubjects();
            }
            else if (type == typeof(Course))
            {
                querySet = educationLogic.GetAllCourses();
            }

            properties = properties.Where(prop => !prop.GetAccessors()[0].IsVirtual).ToArray();

            foreach (var property in properties)
            {
                if (property.Name.Length > COLUMN_WIDTH)
                {
                    Console.Write($"{property.Name.Substring(0, COLUMN_WIDTH).PadRight(COLUMN_WIDTH + SEPARATOR_WIDTH)}");
                }
                else
                {
                    Console.Write($"{property.Name.PadRight(COLUMN_WIDTH + SEPARATOR_WIDTH)}");
                }
            }
            Console.WriteLine();


            foreach (var item in querySet ?? Enumerable.Empty<T>())
            {
                foreach (var property in properties)
                {
                    if (property.GetValue(item) != null)
                    {
                        string otuput = property.GetValue(item).ToString();
                        if (otuput.Length > COLUMN_WIDTH)
                        {
                            Console.Write($"{otuput.Substring(0, COLUMN_WIDTH).PadRight(COLUMN_WIDTH + SEPARATOR_WIDTH)}");
                        }
                        else
                        {
                            Console.Write($"{otuput.PadRight(COLUMN_WIDTH + SEPARATOR_WIDTH)}");
                        }
                    }
                    else
                    {
                        Console.Write("-".PadRight(COLUMN_WIDTH + SEPARATOR_WIDTH));
                    }
                }
                Console.WriteLine();
            }


            Console.ReadKey();
            Main(new string[] { });
        }

        static void Create<T>()
        {

        }

        static void Update<T>()
        {

        }

        static void Delete<T>()
        {
            Type type = typeof(T);
            Console.Write($"Enter {type} ID to delete: ");
            int id;
            if (!Int32.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                Main(new string[] { });
            }

            try
            {
                if (type == typeof(Student))
                {
                    personLogic.RemoveStudent(id);
                }
                else if (type == typeof(Teacher))
                {
                    personLogic.RemoveTeacher(id);
                }
                else if (type == typeof(Subject))
                {
                    educationLogic.RemoveSubject(id);
                }
                else if (type == typeof(Course))
                {
                    educationLogic.RemoveCourse(id);
                }
                Console.WriteLine($"{type} with ID of {id} was deleted");
            }
            catch (ObjectNotFoundException exception)
            {
                Console.WriteLine(exception);
            } 
            finally
            {
                Console.ReadKey();
                Main(new string[] { }); 
            }
        }

        static void GetSubjects<T>() where T : IRegistableForSubject
        {

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

        static void OldTestingMethod()
        {
            Console.WriteLine("Curriculums:");
            foreach (var curriculum in educationLogic.GetAllCurriculums())
            {
                Console.WriteLine($"\t{curriculum}");
            }
            Console.WriteLine();
            Console.WriteLine("Students:");
            foreach (var student in personLogic.GetStudents())
            {
                Console.WriteLine($"\t{student}");
            }
            Console.WriteLine();
            Console.WriteLine("Teachers:");
            foreach (var teacher in personLogic.GetTeachers())
            {
                Console.WriteLine($"\t{teacher}");
            }
            Console.WriteLine();
            Console.WriteLine("Subjects and Courses:");
            foreach (var subject in educationLogic.GetAllSubjects())
            {
                Console.WriteLine($"\tSubject: {subject}; Teacher: {subject.Teacher}");
                Console.WriteLine("\t\tCourses:");
                foreach (var course in subject.SubjectCourses)
                {
                    Console.WriteLine($"\t\t\t{course}");
                }
            }
            Console.WriteLine();
            Console.WriteLine("Students on Java Web:");
            foreach (var student in educationLogic.GetSubject(1).RegisteredStudents)
            {
                Console.WriteLine($"\tStudent: {student}");
                Console.WriteLine($"\t\tEnrolled Courses");
                foreach (var course in student.RegisteredCourses)
                {
                    Console.WriteLine($"\t\t\t{course}");
                }
            }
            Console.WriteLine();
            try
            {
                Console.WriteLine($"Student with ID of 123232: {personLogic.GetStudent(123232)}");
            }
            catch (ObjectNotFoundException exception)
            {
                Console.WriteLine(exception);
            }

            Console.WriteLine();
            try
            {
                educationLogic.RegisterStudentForSubject(1, 2);
            }
            catch (PreRequirementsNotMetException exception)
            {
                Console.WriteLine($"{exception.Student} did not meet the entry requirement for {exception.Subject}");
            }
        }
    }
}
