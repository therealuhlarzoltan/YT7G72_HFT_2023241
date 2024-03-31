using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using YT7G72_HFT_2023241.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class TeacherCreateWindowViewModel : ObservableRecipient, IDisposable
    {
        private Teacher teacher;
        public Teacher Teacher { get { return teacher; } set { SetProperty(ref teacher, value); } }
        public AcademicRank[] AcademicRanks{ get; set; } = (AcademicRank[])Enum.GetValues(typeof(AcademicRank));
        public ICommand CreateTeacherCommand { get; set; }

        public TeacherCreateWindowViewModel()
        {
            Teacher = new Teacher();

            CreateTeacherCommand = new RelayCommand(
                () =>
                {
                    this.Messenger.Send(Teacher, "TeacherCreationRequested");
                }
            );

            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<TeacherCreateWindowViewModel, string, string>(this, "FailedToCreateTeacher", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
            this.Messenger.Register<TeacherCreateWindowViewModel, string, string>(this, "TeacherCreated", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }


        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "TeacherCreationFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
