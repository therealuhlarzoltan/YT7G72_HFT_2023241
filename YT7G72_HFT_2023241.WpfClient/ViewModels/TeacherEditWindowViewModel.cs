using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using YT7G72_HFT_2023241.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using YT7G72_HFT_2023241.WpfClient.Services.Interfaces;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class TeacherEditWindowViewModel : ObservableRecipient, IDisposable
    {
        private IMessageBoxService messageBoxService;
        private Teacher teacher;
        public Teacher Teacher { get { return teacher; } set { SetProperty(ref teacher, value); } }
        public AcademicRank[] AcademicRanks { get; set; } = (AcademicRank[])Enum.GetValues(typeof(AcademicRank));
        public ICommand SaveChangesCommand { get; set; }

        public TeacherEditWindowViewModel(Teacher teacher, IMessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;
            Teacher = new Teacher()
            {
                TeacherId = teacher.TeacherId,
                FirstName = teacher.FirstName,
                LastName = teacher.LastName,
                AcademicRank = teacher.AcademicRank
            };
            SaveChangesCommand = new RelayCommand(
                () =>
                {
                    this.Messenger.Send(Teacher, "TeacherUpdateRequested");
                }
            );

            RegisterMessengers();
            
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<TeacherEditWindowViewModel, string, string>(this, "FailedToUpdateTeacher", (recipient, msg) =>
            {
                messageBoxService.ShowWarning(msg);
            });
            this.Messenger.Register<TeacherEditWindowViewModel, string, string>(this, "TeacherUpdated", (recipient, msg) =>
            {
                messageBoxService.ShowInfo(msg);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "TeacherUpdateFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
