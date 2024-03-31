using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.WpfClient.ViewModels;
using YT7G72_HFT_2023241.WpfClient.Windows;
using YT7G72_HFT_2023241.WpfClient.Logic;
using YT7G72_HFT_2023241.WpfClient.Services.Interfaces;

namespace YT7G72_HFT_2023241.WpfClient.Services.Implementations
{
    public class StudentEditorService : IStudentEditor
    {
        public void Edit(Student student)
        {
            StudentEditWindowViewModel viewModel = new StudentEditWindowViewModel(student);
            StudentEditWindow window = new StudentEditWindow(viewModel);
            window.Show();
        }
    }
}
