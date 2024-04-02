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
    public class GradeCreateWindowViewModel : ObservableRecipient, IDisposable
    {
        private Grade grade;
        public Grade Grade { get { return grade; } set { SetProperty(ref grade, value); } }
        public ICommand CreateGradeCommand { get; set; }

        public GradeCreateWindowViewModel()
        {
            Grade = new Grade();

            CreateGradeCommand = new RelayCommand(
                () =>
                {
                    this.Messenger.Send(Grade, "GradeCreationRequested");
                }
            );

            RegisterMessengers();

        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<GradeCreateWindowViewModel, string, string>(this, "FailedToCreateGrade", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
            this.Messenger.Register<GradeCreateWindowViewModel, string, string>(this, "GradeCreated", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "GradeCreationFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
