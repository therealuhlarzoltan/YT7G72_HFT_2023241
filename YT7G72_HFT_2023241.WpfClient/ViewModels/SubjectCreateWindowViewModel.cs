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
    public class SubjectCreateWindowViewModel : ObservableRecipient, IDisposable
    {
        private Subject subject;
        public Subject Subject { get { return subject; } set { SetProperty(ref subject, value); } }
        public Requirement[] Requirements { get; set; } = (Requirement[])Enum.GetValues(typeof(Requirement));
        public ICommand CreateSubjectCommand { get; set; }

        public SubjectCreateWindowViewModel()
        {
            Subject = new Subject();

            CreateSubjectCommand = new RelayCommand(
                () =>
                {
                    this.Messenger.Send(Subject, "SubjectCreationRequested");
                }
            );

            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<SubjectCreateWindowViewModel, string, string>(this, "FailedToCreateSubject", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
            this.Messenger.Register<SubjectCreateWindowViewModel, string, string>(this, "SubjectCreated", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }


        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "SubjectCreationFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
