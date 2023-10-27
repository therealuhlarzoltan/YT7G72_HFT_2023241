using Castle.Core.Internal;
using ConsoleTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using YT7G72_HFT_2023241.Logic;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Client
{
    internal class Program
    {
        private static RestService restService = new RestService("http://localhost:4180");
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
            .Add("Get Best Teachers By Academic Rank", () => { menu.CloseMenu(); gradeSubmenu.CloseMenu(); GetBestTeachersByAcademicRank(); })
            .Add("Get Subject Statistics", () => { menu.CloseMenu(); gradeSubmenu.CloseMenu(); GetSubjectStatistics(); })
            .Add("Get Semester Statistics", () => { menu.CloseMenu(); gradeSubmenu.CloseMenu(); GetSemesterStatistics(); })
            .Add("Exit", ConsoleMenu.Close);

            var curriculumSubmenu = new ConsoleMenu(args, level: 1);
            curriculumSubmenu
                .Add("List", () => { menu.CloseMenu(); gradeSubmenu.CloseMenu(); List<Curriculum>(); })
                .Add("Create", () => { menu.CloseMenu(); gradeSubmenu.CloseMenu(); Create<Curriculum>(); })
                .Add("Update", () => { menu.CloseMenu(); gradeSubmenu.CloseMenu(); Update<Curriculum>(); })
                .Add("Delete", () => { menu.CloseMenu(); gradeSubmenu.CloseMenu(); Delete<Curriculum>(); });

            var subjectRegistrationSubmenu = new ConsoleMenu(args, level: 1);
            subjectRegistrationSubmenu
                .Add("Register for Subject", () => RegisterForSubject())
                .Add("Unregister from Subject", () => UnregisterFromSubject());

            var courseRegistrationSubmenu = new ConsoleMenu(args, level: 1);
            courseRegistrationSubmenu
                .Add("Register for Course", () => RegisterForCourse())
                .Add("Unregister from Course", () => UnregisterFromCourse());

            menu = new ConsoleMenu(args, level: 0);
            menu
            .Add("Students", () => studentSubmenu.Show())
            .Add("Teachers", () => teacherSubmenu.Show())
            .Add("Subjects", () => subjectSubmenu.Show())
            .Add("Courses", () => courseSubmenu.Show())
            .Add("Grades", () => gradeSubmenu.Show())
            .Add("Curriculums", () => curriculumSubmenu.Show())
            .Add("Subject Registration", () => subjectRegistrationSubmenu.Show())
            .Add("Course Registration", () => courseRegistrationSubmenu.Show())
            .Add("Exit", ConsoleMenu.Close);

            menu.Show();
        }

        #region Generic CRUD Methods

        static void List<T>() where T : class
        {
            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties();
            IEnumerable<object> querySet = null;
            

            if (type == typeof(Student))
            {
                querySet = restService.Get<Student>("/People/Students");
            }
            else if (type == typeof(Teacher))
            {
                querySet = restService.Get<Teacher>("/People/Teachers");
            }
            else if (type == typeof(Subject))
            {
                querySet = restService.Get<Subject>("/Education/Subjects");
            }
            else if (type == typeof(Course))
            {
                querySet = restService.Get<Course>("/Education/Courses");
            }
            else if (type == typeof(Curriculum))
            {
                querySet = restService.Get<Curriculum>("/Curriculums");
            }
            else if (type == typeof(Grade))
            {
                querySet = restService.Get<Grade>("/Grades");
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
            Type type = typeof(T);
            Console.WriteLine($"Creating new {type.Name} entity...");
            T entity = CreateInstance<T>();

            try
            {
                if (type == typeof(Student))
                {
                    restService.Post<Student>("/People/Students", entity as Student);
                }
                else if (type == typeof(Teacher))
                {
                    restService.Post<Teacher>("/People/Teachers", entity as Teacher);
                }
                else if (type == typeof(Subject))
                {
                    restService.Post<Subject>("/Education/Subjects", entity as Subject);
                }
                else if (type == typeof(Course))
                {
                    restService.Post<Course>("/Education/Courses", entity as Course);
                }
                else if (type == typeof(Curriculum))
                {
                    restService.Post<Curriculum>("/Curriculums", entity as Curriculum);
                }
                else if (type == typeof(Grade))
                {
                    restService.Post<Grade>("/Grades", entity as Grade);
                }
                Console.WriteLine($"New {type.Name} created!");
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine($"Failed to create new {type.Name} enitity -- {exception.Message}");
            }
            finally
            {
                Console.ReadKey();
                Main(new string[] { });
            }

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
            Console.WriteLine("Updating entity...");
            try
            {
                if (type == typeof(Student))
                {
                    instance = restService.Get<Student>("/People/Students", id);
                    UpdateInstance<T>(instance as T);
                    restService.Put<Student>("/People/Students", instance as Student);
                }
                else if (type == typeof(Teacher))
                {
                    instance = restService.Get<Teacher>("/People/Teachers", id);
                    UpdateInstance<T>(instance as T);
                    restService.Put<Teacher>("/People/Teachers", instance as Teacher);
                }
                else if (type == typeof(Subject))
                {
                    instance = restService.Get<Subject>("/Education/Subjects", id);
                    UpdateInstance<T>(instance as T);
                    restService.Put<Subject>("/Education/Subjects", instance as Subject);
                }
                else if (type == typeof(Course))
                {
                    instance = restService.Get<Course>("/Education/Courses", id);
                    UpdateInstance<T>(instance as T);
                    restService.Put<Course>("/Education/Courses", instance as Course);
                }
                else if (type == typeof(Curriculum))
                {
                    instance = restService.Get<Curriculum>("/Curriculums", id);
                    UpdateInstance<T>(instance as T);
                    restService.Put<Curriculum>("/Curriculums", instance as Curriculum);
                }
                else if (type == typeof(Grade))
                {
                    instance = restService.Get<Grade>("/Grades", id);
                    UpdateInstance<T>(instance as T);
                    restService.Put<Grade>("/Grades", instance as Grade);
                }
                
                Console.WriteLine("Entity updated!");
            }
            catch (ObjectNotFoundException exception)
            {
                Console.WriteLine(exception);
            }
            catch (ArgumentException exception)
            {
                Console.WriteLine($"Failed to update {type.Name} entity -- {exception.Message}");
            }
            finally
            {
                Console.ReadKey();
                Main(new string[] { });
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
                    restService.Delete("/People/Students", id);
                }
                else if (type == typeof(Teacher))
                {
                    restService.Delete("/People/Teachers", id);
                }
                else if (type == typeof(Subject))
                {
                    restService.Delete("/Education/Subjects", id);
                }
                else if (type == typeof(Course))
                {
                    restService.Delete("/Education/Courses", id);
                }
                else if (type == typeof(Grade))
                {
                    restService.Delete("/Grades", id);
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

        #endregion

        #region Advanced CRUD Methods

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
                    var student = restService.Get<Student>("/People/Students", id);
                    Console.WriteLine($"({student})'s subjects:");
                    List<Subject>(student.RegisteredSubjects);
                }
                else if (type == typeof(Teacher))
                {
                    var teacher = restService.Get<Teacher>("/People/Teachers", id);
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
                    var student = restService.Get<Student>("/People/Students", id);
                    Console.WriteLine($"({student})'s courses:");
                    List<Course>(student.RegisteredCourses);
                }
                else if (type == typeof(Teacher))
                {
                    var teacher = restService.Get<Teacher>("/People/Teachers", id);
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
                var student = restService.Get<Student>("/People/Students", studentId);
                var subject = restService.Get<Subject>("/Education/Subjects", subjectId);

                restService.Post($"/Education/Subjects/{subjectId}/Register/{studentId}");
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

        static void UnregisterFromSubject()
        {
            var exc = new ArgumentException("Invalid lsdjlsd");
            Console.WriteLine(exc);
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
            if (!int.TryParse(Console.ReadLine(), out subjectId))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                Main(new string[] { });
            }

            try
            {
                var student = restService.Get<Student>("/People/Students", studentId);
                var subject = restService.Get<Subject>("/Education/Subjects", subjectId);

                restService.Delete($"/Education/Subjects/{subjectId}/Register/{studentId}");
                Console.WriteLine($"{student} successfuly unregistered from subject {subject}");
            }
            catch (ObjectNotFoundException exception)
            {
                Console.WriteLine(exception);
            }
            catch (PreRequirementsNotMetException exception)
            {
                Console.WriteLine($"Couldn't unregister from subject: {exception}");
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
                var student = restService.Get<Student>("/People/Students", studentId);
                var course = restService.Get<Course>("/Education/Courses", courseId);

                restService.Post($"/Education/Courses/{courseId}/Register/{studentId}");
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

        static void UnregisterFromCourse()
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
                var student = restService.Get<Student>("/People/Students", studentId);
                var course = restService.Get<Course>("/Education/Courses", courseId);

                restService.Delete($"/Education/Courses/{courseId}/Register/{studentId}");
                Console.WriteLine($"{student} successfuly unregistered from course {course}");
            }
            catch (ObjectNotFoundException exception)
            {
                Console.WriteLine(exception);
            }
            catch (CourseIsFullException exception)
            {
                Console.WriteLine($"Couldn't unregister from course: {exception}");
            }
            catch (NotRegisteredForSubjectException exception)
            {
                Console.WriteLine($"Couldn't unregister from course: {exception}");
            }
            finally
            {
                Console.ReadKey();
                Main(new string[] { });
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
                    var student = restService.Get<Student>("/People/Students", id);
                    string schedule = restService.Get<string>("/People/Students/Schedule", id);
                    Console.WriteLine($"({student})'s weekly schedule:");
                    Console.WriteLine(schedule);
                }
                else if (type == typeof(Teacher))
                {
                    var teacher = restService.Get<Teacher>("/People/Teachers", id);
                    string schedule = restService.Get<string>("/People/Teachers/Schedule", id);
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
        #endregion

        #region non-CRUD Methods
        static void GetBestStudents()
        {
            Console.WriteLine("Students with highest weighted averages:");
            var studentDTOs = restService.Get<AverageByPersonDTO<Student>>("/People/Students/Best");
            foreach (var dto in studentDTOs)
            {
                Console.WriteLine(dto);
            }

            Console.ReadKey();
            Main(new string[] { });
        }

        static void GetBestTeachers()
        {
            Console.WriteLine("Teachers with highest average grades given:");
            var teacherDTOs = restService.Get<AverageByPersonDTO<Teacher>>("/People/Teachers/Best");
            foreach (var dto in teacherDTOs)
            {
                Console.WriteLine(dto);
            }

            Console.ReadKey();
            Main(new string[] { });
        }

        static void GetBestTeachersByAcademicRank()
        {
            Console.WriteLine("Available choices for Academic Rank");
            foreach (var value in Enum.GetValues(typeof(AcademicRank)))
            {
                Console.WriteLine($"{(int)value} -- {value}");
            }
            Console.Write("Selected Academic Rank: ");
            string input = Console.ReadLine();
            int intValue;
            if (!int.TryParse(input, out intValue))
            {
                Console.WriteLine("Invalid value!");
                Console.ReadKey();
                Main(new string[] { });
            }

            bool isValid = false;
            foreach (var value in Enum.GetValues(typeof(AcademicRank)))
            {
                if (intValue == (int)value)
                {
                    isValid = true;
                    break;
                }
            }

            if (isValid)
            {
                Console.WriteLine("Teachers who gave the higest grades on average::");
                var teacherDTOs = restService.Get<AverageByPersonDTO<Teacher>>("/People/Teachers/Best");
                foreach (var dto in teacherDTOs)
                {
                    Console.WriteLine(dto);
                }
            }
            else
            {
                Console.WriteLine("Invalid value!");
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
                var results = restService.Get<SemesterStatistics>("/Grades/Semester/Statistics");
                foreach (var semesterResult in results)
                {
                    Console.WriteLine($"Semster: {semesterResult.Semester}\n\tSuccessful subject completions: {semesterResult.NumberOfPasses}" +
                        $"\n\tFailed subject completions: {semesterResult.NumberOfFailures}\n\tWeighted Avrage ammong all studetns: {Math.Round(semesterResult.WeightedAvg, 2)}");
                }
            }
            else
            {
                var semesterResult = restService.GetSingle<SemesterStatistics>($"/Grades/Semester/Statistics/{semester}");
                Console.WriteLine($"Semster: {semesterResult.Semester}\n\tSuccessful subject completions: {semesterResult.NumberOfPasses}" +
                       $"\n\tFailed subject completions: {semesterResult.NumberOfFailures}\n\tWeighted Avrage ammong all studetns: {Math.Round(semesterResult.WeightedAvg, 2)}");
            }
        }

        static void GetSubjectStatistics()
        {
            Console.Write("Please enter Subject ID: ");
            int id;
            if (!int.TryParse( Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID provided!");
                Console.ReadKey();
                Main(new string[] { });
            }
            else
            {
                var subjectStat = restService.GetSingle<SubjectStatistics>($"/Grades/Subjects/Statistics/{id}");
                Console.WriteLine($"Subject: {subjectStat.Subject}\n\tNumber of total registrations: {subjectStat.NumberOfRegistrations}\n\tPass per Registration Ratio: {subjectStat.PassPerRegistrationRatio}\n\tAverages grade given: {subjectStat.Avg}");
                Console.ReadKey();
                Main(new string[] { });
            }
        }
        #endregion

        #region Helper Methods

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
                    if (property.GetAttribute<FormatAttribute>() != null)
                        Console.Write($"{property.Name} ({property.GetAttribute<FormatAttribute>().FormatDescription}) (Required): ");
                    else
                        Console.Write($"{property.Name} (Required): ");
                }
                else
                {
                    if (property.GetAttribute<FormatAttribute>() != null)
                        Console.Write($"{property.Name} ({property.GetAttribute<FormatAttribute>().FormatDescription}): ");
                    else
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
                    if (property.GetAttribute<FormatAttribute>() != null)
                        Console.Write($"{property.Name} ({property.GetAttribute<FormatAttribute>().FormatDescription}) (Required): ");
                    else
                        Console.Write($"{property.Name} (Required): ");
                }
                else
                {
                    if ( property.GetAttribute<FormatAttribute>() != null)
                        Console.Write($"{property.Name} ({property.GetAttribute<FormatAttribute>().FormatDescription}): ");
                    else
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
                        if (property.PropertyType == typeof(int?))
                        {
                            int converted;
                            if (int.TryParse(input, out converted))
                            {
                                property.SetValue(instance, converted);
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
            }
            return (T)instance;
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
        #endregion
    }
}
