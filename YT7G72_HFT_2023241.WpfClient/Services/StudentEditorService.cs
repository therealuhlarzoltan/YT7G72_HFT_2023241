using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.WpfClient.ViewModels;
using YT7G72_HFT_2023241.WpfClient.Windows;
using YT7G72_HFT_2023241.WpfClient.Logic;

namespace YT7G72_HFT_2023241.WpfClient.Services
{
    public class StudentEditorService
    {
        public void Edit(RestCollection<Student> students, Student student)
        {
            StudentEditWindowViewModel viewModel = new StudentEditWindowViewModel(students, student);
            StudentEditWindow window = new StudentEditWindow(viewModel);
            window.Show();
        }
    }
}
