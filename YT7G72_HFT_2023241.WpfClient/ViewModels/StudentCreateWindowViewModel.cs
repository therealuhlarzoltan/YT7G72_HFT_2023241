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
using YT7G72_HFT_2023241.WpfClient.Services.Interfaces;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class StudentCreateWindowViewModel : ObservableRecipient, IDisposable
    {
        private IMessageBoxService messageBoxService;
        private Student student;
        public Student Student { get { return student; } set { SetProperty(ref student, value); } }
        public FinancialStatus[] FinancialStatuses { get; set; } = (FinancialStatus[])Enum.GetValues(typeof(FinancialStatus));
        public ICommand CreateStudentCommand { get; set; }

        public StudentCreateWindowViewModel(IMessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;
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
                messageBoxService.ShowWarning(msg);
            });
            this.Messenger.Register<StudentCreateWindowViewModel, string, string>(this, "StudentCreated", (recipient, msg) =>
            {
                messageBoxService.ShowInfo(msg);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "StudentCreationFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
