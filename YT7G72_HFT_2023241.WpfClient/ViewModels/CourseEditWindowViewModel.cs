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

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class CourseEditWindowViewModel : ObservableRecipient, IDisposable
    {
        private Course course;
        private string timeSpanString;
        public string TimeSpanString { get { return timeSpanString; } set { SetProperty(ref timeSpanString, value); } }
        public Course Course { get { return course; } set { SetProperty(ref course, value); } }
        public DayOfWeek[] Days { get; set; } = (DayOfWeek[])Enum.GetValues(typeof(DayOfWeek));
        public CourseType[] CourseTypes { get; set; } = (CourseType[])Enum.GetValues(typeof(CourseType));
        public ICommand SaveChangesCommand { get; set; }

        public CourseEditWindowViewModel(Course course)
        {
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

            SaveChangesCommand = new RelayCommand(
                () =>
                {
                    try
                    {
                        Course.StartTime = TimeSpan.Parse(TimeSpanString);
                        this.Messenger.Send(Course, "CourseUpdateRequested");
                    }
                    catch (Exception e) when (e is ArgumentNullException || e is FormatException || e is OverflowException)
                    {
                        MessageBox.Show("Start Time format: HH:MM", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }

                }
            );

            RegisterMessengers();

        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<CourseEditWindowViewModel, string, string>(this, "FailedToUpdateCourse", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
            this.Messenger.Register<CourseEditWindowViewModel, string, string>(this, "CourseUpdated", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "CourseUpdateFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
