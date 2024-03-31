using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.WpfClient.Logic;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class StudentEditWindowViewModel : ObservableRecipient, IDisposable
    {
        private Student student;
        public Student Student { get { return student; } set { SetProperty(ref student, value); } } 
        public FinancialStatus[] FinancialStatuses { get; set; } = (FinancialStatus[])Enum.GetValues(typeof(FinancialStatus));
        public ICommand SaveChangesCommand { get; set; }

        public StudentEditWindowViewModel(Student student)
        {
            Student = new Student()
            {
                StudentId = student.StudentId,
                FirstName = student.FirstName,
                LastName = student.LastName,
                StudentCode = student.StudentCode,
                CurriculumId = student.CurriculumId,
                FinancialStatus = student.FinancialStatus,
            };
            SaveChangesCommand = new RelayCommand(
                () =>
                {
                    this.Messenger.Send(Student, "StudentUpdateRequested");
                }
            );

            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<StudentEditWindowViewModel, string, string>(this, "FailedToUpdateStudent", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
            this.Messenger.Register<StudentEditWindowViewModel, string, string>(this, "StudentUpdated", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "StudentUpdateFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
