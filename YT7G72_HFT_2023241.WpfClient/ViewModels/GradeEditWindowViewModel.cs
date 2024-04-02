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
    public class GradeEditWindowViewModel : ObservableRecipient, IDisposable
    {
        private Grade grade;
        public Grade Grade { get { return grade; } set { SetProperty(ref grade, value); } }
        public ICommand SaveChangesCommand { get; set; }

        public GradeEditWindowViewModel(Grade grade)
        {
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
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
            this.Messenger.Register<GradeEditWindowViewModel, string, string>(this, "GradeUpdated", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "GradeUpdateFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
