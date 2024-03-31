using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.WpfClient.Services;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class StudentCreateWindowViewModel : ObservableRecipient, IDisposable
    {
        private Student student;
        public Student Student { get { return student; } set { SetProperty(ref student, value); } }
        public FinancialStatus[] FinancialStatuses { get; set; } = (FinancialStatus[])Enum.GetValues(typeof(FinancialStatus));
        public ICommand CreateStudentCommand { get; set; }

        public StudentCreateWindowViewModel()
        {
            Student = new Student();

            CreateStudentCommand = new RelayCommand(
                () =>
                {
                    this.Messenger.Send(Student, "StudentCreationRequested");
                }
            );

            RegisterMessengers();
          
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<StudentCreateWindowViewModel, string, string>(this, "FailedToCreateStudent", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
            this.Messenger.Register<StudentCreateWindowViewModel, string, string>(this, "StudentCreated", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "StudentCreationFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
