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
using YT7G72_HFT_2023241.WpfClient.Services.Interfaces;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class CourseEditWindowViewModel : ObservableRecipient, IDisposable
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
        public string TimeSpanString { get { return timeSpanString; } set { SetProperty(ref timeSpanString, value); } }
        public Course Course { get { return course; } set { SetProperty(ref course, value); } }
        public DayOfWeek[] Days { get; set; } = (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek));
        public CourseType[] CourseTypes { get; set; } = (CourseType[])Enum.GetValues(typeof(CourseType));
        public ICommand SaveChangesCommand { get; set; }

        public CourseEditWindowViewModel(Course course, IMessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;
            Course = new Course()
            {
                CourseId = course.CourseId,
                CourseName = course.CourseName,
                CourseCapacity = course.CourseCapacity,
                CourseType = course.CourseType,
                DayOfWeek = course.DayOfWeek,
                LengthInMinutes = course.LengthInMinutes,
                Room = course.Room,
                StartTime = course.StartTime,
                TeacherId = course.TeacherId,
                SubjectId = course.SubjectId,
            };

            TimeSpanString = course.StartTime.ToString(@"hh\:mm");
            TeacherIdFKString = course?.TeacherId != null ? course?.TeacherId.ToString() : string.Empty;

            SaveChangesCommand = new RelayCommand(
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
                        this.Messenger.Send(Course, "CourseUpdateRequested");
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
            this.Messenger.Register<CourseEditWindowViewModel, string, string>(this, "FailedToUpdateCourse", (recipient, msg) =>
            {
                messageBoxService.ShowWarning(msg);
            });
            this.Messenger.Register<CourseEditWindowViewModel, string, string>(this, "CourseUpdated", (recipient, msg) =>
            {
                messageBoxService.ShowInfo(msg);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "CourseUpdateFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
