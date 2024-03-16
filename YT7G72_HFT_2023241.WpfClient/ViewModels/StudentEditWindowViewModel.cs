using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
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
    public class StudentEditWindowViewModel : ObservableRecipient
    {
        private Student student;
        private string errorMessage;
        public RestCollection<Student> Students { get; set; }
        public Student Student { get { return student; } set { SetProperty(ref student, value); } }
        public string ErrorMessage { get { return errorMessage; } set { SetProperty(ref errorMessage, value); MessageBox.Show(errorMessage); } }
        public FinancialStatus[] FinancialStatuses { get; set; } = (FinancialStatus[])Enum.GetValues(typeof(FinancialStatus));
        public ICommand SaveChangesCommand { get; set; }

        public StudentEditWindowViewModel(RestCollection<Student> students,Student student)
        {
            Students = students;
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
                () => {
                    try
                    {
                        students.Update(Student);
                    } catch (Exception e)
                    {
                        ErrorMessage = e.Message;
                    }
                }
            );
        }
    }
}
