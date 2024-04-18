using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using YT7G72_HFT_2023241.Models;
using System.Windows.Interop;
using System.Windows.Controls;
using YT7G72_HFT_2023241.WpfClient.Services.Interfaces;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class CourseCreateWindowViewModel : ObservableRecipient, IDisposable
    {
        private IMessageBoxService messageBoxService;
        private Course course;
        private string timeSpanString;
        private string teacherIdFKString;
        public string TeacherIdFKString
        {
            get { return teacherIdFKString; }
            set { SetProperty(ref teacherIdFKString, value); }
        }
        public string TimeSpanString { get { return timeSpanString; }   set { SetProperty(ref timeSpanString, value); } }
        public Course Course { get { return course; } set { SetProperty(ref course, value); } }
        public DayOfWeek[] Days { get; set; } = (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek));
        public CourseType[] CourseTypes { get; set; } = (CourseType[])Enum.GetValues(typeof(CourseType));
        public ICommand CreateCourseCommand { get; set; }

        public CourseCreateWindowViewModel(IMessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;
            Course = new Course();
            TeacherIdFKString = string.Empty;

            CreateCourseCommand = new RelayCommand(
                () =>
                {
                    int? tId = null;

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(TeacherIdFKString))
                            tId = int.Parse(TeacherIdFKString);
                        Course.TeacherId = tId;
                    }
                    catch (Exception e) when (e is FormatException || e is OverflowException)
                    {
                        messageBoxService.ShowWarning("Invalid TeacherId provided!");
                        return;
                    }

                    try
                    {
                        Course.StartTime = TimeSpan.Parse(TimeSpanString);
                        this.Messenger.Send(Course, "CourseCreationRequested");
                    }
                    catch (Exception e) when (e is ArgumentNullException || e is FormatException || e is OverflowException)
                    {
                        messageBoxService.ShowWarning("Start Time format: HH:MM");
                    }

                }
            );

            RegisterMessengers();

        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<CourseCreateWindowViewModel, string, string>(this, "FailedToCreateCourse", (recipient, msg) =>
            {
                messageBoxService.ShowWarning(msg);
            });
            this.Messenger.Register<CourseCreateWindowViewModel, string, string>(this, "CourseCreated", (recipient, msg) =>
            {
                messageBoxService.ShowInfo(msg);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "CourseCreationFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
