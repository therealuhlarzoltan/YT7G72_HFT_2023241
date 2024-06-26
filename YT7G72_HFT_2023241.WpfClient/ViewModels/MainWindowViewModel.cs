﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using YT7G72_HFT_2023241.WpfClient.Logic;
using YT7G72_HFT_2023241.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.Pkcs;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using YT7G72_HFT_2023241.WpfClient.Services.Interfaces;
using YT7G72_HFT_2023241.WpfClient.Windows;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient
    {
        #region Private Fields
        private IStudentCreator studentCreator;
        private IStudentEditor studentEditor;
        private ITeacherCreator teacherCreator;
        private ITeacherEditor teacherEditor;
        private ISemesterStatisticsDisplay semesterStatisticsDisplay;
        private ITeacherStatisticsDisplay teacherStatisticsDisplay;
        private IStudentStatisticsDisplay studentStatisticsDisplay;
        private ISubjectStatisticsDisplay subjectStatisticsDisplay;
        private IMessageBoxService messageBoxService;
        private RestService restService = new RestService("http://localhost:4180/", "");
        private bool isStudentCreating;
        private bool isStudentUpdating;
        private bool isTeacherUpdating;
        private bool isTeacherCreating;
        private bool isSubjectUpdating;
        private bool isSubjectCreating;
        private bool isCourseUpdating;
        private bool isCourseCreating;
        private bool isGradeCreating;
        private bool isGradeUpdating;
        private bool isCurriculumUpdating;
        private bool isCurriculumCreating;
        private string errorMessage;
        private Student selectedStudent;
        private Teacher selectedTeacher;
        private Subject selectedSubject;
        private Course selectedCourse;
        private Grade selectedGrade;
        private Curriculum selectedCurriculum;

        #endregion Private Fields

        #region Public Properites

        public RestCollection<Student> Students { get; set; }
        public RestCollection<Teacher> Teachers { get; set; }
        public RestCollection<Subject> Subjects { get; set; }
        public RestCollection<Course> Courses { get; set; }
        public RestCollection<Grade> Grades { get; set; }
        public RestCollection<Curriculum> Curriculums { get; set; }
        public Student SelectedStudent
        {
            get
            {
                return selectedStudent;
            }
            set { 
                SetProperty(ref selectedStudent, value);
                (UpdateStudentCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (DeleteStudentCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (RegisterForSubjectCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (UnregisterFromSubjectCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (RegisterForCourseCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (UnregisterFromCourseCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (GetStudentScheduleCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public Teacher SelectedTeacher
        {
            get { return selectedTeacher; } set
            {
                SetProperty(ref selectedTeacher, value);
                (UpdateTeacherCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (DeleteTeacherCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (GetTeacherScheduleCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }


        public Course SelectedCourse
        {
            get { return selectedCourse; }
            set
            {
                SetProperty(ref selectedCourse, value);
                (UpdateCourseCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (DeleteCourseCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (RegisterForCourseCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (UnregisterFromCourseCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public Subject SelectedSubject
        {
            get { return selectedSubject; }
            set
            {
                SetProperty(ref selectedSubject, value);
                (UpdateSubjectCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (DeleteSubjectCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (RegisterForSubjectCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (UnregisterFromSubjectCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (GetSubjectStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();

            }
        }

        public Grade SelectedGrade
        {
            get { return selectedGrade; }
            set
            {
                SetProperty(ref selectedGrade, value);
                (UpdateGradeCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (DeleteGradeCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public Curriculum SelectedCurriculum
        {
            get { return selectedCurriculum; }
            set
            {
                SetProperty(ref selectedCurriculum, value);
                (UpdateCurriculumCommand as RelayCommand)?.NotifyCanExecuteChanged();
                (DeleteCurriculumCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsStudentCreating
        {
            get { return isStudentCreating; }
            set
            {
                isStudentCreating = value;
                (CreateStudentCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsTeacherCreating
        {
            get { return isTeacherCreating; }
            set
            {
                isTeacherCreating = value;
                (CreateTeacherCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsSubjectCreating
        {
            get { return isSubjectCreating; }
            set
            {
                isSubjectCreating = value;
                (CreateSubjectCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsGradeCreating
        {
            get { return isGradeCreating; }
            set
            {
                isGradeCreating = value;
                (CreateGradeCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsCourseCreating
        {
            get { return isCourseCreating; }
            set
            {
                isCourseCreating = value;
                (CreateCourseCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsCurriculumCreating
        {
            get { return isCurriculumCreating; }
            set
            {
                isCurriculumCreating = value;
                (CreateCurriculumCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsStudentUpdating
        {
            get { return isStudentUpdating; }
            set
            {
                isStudentUpdating = value;
                (UpdateStudentCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsTeacherUpdating
        {
            get { return isTeacherUpdating; }
            set
            {
                isTeacherUpdating = value;
                (UpdateTeacherCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsSubjectUpdating
        {
            get { return isSubjectUpdating; }
            set
            {
                isSubjectUpdating = value;
                (UpdateSubjectCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsGradeUpdating
        {
            get { return isGradeUpdating; }
            set
            {
                isGradeUpdating = value;
                (UpdateGradeCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsCourseUpdating
        {
            get { return isCourseUpdating; }
            set
            {
                isCourseUpdating = value;
                (UpdateCourseCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public bool IsCurriculumUpdating
        {
            get { return isCurriculumUpdating; }
            set
            {
                isCurriculumUpdating = value;
                (UpdateCurriculumCommand as RelayCommand)?.NotifyCanExecuteChanged();
            }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
            }
        }

        #endregion Public Properties

        #region Command Declarations
        public ICommand CreateStudentCommand { get; set; }
        public ICommand UpdateStudentCommand { get; set; }

        public ICommand DeleteStudentCommand { get; set; }
        public ICommand CreateTeacherCommand { get; set; }
        public ICommand UpdateTeacherCommand { get; set; }

        public ICommand DeleteTeacherCommand { get; set; }
        public ICommand CreateSubjectCommand { get; set; }
        public ICommand UpdateSubjectCommand { get; set; }

        public ICommand DeleteSubjectCommand { get; set; }
        public ICommand CreateCourseCommand { get; set; }
        public ICommand UpdateCourseCommand { get; set; }

        public ICommand DeleteCourseCommand { get; set; }
        public ICommand CreateGradeCommand { get; set; }
        public ICommand UpdateGradeCommand { get; set; }

        public ICommand DeleteGradeCommand { get; set; }
        public ICommand CreateCurriculumCommand { get; set; }
        public ICommand UpdateCurriculumCommand { get; set; }

        public ICommand DeleteCurriculumCommand { get; set; }
        public ICommand RegisterForCourseCommand { get; set; }
        public ICommand RegisterForSubjectCommand { get; set; }
        public ICommand UnregisterFromCourseCommand { get; set; }
        public ICommand UnregisterFromSubjectCommand { get; set; }
        public ICommand GetTeacherScheduleCommand { get; set; }
        public ICommand GetStudentScheduleCommand { get; set; }
        public ICommand ResetSemesterCommand { get; set; }
        public ICommand GetSubjectStatisticsCommand { get; set; }
        public ICommand ToSemesterStatisticsCommand { get; set; }
        public ICommand ToTeacherStatisticsCommand { get; set; }
        public ICommand ShowBestStudentsCommand { get; set; }

        #endregion Command Declarations



         public MainWindowViewModel() : this(IsInDesignMode ? null : Ioc.Default.GetService<IStudentCreator>(),
             IsInDesignMode ? null : Ioc.Default.GetService<IStudentEditor>(),
             IsInDesignMode ? null : Ioc.Default.GetService<ITeacherCreator>(),
             IsInDesignMode ? null : Ioc.Default.GetService<ITeacherEditor>(),
             IsInDesignMode ? null : Ioc.Default.GetService<ISubjectCreator>(),
             IsInDesignMode ? null : Ioc.Default.GetService<ISubjectEditor>(),
             IsInDesignMode ? null : Ioc.Default.GetService<ICourseCreator>(),
             IsInDesignMode ? null : Ioc.Default.GetService<ICourseEditor>(),
             IsInDesignMode ? null : Ioc.Default.GetService<IGradeCreator>(),
             IsInDesignMode ? null : Ioc.Default.GetService<IGradeEditor>(),
             IsInDesignMode ? null : Ioc.Default.GetService<ICurriculumCreator>(),
             IsInDesignMode ? null : Ioc.Default.GetService<ICurriculumEditor>(),
             IsInDesignMode ? null : Ioc.Default.GetService<ISemesterStatisticsDisplay>(),
             IsInDesignMode ? null : Ioc.Default.GetService<ITeacherStatisticsDisplay>(),
             IsInDesignMode ? null : Ioc.Default.GetService<IStudentStatisticsDisplay>(),
             IsInDesignMode ? null : Ioc.Default.GetService<ISubjectStatisticsDisplay>(),
             IsInDesignMode ? null : Ioc.Default.GetService<IMessageBoxService>()) { }

        public MainWindowViewModel(IStudentCreator studentCreator, IStudentEditor studentEditor, ITeacherCreator teacherCreator, ITeacherEditor teacherEditor,
            ISubjectCreator subjectCreator, ISubjectEditor subjectEditor, ICourseCreator courseCreator, ICourseEditor courseEditor,
            IGradeCreator gradeCreator, IGradeEditor gradeEditor, ICurriculumCreator curriculumCreator, ICurriculumEditor curriculumEditor,
            ISemesterStatisticsDisplay semesterStatisticsDisplay, ITeacherStatisticsDisplay teacherStatisticsDisplay, IStudentStatisticsDisplay studentStatisticsDisplay,
            ISubjectStatisticsDisplay subjectStatisticsDisplay, IMessageBoxService messageBoxService)
        {
            #region Property Inititalizations

            Students = new RestCollection<Student>("http://localhost:4180/", "people/students", "hub");
            Teachers = new RestCollection<Teacher>("http://localhost:4180/", "people/teachers", "hub");
            Subjects = new RestCollection<Subject>("http://localhost:4180/", "education/subjects", "hub");
            Courses = new RestCollection<Course>("http://localhost:4180/", "education/courses", "hub");
            Grades = new RestCollection<Grade>("http://localhost:4180/", "grades", "hub");
            Curriculums = new RestCollection<Curriculum>("http://localhost:4180/", "curriculums", "hub");


            #endregion Property Initialization


            #region Command Initializations

            UpdateStudentCommand = new RelayCommand(
               () => { IsStudentUpdating = true; studentEditor?.Edit(SelectedStudent); },
               () => SelectedStudent != null && !IsStudentUpdating
            );

            CreateStudentCommand = new RelayCommand(
            () => { IsStudentCreating = true; studentCreator?.Create(); },
            () => !IsStudentCreating
            );


            DeleteStudentCommand = new RelayCommand(
                async () => {
                    try
                    {
                        await Students?.Delete(SelectedStudent.StudentId);
                        Grades?.TriggerReset();
                        this.Messenger.Send("", "StudentDeleted");
                    } catch (ArgumentException e)
                    {
                        messageBoxService.ShowWarning(e.Message);
                    }
                },
                () => SelectedStudent != null
            );

            UpdateTeacherCommand = new RelayCommand(
                () => { IsTeacherUpdating = true; teacherEditor?.Edit(SelectedTeacher); },
                () => SelectedTeacher != null && !IsTeacherUpdating
             );

            CreateTeacherCommand = new RelayCommand(
                () => { IsTeacherCreating = true; teacherCreator?.Create(); },
                () => !IsTeacherCreating
            );

            DeleteTeacherCommand = new RelayCommand(
                async () => {
                    try
                    {
                        await Teachers?.Delete(SelectedTeacher.TeacherId);
                        Subjects?.TriggerReset();
                        Courses?.TriggerReset();
                        Grades?.TriggerReset();
                        this.Messenger.Send("", "TeacherDeleted");
                    } 
                    catch (ArgumentException e)
                    {
                        messageBoxService.ShowWarning(e.Message);
                    }
                },
                () => SelectedTeacher != null
            );

            UpdateSubjectCommand = new RelayCommand(
                () => { IsSubjectUpdating = true; subjectEditor?.Edit(SelectedSubject); },
                () => SelectedSubject != null && !IsSubjectUpdating
             );

            CreateSubjectCommand = new RelayCommand(
                () => { IsSubjectCreating = true; subjectCreator.Create(); },
                () => !IsSubjectCreating
            );

            DeleteSubjectCommand = new RelayCommand(
                async () => {
                    try
                    {
                        await Subjects?.Delete(SelectedSubject.SubjectId);
                        Courses?.TriggerReset();
                        Grades?.TriggerReset();
                        this.Messenger.Send("", "TeacherDeleted");
                    }
                    catch (ArgumentException e)
                    {
                        messageBoxService.ShowWarning(e.Message);
                    }
                },
                () => SelectedSubject != null
            );

            CreateCurriculumCommand = new RelayCommand(
                () => { IsCurriculumCreating = true; curriculumCreator.Create(); },
                () => !IsCurriculumCreating
             );

            UpdateCurriculumCommand = new RelayCommand(
                () => { IsCurriculumUpdating = true; curriculumEditor.Edit(SelectedCurriculum); },
                () => SelectedCurriculum != null && !IsCurriculumUpdating
             );

            DeleteCurriculumCommand = new RelayCommand(
                async () => {
                    try
                    {
                        await Curriculums?.Delete(SelectedCurriculum.CurriculumId);
                        Students?.TriggerReset();
                        Subjects?.TriggerReset();
                        Grades?.TriggerReset();
                        this.Messenger.Send("", "CurriculumDeleted");
                    } catch (ArgumentException e)
                    {
                        messageBoxService.ShowWarning(e.Message);
                    }

                },
                () => SelectedCurriculum != null
             );

            CreateGradeCommand = new RelayCommand(
                () => { IsGradeCreating = true; gradeCreator?.Create(); },
                () => !IsGradeCreating
             );


            UpdateGradeCommand = new RelayCommand(
                () => { IsGradeUpdating = true; gradeEditor?.Edit(SelectedGrade); },
                () => SelectedGrade != null && !IsGradeUpdating
            );

            DeleteGradeCommand = new RelayCommand(
                async () => {
                    await Grades?.Delete(SelectedGrade.GradeId);
                    this.Messenger.Send("", "GradeDeleted");
                },
                () => SelectedGrade != null
             );

            CreateCourseCommand = new RelayCommand(
                () => { IsCourseCreating = true; courseCreator?.Create(); },
                () => !IsCourseCreating
             );


            UpdateCourseCommand = new RelayCommand(
                () => { IsCourseUpdating = true; courseEditor?.Edit(SelectedCourse); },
                () => !IsCourseUpdating && SelectedCourse != null
             );

            DeleteCourseCommand = new RelayCommand(
                () => Courses?.Delete(SelectedCourse.CourseId),
                () => SelectedCourse != null
            );

            RegisterForSubjectCommand = new RelayCommand(
                () =>
                {
                    try
                    {
                        restService.Post($"Education/Subjects/{SelectedSubject.SubjectId}/Register/{SelectedStudent.StudentId}");
                        messageBoxService.ShowInfo("Successfully registered for subject!");
                    }
                    catch (Exception ex) 
                    {
                        messageBoxService.ShowWarning(ex.Message);
                    }
                },
                () => SelectedStudent != null && SelectedSubject != null
            );

            UnregisterFromSubjectCommand = new RelayCommand(
                () =>
                {
                    try
                    {
                        restService.Delete($"Education/Subjects/{SelectedSubject.SubjectId}/Register/{SelectedStudent.StudentId}");
                        messageBoxService.ShowInfo("Successfully unregistered from subject!");
                    }
                    catch (Exception ex)
                    {
                        messageBoxService.ShowWarning(ex.Message);
                    }
                },
                () => SelectedStudent != null && SelectedSubject != null
            );

            RegisterForCourseCommand = new RelayCommand(
                () =>
                {
                    try
                    {
                        restService.Post($"Education/Courses/{SelectedCourse.CourseId}/Register/{SelectedStudent.StudentId}");
                        messageBoxService.ShowInfo("Successfully registered for course!");
                    }
                    catch (Exception ex)
                    {
                        messageBoxService.ShowWarning(ex.Message);
                    }
                },
                () => SelectedStudent != null && SelectedCourse != null
             );

            UnregisterFromCourseCommand = new RelayCommand(
                () =>
                {
                    try
                    {
                        restService.Delete($"Education/Courses/{SelectedCourse.CourseId}/Register/{SelectedStudent.StudentId}");
                        messageBoxService.ShowInfo("Successfully unregistered from course!");
                    }
                    catch (Exception ex)
                    {
                        messageBoxService.ShowWarning(ex.Message);
                    }
                },
                () => SelectedStudent != null && SelectedCourse != null
            );

            ResetSemesterCommand = new RelayCommand(
                () =>
                {
                    try
                    {
                        restService.Post("Education/Semester/Reset");
                        messageBoxService.ShowInfo("Subject and Course Resgistrations have been cleared!");

                    }
                    catch (Exception ex)
                    {
                        messageBoxService.ShowWarning(ex.Message);
                    }
                }
            );

            GetStudentScheduleCommand = new RelayCommand(
                () =>
                {
                    try
                    {
                        string schedule = restService.GetAsString($"People/Students/Schedule/{SelectedStudent.StudentId}");
                        schedule = schedule.Replace("\"", "");
                        schedule = schedule.Replace("\\n", Environment.NewLine);
                        schedule = schedule.Replace("\\t", "    ");
                        messageBoxService.ShowInfo(schedule, "Schedule");

                    }
                    catch (Exception ex)
                    {
                        messageBoxService.ShowWarning(ex.Message);
                    }
                },
                () => SelectedStudent != null
            );

            GetTeacherScheduleCommand = new RelayCommand(
               () =>
               {
                   try
                   {
                       string schedule = restService.GetAsString($"People/Teachers/Schedule/{SelectedTeacher.TeacherId}");
                       schedule = schedule.Replace("\"", "");
                       schedule = schedule.Replace("\\n", Environment.NewLine);
                       schedule = schedule.Replace("\\t", "    ");
                       messageBoxService.ShowInfo(schedule, "Schedule");
                   }
                   catch (Exception ex)
                   {
                       messageBoxService.ShowWarning(ex.Message);
                   }
               },
               () => SelectedTeacher != null
           );

            GetSubjectStatisticsCommand = new RelayCommand(
                () =>
                {
                    subjectStatisticsDisplay.Display(restService, SelectedSubject.SubjectId);
                },
                () => SelectedSubject != null
            );

            ShowBestStudentsCommand = new RelayCommand(
                () =>
                {
                    studentStatisticsDisplay.Display(restService);
                }
            );

            ToSemesterStatisticsCommand = new RelayCommand(
                () =>
                {
                    semesterStatisticsDisplay.Display(restService);
                }
             );

            ToTeacherStatisticsCommand = new RelayCommand(
                () => {
                    teacherStatisticsDisplay.Display(restService);
                    }
            );


            #endregion Command Initializations


            #region Messenger Registrations


            this.Messenger.Register<MainWindowViewModel, Student, string>(this, "StudentUpdateRequested", async (recipient, msg) =>
            {
                try
                {
                    await Students?.Update(msg);
                    this.Messenger.Send("Student updated!", "StudentUpdated");
                    Grades?.TriggerReset();
                } 
                catch (Exception ex)
                {
                    this.Messenger.Send(ex.Message, "FailedToUpdateStudent");
                }
            });

            this.Messenger.Register<MainWindowViewModel, Student, string>(this, "StudentCreationRequested", async (recipient, msg) =>
            {
                try
                {
                    await Students?.Add(msg);
                    this.Messenger.Send("Student created!", "StudentCreated");
                }
                catch (Exception ex)
                {
                    this.Messenger.Send(ex.Message, "FailedToCreateStudent");
                }
            });



            this.Messenger.Register<MainWindowViewModel, Teacher, string>(this, "TeacherUpdateRequested", async (recipient, msg) =>
            {
                try
                {
                    await Teachers?.Update(msg);
                    this.Messenger.Send("Teacher updated!", "TeacherUpdated");
                    Courses?.TriggerReset();
                    Grades?.TriggerReset();
                    Subjects?.TriggerReset();
                }
                catch (Exception ex)
                {
                    this.Messenger.Send(ex.Message, "FailedToUpdateTeacher");
                }
            });

            this.Messenger.Register<MainWindowViewModel, Teacher, string>(this, "TeacherCreationRequested", async (recipient, msg) =>
            {
                try
                {
                    await Teachers?.Add(msg);
                    this.Messenger.Send("Teacher created!", "TeacherCreated");
                }
                catch (Exception ex)
                {
                    this.Messenger.Send(ex.Message, "FailedToCreateTeacher");
                }
            });


            this.Messenger.Register<MainWindowViewModel, Subject, string>(this, "SubjectUpdateRequested", async (recipient, msg) =>
            {
                try
                {
                    await Subjects?.Update(msg);
                    this.Messenger.Send("Subject updated!", "SubjectUpdated");
                    Grades?.TriggerReset();
                }
                catch (Exception ex)
                {
                    this.Messenger.Send(ex.Message, "FailedToUpdateSubject");
                }
            });

            this.Messenger.Register<MainWindowViewModel, Subject, string>(this, "SubjectCreationRequested", async (recipient, msg) =>
            {
                try
                {
                    await Subjects?.Add(msg);
                    this.Messenger.Send("Subject created!", "SubjectCreated");
                }
                catch (Exception ex)
                {
                    this.Messenger.Send(ex.Message, "FailedToCreateSubject");
                }
            });

            this.Messenger.Register<MainWindowViewModel, Curriculum, string>(this, "CurriculumUpdateRequested", async (recipient, msg) =>
            {
                try
                {
                    await Curriculums?.Update(msg);
                    this.Messenger.Send("Curriculum updated!", "CurriculumUpdated");
                }
                catch (Exception ex)
                {
                    this.Messenger.Send(ex.Message, "FailedToUpdateCurriculum");
                }
            });

            this.Messenger.Register<MainWindowViewModel, Curriculum, string>(this, "CurriculumCreationRequested", async (recipient, msg) =>
            {
                try
                {
                    await Curriculums?.Add(msg);
                    this.Messenger.Send("Curriculum created!", "CurriculumCreated");
                }
                catch (Exception ex)
                {
                    this.Messenger.Send(ex.Message, "FailedToCreateCurriculum");
                }
            });

            this.Messenger.Register<MainWindowViewModel, Grade, string>(this, "GradeUpdateRequested", async (recipient, msg) =>
            {
                try
                {
                    await Grades?.UpdateDirectly (msg);
                    this.Messenger.Send("Grade updated!", "GradeUpdated");
                }
                catch (Exception ex)
                {
                    this.Messenger.Send(ex.Message, "FailedToUpdateGrade");
                }
            });

            this.Messenger.Register<MainWindowViewModel, Grade, string>(this, "GradeCreationRequested", async (recipient, msg) =>
            {
                try
                {
                    await Grades?.AddDirectly(msg);
                    this.Messenger.Send("Grade created!", "GradeCreated");
                    Grades?.TriggerReset();
                }
                catch (Exception ex)
                {
                    this.Messenger.Send(ex.Message, "FailedToCreateGrade");
                }
            });

            this.Messenger.Register<MainWindowViewModel, Course, string>(this, "CourseUpdateRequested", async (recipient, msg) =>
            {
                try
                {
                    await Courses?.UpdateDirectly(msg);
                    this.Messenger.Send("Course updated!", "CourseUpdated");
                }
                catch (Exception ex)
                {
                    this.Messenger.Send(ex.Message, "FailedToUpdateCourse");
                }
            });

            this.Messenger.Register<MainWindowViewModel, Course, string>(this, "CourseCreationRequested", async (recipient, msg) =>
            {
                try
                {
                    await Courses?.AddDirectly(msg);
                    this.Messenger.Send("Course created!", "CourseCreated");
                }
                catch (Exception ex)
                {
                    this.Messenger.Send(ex.Message, "FailedToCreateGrade");
                }
            });

            this.Messenger.Register<MainWindowViewModel, string, string>(this, "StudentUpdateFinished", (recipient, msg) =>
            {
                IsStudentUpdating = false;
            });

            this.Messenger.Register<MainWindowViewModel, string, string>(this, "StudentCreationFinished", (recipient, msg) =>
            {
                IsStudentCreating = false;
            });

            this.Messenger.Register<MainWindowViewModel, string, string>(this, "TeacherUpdateFinished", (recipient, msg) =>
            {
                IsTeacherUpdating = false;
            });

            this.Messenger.Register<MainWindowViewModel, string, string>(this, "TeacherCreationFinished", (recipient, msg) =>
            {
                IsTeacherCreating = false;
            });

            this.Messenger.Register<MainWindowViewModel, string, string>(this, "SubjectUpdateFinished", (recipient, msg) =>
            {
                IsSubjectUpdating = false;
            });

            this.Messenger.Register<MainWindowViewModel, string, string>(this, "SubjectCreationFinished", (recipient, msg) =>
            {
                IsSubjectCreating = false;
            });

            this.Messenger.Register<MainWindowViewModel, string, string>(this, "CurriculumUpdateFinished", (recipient, msg) =>
            {
                IsCurriculumUpdating = false;
            });

            this.Messenger.Register<MainWindowViewModel, string, string>(this, "CurriculumCreationFinished", (recipient, msg) =>
            {
                IsCurriculumCreating = false;
            });

            this.Messenger.Register<MainWindowViewModel, string, string>(this, "GradeUpdateFinished", (recipient, msg) =>
            {
                IsGradeUpdating = false;
            });

            this.Messenger.Register<MainWindowViewModel, string, string>(this, "GradeCreationFinished", (recipient, msg) =>
            {
                IsGradeCreating = false;
            });


            this.Messenger.Register<MainWindowViewModel, string, string>(this, "CourseUpdateFinished", (recipient, msg) =>
            {
                IsCourseUpdating = false;
            });

            this.Messenger.Register<MainWindowViewModel, string, string>(this, "CourseCreationFinished", (recipient, msg) =>
            {
                IsCourseCreating = false;
            });


            #endregion Messenger Registrations
        }
    }
}
