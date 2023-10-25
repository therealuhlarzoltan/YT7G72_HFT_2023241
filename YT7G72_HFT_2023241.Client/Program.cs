using Castle.Core.Internal;
using ConsoleTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using YT7G72_HFT_2023241.Logic;
using YT7G72_HFT_2023241.Logic.Implementations;
using YT7G72_HFT_2023241.Logic.Interfaces;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.Repository;

namespace YT7G72_HFT_2023241.Client
{
    internal class Program
    {
        static UniversityDatabaseContext context = new UniversityDatabaseContext();
        static IRepository<Student> studentRepository = new StudentRepository(context);
        static IRepository<Teacher> teacherRepository = new TeacherRepository(context);
        static IRepository<Curriculum> curriculumRepository = new CurriculumRepository(context);
        static IRepository<Subject> subjectRepository = new SubjectRepository(context);
        static IRepository<Course> courseRepository = new CourseRepository(context);
        static IRepository<Grade> gradeRepository = new GradeRepository(context);
        static IPersonLogic personLogic = new PersonLogic(studentRepository, teacherRepository);
        static IEducationLogic educationLogic = new EducationLogic(studentRepository, courseRepository, subjectRepository);
        static IGradeLogic gradeLogic = new GradeLogic(gradeRepository);
        static ICurriculumLogic curriculumLogic = new CurriculumLogic(curriculumRepository);
        const int COLUMN_WIDTH = 16;
        const int SEPARATOR_WIDTH = 1;
        static void Main(string[] args)
        {
            ConsoleMenu menu = null;

            var studentSubmenu = new ConsoleMenu(args, level: 1);
            studentSubmenu
            .Add("List", () => { menu.CloseMenu(); studentSubmenu.CloseMenu(); List<Student>(); })
            .Add("Create", () => { menu.CloseMenu(); studentSubmenu.CloseMenu(); Create<Student>(); })
            .Add("Delete", () => { menu.CloseMenu(); Delete<Student>(); menu.Show(); })
            .Add("Update", () => Update<Student>())
            .Add("Get Registered Subjects", () => { menu.CloseMenu(); studentSubmenu.CloseMenu(); GetSubjects<Student>(); })
            .Add("Get Enrolled Courses", () => { menu.CloseMenu(); studentSubmenu.CloseMenu(); GetCourses<Student>(); })
            .Add("Get Weekly Schedule", () => { menu.CloseMenu(); studentSubmenu.CloseMenu(); GetSchedule<Student>(); })
            .Add("Exit", ConsoleMenu.Close);

            var teacherSubmenu = new ConsoleMenu(args, level: 1);
            teacherSubmenu
            .Add("List", () => { menu.CloseMenu(); teacherSubmenu.CloseMenu(); List<Teacher>(); })
            .Add("Create", () => Create<Teacher>())
            .Add("Delete", () => Delete<Teacher>())
            .Add("Update", () => Update<Teacher>())
            .Add("Get Taught Subjects", () => { menu.CloseMenu(); teacherSubmenu.CloseMenu(); GetSubjects<Teacher>(); })
            .Add("Get Taught Courses", () => { menu.CloseMenu(); teacherSubmenu.CloseMenu(); GetCourses<Teacher>(); })
            .Add("Get Weekly Schedule", () => { menu.CloseMenu(); teacherSubmenu.CloseMenu(); GetSchedule<Teacher>(); } )
            .Add("Exit", ConsoleMenu.Close);

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
            .Add("List", () => { menu.CloseMenu(); gradeSubmenu.CloseMenu(); List<Grade>(); })
            .Add("Create", () => Create<Grade>())
            .Add("Delete", () => Delete<Grade>())
            .Add("Update", () => Update<Grade>())
            .Add("Get Best Students", () => { menu.CloseMenu(); gradeSubmenu.CloseMenu(); GetBestStudents(); })
            .Add("Get Best Teachers", () => { menu.CloseMenu(); gradeSubmenu.CloseMenu(); GetBestTeachers(); })
            .Add("Get Semester Statistics", () => { menu.CloseMenu(); gradeSubmenu.CloseMenu(); GetSemesterStatistics(); })
            .Add("Exit", ConsoleMenu.Close);

            var subjectRegistrationSubmenu = new ConsoleMenu(args, level: 1);
            subjectRegistrationSubmenu
                .Add("Register for Subject", () => RegisterForSubject());

            var courseRegistrationSubmenu = new ConsoleMenu(args, level: 1);
            courseRegistrationSubmenu
                .Add("Register for Course", () => RegisterForCourse());

            menu = new ConsoleMenu(args, level: 0);
            menu
            .Add("Students", () => studentSubmenu.Show())
            .Add("Teachers", () => teacherSubmenu.Show())
            .Add("Subjects", () => subjectSubmenu.Show())
            .Add("Courses", () => courseSubmenu.Show())
            .Add("Grades", () => gradeSubmenu.Show())
            .Add("Subject Registration", () => subjectRegistrationSubmenu.Show())
            .Add("Course Registration", () => courseRegistrationSubmenu.Show())
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
            else if (type == typeof(Curriculum))
            {
                querySet = curriculumLogic.GetCurriculums();
            }
            else if (type == typeof(Grade))
            {
                querySet = gradeLogic.GetAllGrades();
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

        static T CreateInstance<T>()
        {
            object instance = Activator.CreateInstance(typeof(T));
            PropertyInfo[] properties = instance.GetType().GetProperties().Where(property => !property.GetAccessors()[0].IsVirtual
           && !property.Name.Contains($"{typeof(T).Name}Id")).ToArray();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsEnum)
                {
                    Type enumType = property.PropertyType;
                    Console.WriteLine($"Available choices for {enumType.Name}:");
                    foreach (var value in Enum.GetValues(enumType))
                    {
                        Console.WriteLine($"{(int)value} -- {value}");
                    }
                }
                if (property.GetAttribute<RequiredAttribute>() != null)
                {
                    Console.Write($"{property.Name} (Required): ");
                }
                else
                {
                    Console.Write($"{property.Name}: ");
                }

                string input = Console.ReadLine();
                if (!input.IsNullOrEmpty())
                {
                    if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(instance, input);
                    }
                    else if (property.PropertyType.IsEnum)
                    {
                        var values = Enum.GetValues(property.PropertyType);
                        int converted;
                        if (int.TryParse(input, out converted))
                        {
                            foreach (var value in values)
                            {
                                if ((int)value == converted)
                                {
                                    property.SetValue(instance, value);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        var methods = property.PropertyType.GetMethods();
                        var converterMethod = methods.FirstOrDefault(m => m.Name.Contains("Parse"));
                        if (converterMethod != null)
                        {
                            try
                            {
                                var convertedValue = converterMethod.Invoke(null, new object[] { input });
                                property.SetValue(instance, convertedValue);
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Invalid input value!");
                                Console.ReadKey();
                                Main(new string[] { });
                            }
                        }
                    }
                }
            }
            return (T)instance;

        }

        static void Create<T>()
        {
            Type type = typeof(T);
            Console.WriteLine($"Creating new {type.Name} entity...");
            T entity = CreateInstance<T>();
            
            if (type == typeof(Student))
            {
                personLogic.AddStudent(entity as Student);
            }
            else if (type == typeof(Teacher))
            {
                personLogic.AddTeacher(entity as Teacher);
            }
            else if (type == typeof(Subject)) 
            {
                educationLogic.AddSubject(entity as Subject);
            }
            else if (type == typeof(Course))
            {
                educationLogic.AddCourse(entity as Course);
            }
            else if (type == typeof(Curriculum))
            {
                curriculumLogic.AddCurriculum(entity as Curriculum);
            }
            else if (type == typeof(Grade))
            {
                gradeLogic.AddGrade(entity as Grade);
            }

            Console.WriteLine($"New {type.Name} created!");
            Console.ReadKey();
            Main(new string[] { });

         }

        static void Update<T>() where T : class
        {
            Type type = typeof(T);
            object instance = null;
            Console.Write($"Please enter {type.Name} ID: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID provided!");
                Console.ReadKey();
                Main(new string[] { });
            }

            try
            {
                if (type == typeof(Student))
                {
                    instance = personLogic.GetStudent(id);
                }
                else if (type == typeof(Teacher))
                {
                    instance = personLogic.GetTeacher(id);
                }
                else if (type == typeof(Subject))
                {
                    instance = educationLogic.GetSubject(id);
                }
                else if (type == typeof(Course))
                {
                    instance = educationLogic.GetCourse(id);
                }
                else if (type == typeof(Curriculum))
                {
                    instance = curriculumLogic.GetCurriculum(id);
                }
                else if (type == typeof(Grade))
                {
                    instance = gradeLogic.GetGrade(id);
                }
                Console.WriteLine("Updating entity...");
                UpdateInstance<T>(instance as T);
                Console.WriteLine("Entity updated!");
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

        static void UpdateInstance<T>(T instance) where T : class
        {
            PropertyInfo[] properties = instance.GetType().GetProperties().Where(property => !property.GetAccessors()[0].IsVirtual
           && !property.Name.Contains($"{typeof(T).Name}Id")).ToArray();
            foreach (var property in properties)
            {
                if (property.PropertyType.IsEnum)
                {
                    Type enumType = property.PropertyType;
                    Console.WriteLine($"Available choices for {enumType.Name}:");
                    foreach (var value in Enum.GetValues(enumType))
                    {
                        Console.WriteLine($"{(int)value} -- {value}");
                    }
                }
                if (property.GetAttribute<RequiredAttribute>() != null)
                {
                    Console.Write($"{property.Name} (Required): ");
                }
                else
                {
                    Console.Write($"{property.Name}: ");
                }

                string input = Console.ReadLine();
                if (!input.IsNullOrEmpty())
                {
                    if (property.PropertyType == typeof(string))
                    {
                        property.SetValue(instance, input);
                    }
                    else if (property.PropertyType.IsEnum)
                    {
                        var values = Enum.GetValues(property.PropertyType);
                        int converted;
                        if (int.TryParse(input, out converted))
                        {
                            foreach (var value in values)
                            {
                                if ((int)value == converted)
                                {
                                    property.SetValue(instance, value);
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        var methods = property.PropertyType.GetMethods();
                        var converterMethod = methods.FirstOrDefault(m => m.Name.Contains("Parse"));
                        if (converterMethod != null)
                        {
                            try
                            {
                                var convertedValue = converterMethod.Invoke(null, new object[] { input });
                                property.SetValue(instance, convertedValue);
                            }
                            catch (Exception exception)
                            {
                                Console.WriteLine("Invalid input value!");
                                Console.ReadKey();
                                Main(new string[] { });
                            }
                        }
                    }
                }
            }
        }

        static void Delete<T>()
        {
            Type type = typeof(T);
            Console.Write($"Enter {type.Name} ID to delete: ");
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
                else if (type == typeof(Grade))
                {
                    gradeLogic.RemoveGrade(id);
                }
                Console.WriteLine($"{type.Name} with ID of {id} was deleted");
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
            Type type = typeof(T);
            Console.Write($"Please enter {type.Name} ID  to list their subjects: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                Main(new string[] { });
            }

            try
            {
                if (type == typeof(Student))
                {
                    var student = personLogic.GetStudent(id);
                    Console.WriteLine($"({student})'s subjects:");
                    List<Subject>(student.RegisteredSubjects);
                }
                else if (type == typeof(Teacher))
                {
                    var teacher = personLogic.GetTeacher(id);
                    Console.WriteLine($"({teacher})'s subjects:");
                    List<Subject>(teacher.RegisteredSubjects);
                }
                
            }
            catch (ObjectNotFoundException excpetion)
            {
                Console.WriteLine(excpetion);
            }
            finally
            {
                Console.ReadKey();
                Main(new string[] { });
            }
        }

        static void GetCourses<T>() where T : IRegistableForCourse
        {
            Type type = typeof(T);
            Console.Write($"Please enter {type.Name} ID  to list their courses: ");
            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                Main(new string[] { });
            }

            try
            {
                if (type == typeof(Student))
                {
                    var student = personLogic.GetStudent(id);
                    Console.WriteLine($"({student})'s courses:");
                    List<Course>(student.RegisteredCourses);
                }
                else if (type == typeof(Teacher))
                {
                    var teacher = personLogic.GetTeacher(id);
                    Console.WriteLine($"({teacher})'s courses:");
                    List<Course>(teacher.RegisteredCourses);
                }

            }
            catch (ObjectNotFoundException excpetion)
            {
                Console.WriteLine(excpetion);
            }
            finally
            {
                Console.ReadKey();
                Main(new string[] { });
            }
        }

        static void RegisterForSubject()
        {
            Console.Write("Please enter Student ID: ");
            int studentId;
            if (!int.TryParse(Console.ReadLine(), out studentId))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                Main(new string[] { });
            }
            Console.Write("Please enter Subject ID: ");
            int subjectId;
            if (!int.TryParse (Console.ReadLine(), out subjectId))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                Main(new string[] { });
            }

            try
            {
                var student = personLogic.GetStudent(studentId);
                var subject = educationLogic.GetSubject(subjectId);

                educationLogic.RegisterStudentForSubject(studentId, subjectId);
                Console.WriteLine($"{student} successfuly registered for subject {subject}");
            }
            catch (ObjectNotFoundException exception)
            {
                Console.WriteLine(exception);
            }
            catch (PreRequirementsNotMetException exception)
            {
                Console.WriteLine($"Couldn't register for subject: {exception}");
            }
            finally
            {
                Console.ReadKey();
                Main(new string[] { });
            }
        }

        static void RegisterForCourse()
        {
            Console.Write("Please enter Student ID: ");
            int studentId;
            if (!int.TryParse(Console.ReadLine(), out studentId))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                Main(new string[] { });
            }
            Console.Write("Please enter Course ID: ");
            int courseId;
            if (!int.TryParse(Console.ReadLine(), out courseId))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                Main(new string[] { });
            }

            try
            {
                var student = personLogic.GetStudent(studentId);
                var course = educationLogic.GetCourse(courseId);

                educationLogic.RegisterStudentForCourse(studentId, courseId);
                Console.WriteLine($"{student} successfuly registered for course {course}");
            }
            catch (ObjectNotFoundException exception)
            {
                Console.WriteLine(exception);
            }
            catch (CourseIsFullException exception)
            {
                Console.WriteLine($"Couldn't register for course: {exception}");
            }
            catch (NotRegisteredForSubjectException exception)
            {
                Console.WriteLine($"Couldn't register for course: {exception}");
            }
            finally
            {
                Console.ReadKey();
                Main(new string[] { });
            }
        }

        static void List<T>(IEnumerable<T> collection)
        {
            var properties = typeof(T).GetProperties();
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


            foreach (var item in collection ?? Enumerable.Empty<T>())
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
        }

        static void GetSchedule<T>() where T : IRegistableForSubject, IRegistableForCourse
        {
            Type type = typeof(T);
            Console.Write($"Please enter {type.Name} ID: ");

            int id;
            if (!int.TryParse(Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                Main(new string[] { });
            }

            try
            {
                if (type == typeof(Student))
                {
                    var student = personLogic.GetStudent(id);
                    string schedule = personLogic.GetSchedule<Student>(id);
                    Console.WriteLine($"({student})'s weekly schedule:");
                    Console.WriteLine(schedule);
                }
                else if (type == typeof(Teacher))
                {
                    var teacher = personLogic.GetTeacher(id);
                    string schedule = personLogic.GetSchedule<Teacher>(id);
                    Console.WriteLine($"({teacher})'s weekly schedule:");
                    Console.WriteLine(schedule);
                }
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

        static void GetBestStudents()
        {
            Console.WriteLine("Students with highest weighted averages:");
            var studentTuples = personLogic.GetBestStudents();
            foreach (var studentTuple in studentTuples)
            {
                Console.WriteLine($"Student: {studentTuple.Item1}, Average: {Math.Round(studentTuple.Item2, 2)}");
            }

            Console.ReadKey();
            Main(new string[] { });
        }

        static void GetBestTeachers()
        {
            Console.WriteLine("Teachers with highest average grades given:");
            var teacherTuples = personLogic.GetBestTeachers();
            foreach (var teacherTuple in teacherTuples)
            {
                Console.WriteLine($"Teacher: {teacherTuple.Item1}, Average: {Math.Round(teacherTuple.Item2, 2)}");
            }

            Console.ReadKey();
            Main(new string[] { });
        }

        static void GetSemesterStatistics()
        {
            Console.Write("Enter semester (Optional): ");
            string semester = Console.ReadLine();
            if (semester.IsNullOrEmpty() )
            {
                var results = gradeLogic.GetSemesterStatistics();
                foreach (var semesterResult in results)
                {
                    Console.WriteLine($"Semster: {semesterResult.Semester}\n\tSuccessful subject completions: {semesterResult.NumberOfPasses}" +
                        $"\n\tFailed subject completions: {semesterResult.NumberOfFailures}\n\tWeighted Avrage ammong all studetns: {Math.Round(semesterResult.WeightedAvg, 2)}");
                }
            }
            else
            {
                var semesterResult = gradeLogic.GetSemesterStatistics(semester);
                Console.WriteLine($"Semster: {semesterResult.Semester}\n\tSuccessful subject completions: {semesterResult.NumberOfPasses}" +
                       $"\n\tFailed subject completions: {semesterResult.NumberOfFailures}\n\tWeighted Avrage ammong all studetns: {Math.Round(semesterResult.WeightedAvg, 2)}");
            }
        }


        static void OldTestingMethod()
        {
            Console.WriteLine("Curriculums:");
            foreach (var curriculum in curriculumLogic.GetCurriculums())
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
