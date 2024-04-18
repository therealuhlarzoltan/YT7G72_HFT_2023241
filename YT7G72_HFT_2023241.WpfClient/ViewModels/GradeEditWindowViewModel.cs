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
    public class GradeEditWindowViewModel : ObservableRecipient, IDisposable
    {
        private IMessageBoxService messageBoxService;
        private Grade grade;
        public Grade Grade { get { return grade; } set { SetProperty(ref grade, value); } }
        public ICommand SaveChangesCommand { get; set; }

        public GradeEditWindowViewModel(Grade grade, IMessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;
            Grade = new Grade()
            {
                GradeId = grade.GradeId,
                SubjectId = grade.SubjectId,
                TeacherId = grade.TeacherId,
                StudentId = grade.StudentId,
                Semester = grade.Semester,
                Mark = grade.Mark,
            };

            SaveChangesCommand = new RelayCommand(
                () =>
                {
                    this.Messenger.Send(Grade, "GradeUpdateRequested");
                }
            );

            RegisterMessengers();

        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<GradeEditWindowViewModel, string, string>(this, "FailedToUpdateGrade", (recipient, msg) =>
            {
                messageBoxService.ShowWarning(msg);
            });
            this.Messenger.Register<GradeEditWindowViewModel, string, string>(this, "GradeUpdated", (recipient, msg) =>
            {
                messageBoxService.ShowInfo(msg);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "GradeUpdateFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
