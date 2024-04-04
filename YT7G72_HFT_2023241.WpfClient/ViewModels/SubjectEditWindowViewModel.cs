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
    public class SubjectEditWindowViewModel : ObservableRecipient, IDisposable
    {


        private Subject subject;
        public Subject Subject { get { return subject; } set { SetProperty(ref subject, value); } }
        public Requirement[] Requirements { get; set; } = (Requirement[])Enum.GetValues(typeof(Requirement));
        public ICommand SaveChangesCommand { get; set; }

        public SubjectEditWindowViewModel(Subject subject)
        {
            Subject = new Subject()
            { 
                SubjectId = subject.SubjectId,
                SubjectCode = subject.SubjectCode,
                SubjectName = subject.SubjectName,
                Credits = subject.Credits,
                PreRequirementId = subject.PreRequirementId,
                Requirement = subject.Requirement,
                CurriculumId = subject.CurriculumId,
                TeacherId = subject.TeacherId,
            };

            SaveChangesCommand = new RelayCommand(
                () =>
                {
                    this.Messenger.Send(Subject, "SubjectUpdateRequested");
                }
            );

            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<SubjectEditWindowViewModel, string, string>(this, "FailedToUpdateSubject", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
            this.Messenger.Register<SubjectEditWindowViewModel, string, string>(this, "SubjectUpdated", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }


        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "SubjectUpdateFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
