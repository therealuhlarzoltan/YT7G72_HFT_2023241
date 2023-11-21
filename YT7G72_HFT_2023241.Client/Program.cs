using ConsoleTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Client
{
    internal class Program
    {
        private static RestService restService = new RestService("http://localhost:5000");
        const int COLUMN_WIDTH = 16;
        const int SEPARATOR_WIDTH = 1;
        static void Main(string[] args)
        {
            ConsoleMenu menu = null;

            menu = new ConsoleMenu(args, level: 0);
            menu
                .Add("Students", () =>
                {
                    var studentSubmenu = new ConsoleMenu(args, level: 1);
                    studentSubmenu
                        .Add("List", (innerMenu) => { List<Student>(); innerMenu.CloseMenu(); })
                        .Add("Create", (innerMenu) => { Create<Student>(); innerMenu.CloseMenu(); })
                        .Add("Delete", (innerMenu) => { Delete<Student>(); innerMenu.CloseMenu(); })
                        .Add("Update", (innerMenu) => { Update<Student>(); innerMenu.CloseMenu(); })
                        .Add("Get Registered Subjects", (innerMenu) => { GetSubjects<Student>(); innerMenu.CloseMenu(); })
                        .Add("Get Enrolled Courses", (innerMenu) => { GetCourses<Student>(); innerMenu.CloseMenu(); })
                        .Add("Get Weekly Schedule", (innerMenu) => { GetSchedule<Student>(); innerMenu.CloseMenu(); })
                        .Add("Exit", ConsoleMenu.Close)
                        .Show();
                })
                .Add("Teachers", () =>
                {
                    var teacherSubmenu = new ConsoleMenu(args, level: 1);
                    teacherSubmenu
                        .Add("List", (innerMenu) => { List<Teacher>(); innerMenu.CloseMenu(); })
                        .Add("Create", (innerMenu) => { Create<Teacher>(); innerMenu.CloseMenu(); })
                        .Add("Delete", (innerMenu) => { Delete<Teacher>(); innerMenu.CloseMenu(); })
                        .Add("Update", (innerMenu) => { Update<Teacher>(); innerMenu.CloseMenu(); })
                        .Add("Get Taught Subjects", (innerMenu) => { GetSubjects<Teacher>(); innerMenu.CloseMenu(); })
                        .Add("Get Taught Courses", (innerMenu) => { GetCourses<Teacher>(); innerMenu.CloseMenu(); })
                        .Add("Get Weekly Schedule", (innerMenu) => { GetSchedule<Teacher>(); innerMenu.CloseMenu(); })
                        .Add("Exit", ConsoleMenu.Close)
                        .Show();
                })
                .Add("Subjects", () => 
                { 
                    var subjectSubmenu = new ConsoleMenu(args, level: 1);
                    subjectSubmenu
                        .Add("List", (innerMenu) => { List<Subject>(); innerMenu.CloseMenu(); })
                        .Add("Create", (innerMenu) => { Create<Subject>(); innerMenu.CloseMenu(); })
                        .Add("Delete", (innerMenu) => { Delete<Subject>(); innerMenu.CloseMenu(); })
                        .Add("Update", (innerMenu) => { Update<Subject>(); innerMenu.CloseMenu(); })
                        .Add("Exit", ConsoleMenu.Close)
                        .Show();

                })
                .Add("Courses", () =>
                {
                    var courseSubmenu = new ConsoleMenu(args, level: 1);
                    courseSubmenu
                    .Add("List", (innerMenu) => { List<Course>(); innerMenu.CloseMenu(); })
                    .Add("Create", (innerMenu) => { Create<Course>(); innerMenu.CloseMenu(); })
                    .Add("Delete", (innerMenu) => { Delete<Course>(); innerMenu.CloseMenu(); })
                    .Add("Update", (innerMenu) => { Update<Course>(); innerMenu.CloseMenu(); })
                    .Add("Exit", ConsoleMenu.Close)
                    .Show();
                })
                .Add("Grades", () => 
                {
                    var gradeSubmenu = new ConsoleMenu(args, level: 1);
                    gradeSubmenu
                    .Add("List", (innerMenu) => { List<Grade>(); innerMenu.CloseMenu(); })
                    .Add("Create", (innerMenu) => {  Create<Grade>(); innerMenu.CloseMenu(); })
                    .Add("Delete", (innerMenu) => { Delete<Grade>(); innerMenu.CloseMenu(); })
                    .Add("Update", (innerMenu) => { Update<Grade>(); innerMenu.CloseMenu(); })
                    .Add("Get Best Students", (innerMenu) => { GetBestStudents(); innerMenu.CloseMenu(); })
                    .Add("Get Best Teachers", (innerMenu) => { GetBestTeachers(); innerMenu.CloseMenu(); })
                    .Add("Get Best Teachers By Academic Rank", (innerMenu) => { GetBestTeachersByAcademicRank(); innerMenu.CloseMenu(); })
                    .Add("Get Subject Statistics", (innerMenu) => { GetSubjectStatistics(); innerMenu.CloseMenu(); })
                    .Add("Get Semester Statistics", (innerMenu) => { GetSemesterStatistics(); innerMenu.CloseMenu(); })
                    .Add("Exit", ConsoleMenu.Close)
                    .Show();
                })
                .Add("Curriculums", () => 
                {
                    var curriculumSubmenu = new ConsoleMenu(args, level: 1);
                    curriculumSubmenu
                        .Add("List", (innerMenu) => { List<Curriculum>(); innerMenu.CloseMenu(); })
                        .Add("Create", (innerMenu) => { Create<Curriculum>(); innerMenu.CloseMenu(); })
                        .Add("Update", (innerMenu) => { Update<Curriculum>(); innerMenu.CloseMenu(); })
                        .Add("Delete", (innerMenu) => { Delete<Curriculum>(); innerMenu.CloseMenu(); })
                        .Add("Reset Semester", (innerMenu) => {ResetSemester(); innerMenu.CloseMenu(); })
                        .Add("Exit", ConsoleMenu.Close)
                        .Show();
                })
                .Add("Subject Registration", () => 
                {
                    var subjectRegistrationSubmenu = new ConsoleMenu(args, level: 1);
                    subjectRegistrationSubmenu
                        .Add("Register for Subject", (innerMenu) => { RegisterForSubject(); innerMenu.CloseMenu(); })
                        .Add("Unregister from Subject", (innerMenu) => { UnregisterFromSubject(); innerMenu.CloseMenu(); })
                        .Add("Exit", ConsoleMenu.Close)
                        .Show();
                })
                .Add("Course Registration", () => 
                {
                    var courseRegistrationSubmenu = new ConsoleMenu(args, level: 1);
                    courseRegistrationSubmenu
                        .Add("Register for Course", (innerMenu) => { RegisterForCourse(); innerMenu.CloseMenu(); })
                        .Add("Unregister from Course", (innerMenu) => { UnregisterFromCourse(); innerMenu.CloseMenu(); })
                        .Add("Exit", ConsoleMenu.Close)
                        .Show();
                })
                .Add("Exit", ConsoleMenu.Close)
                .Show();
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
        }

        

        static void Create<T>()
        {
            Type type = typeof(T);
            Console.WriteLine($"Creating new {type.Name} entity...");

            try
            {
                T entity = CreateInstance<T>();
                if (type == typeof(Student))
                {
                    var curriculum = restService.GetSingle<Curriculum>($"/Curriculums/{(entity as Student).CurriculumId}");
                    restService.Post<Student>("/People/Students", entity as Student);
                }
                else if (type == typeof(Teacher))
                {
                    restService.Post<Teacher>("/People/Teachers", entity as Teacher);
                }
                else if (type == typeof(Subject))
                {
                    if ((entity as Subject).TeacherId != null)
                    {
                        var teacher = restService.GetSingle<Teacher>($"/People/Teachers/{(entity as Subject).TeacherId}");
                    }
                    if ((entity as Subject).PreRequirementId != null)
                    {
                        var preReq = restService.GetSingle<Subject>($"/Education/Subjects/{(entity as Subject).PreRequirementId}");
                    }
                    var curriculum = restService.GetSingle<Curriculum>($"/Curriculums/{(entity as Subject).CurriculumId}");
                    restService.Post<Subject>("/Education/Subjects", entity as Subject);
                }
                else if (type == typeof(Course))
                {
                    if ((entity as Course).TeacherId != null)
                    {
                        var teacher = restService.GetSingle<Teacher>($"/People/Teachers/{(entity as Course).TeacherId}");
                    }
                    var subject = restService.GetSingle<Subject>($"/Education/Subjects/{(entity as Course).SubjectId}");
                    restService.Post<Course>("/Education/Courses", entity as Course);
                }
                else if (type == typeof(Curriculum))
                {
                    restService.Post<Curriculum>("/Curriculums", entity as Curriculum);
                }
                else if (type == typeof(Grade))
                {
                    var student = restService.GetSingle<Subject>($"/People/Students/{(entity as Grade).StudentId}");
                    var subject = restService.GetSingle<Subject>($"/Education/Subjects/{(entity as Grade).SubjectId}");
                    var teacher = restService.GetSingle<Teacher>($"/People/Teachers/{(entity as Grade).TeacherId}");
                    restService.Post<Grade>("/Grades", entity as Grade);
                }
                Console.WriteLine($"New {type.Name} created!");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Failed to create new {type.Name} enitity -- {exception.Message}");
            }
            finally
            {
                Console.ReadKey();
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
                return;
            }
            Console.WriteLine($"Updating {type.Name} entity...");
            try
            {
                if (type == typeof(Student))
                {
                    instance = restService.Get<Student>("/People/Students", id);
                    UpdateInstance<T>(instance as T);
                    var curriculum = restService.GetSingle<Curriculum>($"/Curriculums/{(instance as Student).CurriculumId}");
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
                    if ((instance as Subject).TeacherId != null)
                    {
                        var teacher = restService.GetSingle<Teacher>($"/People/Teachers/{(instance as Subject).TeacherId}");
                    }
                    if ((instance as Subject).PreRequirementId != null)
                    {
                        var preReq = restService.GetSingle<Subject>($"/Education/Subjects/{(instance as Subject).PreRequirementId}");
                    }
                    var curriculum = restService.GetSingle<Curriculum>($"/Curriculums/{(instance as Subject).CurriculumId}");
                    restService.Put<Subject>("/Education/Subjects", instance as Subject);
                }
                else if (type == typeof(Course))
                {
                    instance = restService.Get<Course>("/Education/Courses", id);
                    UpdateInstance<T>(instance as T);
                    if ((instance as Course).TeacherId != null)
                    {
                        var teacher = restService.GetSingle<Teacher>($"/People/Teachers/{(instance as Course).TeacherId}");
                    }
                    var subject = restService.GetSingle<Subject>($"/Education/Subjects/{(instance as Course).SubjectId}");
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
                    var student = restService.GetSingle<Subject>($"/People/Students/{(instance as Grade).StudentId}");
                    var subject = restService.GetSingle<Subject>($"/Education/Subjects/{(instance as Grade).SubjectId}");
                    var teacher = restService.GetSingle<Teacher>($"/People/Teachers/{(instance as Grade).TeacherId}");
                    restService.Put<Grade>("/Grades", instance as Grade);
                }
                
                Console.WriteLine("Entity updated!");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Failed to update {type.Name} entity -- {exception.Message}");
            }
            finally
            {
                Console.ReadKey();
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
                return;
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
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            } 
            finally
            {
                Console.ReadKey();
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
                return;
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
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                Console.ReadKey();
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
                return;
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
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                Console.ReadKey();
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
                return;
            }
            Console.Write("Please enter Subject ID: ");
            int subjectId;
            if (!int.TryParse (Console.ReadLine(), out subjectId))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                return;
            }

            try
            {
                var student = restService.Get<Student>("/People/Students", studentId);
                var subject = restService.Get<Subject>("/Education/Subjects", subjectId);

                restService.Post($"/Education/Subjects/{subjectId}/Register/{studentId}");
                Console.WriteLine($"{student} successfuly registered for subject {subject}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Couldn't register for subject: {exception.Message}");
            }
            finally
            {
                Console.ReadKey();
            }
        }

        static void UnregisterFromSubject()
        {
            Console.Write("Please enter Student ID: ");
            int studentId;
            if (!int.TryParse(Console.ReadLine(), out studentId))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                return;
            }
            Console.Write("Please enter Subject ID: ");
            int subjectId;
            if (!int.TryParse(Console.ReadLine(), out subjectId))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                return;
            }

            try
            {
                var student = restService.Get<Student>("/People/Students", studentId);
                var subject = restService.Get<Subject>("/Education/Subjects", subjectId);

                restService.Delete($"/Education/Subjects/{subjectId}/Register/{studentId}");
                Console.WriteLine($"{student} successfuly unregistered from subject {subject}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Couldn't unregister from subject: {exception.Message}");
            }
            finally
            {
                Console.ReadKey();
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
                return;
            }
            Console.Write("Please enter Course ID: ");
            int courseId;
            if (!int.TryParse(Console.ReadLine(), out courseId))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                return; 
            }

            try
            {
                var student = restService.Get<Student>("/People/Students", studentId);
                var course = restService.Get<Course>("/Education/Courses", courseId);

                restService.Post($"/Education/Courses/{courseId}/Register/{studentId}");
                Console.WriteLine($"{student} successfuly registered for course {course}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Couldn't register for course: {exception.Message}");
            }
            finally
            {
                Console.ReadKey();
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
                return;
            }
            Console.Write("Please enter Course ID: ");
            int courseId;
            if (!int.TryParse(Console.ReadLine(), out courseId))
            {
                Console.WriteLine("Invalid ID provided");
                Console.ReadKey();
                return;
            }

            try
            {
                var student = restService.Get<Student>("/People/Students", studentId);
                var course = restService.Get<Course>("/Education/Courses", courseId);

                restService.Delete($"/Education/Courses/{courseId}/Register/{studentId}");
                Console.WriteLine($"{student} successfuly unregistered from course {course}");
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Couldn't unregister from course: {exception.Message}");
            }
            finally
            {
                Console.ReadKey();
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
                return;
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
            catch (Exception exception)
            {
                Console.WriteLine(exception.Message);
            }
            finally
            {
                Console.ReadKey();
            }
        }

        static void ResetSemester()
        {
            try
            {
                restService.Post("/Education/Semester/Reset");
                Console.WriteLine("All Course and Subject Registrations have been celared!");
            }
            catch
            {
                Console.WriteLine("Failed to reset semester");
            }
            finally
            {
                Console.ReadKey();
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
                return;
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
                AcademicRank academicRank = (AcademicRank)intValue;
                Console.WriteLine($"{academicRank}s who gave the higest grades on average:");
                var teacherDTOs = restService.Get<AverageByPersonDTO<Teacher>>($"/People/Teachers/Best/{intValue}");
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
        }

        static void GetSemesterStatistics()
        {
            Console.Write("Enter semester (Optional): ");
            string semester = Console.ReadLine();
            if (string.IsNullOrEmpty(semester) )
            {
                var results = restService.Get<SemesterStatistics>("/Grades/Semester/Statistics");
                foreach (var semesterResult in results)
                {
                    Console.WriteLine($"Semster: {semesterResult.Semester}\n\tSuccessful subject completions: {semesterResult.NumberOfPasses}" +
                        $"\n\tNumber of failures: {semesterResult.NumberOfFailures}\n\tWeighted Average among all studetns: {Math.Round(semesterResult.WeightedAvg, 2)}");
                }
            }
            else
            {
                var semesterResult = restService.GetSingle<SemesterStatistics>($"/Grades/Semester/Statistics/{semester}");
                Console.WriteLine($"Semster: {semesterResult.Semester}\n\tSuccessful subject completions: {semesterResult.NumberOfPasses}" +
                       $"\n\tNumber of failures: {semesterResult.NumberOfFailures}\n\tWeighted Average among all studetns: {Math.Round(semesterResult.WeightedAvg, 2)}");
            }
            Console.ReadKey();
        }

        static void GetSubjectStatistics()
        {
            Console.Write("Please enter Subject ID: ");
            int id;
            if (!int.TryParse( Console.ReadLine(), out id))
            {
                Console.WriteLine("Invalid ID provided!");
                Console.ReadKey();;
                return;
            }
            else
            {
                var subjectStat = restService.GetSingle<SubjectStatistics>($"/Grades/Subjects/Statistics/{id}");
                Console.WriteLine($"Subject: {subjectStat.Subject}\n\tNumber of total registrations: {subjectStat.NumberOfRegistrations}\n\tPass per Registration Ratio: {subjectStat.PassPerRegistrationRatio}\n\tAverages grade given: {subjectStat.Avg}");
                Console.ReadKey();
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
                if (property.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    if (property.GetCustomAttribute<FormatAttribute>() != null)
                        Console.Write($"{property.Name} ({property.GetCustomAttribute<FormatAttribute>().FormatDescription}) (Required): ");
                    else
                        Console.Write($"{property.Name} (Required): ");
                }
                else
                {
                    if (property.GetCustomAttribute<FormatAttribute>() != null)
                        Console.Write($"{property.Name} ({property.GetCustomAttribute<FormatAttribute>().FormatDescription}): ");
                    else
                        Console.Write($"{property.Name}: ");
                }

                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
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
                            if (!int.TryParse(input, out converted))
                            {
                                throw new ArgumentException("Invalid input value!");
                            }
                            else
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
                                    throw new ArgumentException("Invalid input value!");
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                    {
                        property.SetValue(instance, null);
                    }
                    else
                    {
                        property.SetValue(instance, property.PropertyType.IsValueType ? Activator.CreateInstance(property.PropertyType) : null);
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
                if (property.GetCustomAttribute<RequiredAttribute>() != null)
                {
                    if (property.GetCustomAttribute<FormatAttribute>() != null)
                        Console.Write($"{property.Name} ({property.GetCustomAttribute<FormatAttribute>().FormatDescription}) (Required): ");
                    else
                        Console.Write($"{property.Name} (Required): ");
                }
                else
                {
                    if ( property.GetCustomAttribute<FormatAttribute>() != null)
                        Console.Write($"{property.Name} ({property.GetCustomAttribute<FormatAttribute>().FormatDescription}): ");
                    else
                        Console.Write($"{property.Name}: ");
                }

                string input = Console.ReadLine();
                if (!string.IsNullOrEmpty(input))
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
                                    throw new ArgumentException("Invalid input value!");
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (Nullable.GetUnderlyingType(property.PropertyType) != null)
                    {
                        property.SetValue(instance, null);
                    }
                    else
                    {
                        throw new ArgumentException("Invalid input value!");
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
