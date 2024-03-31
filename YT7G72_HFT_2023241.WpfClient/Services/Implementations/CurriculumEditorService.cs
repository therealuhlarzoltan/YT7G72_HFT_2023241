using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.WpfClient.Services.Interfaces;
using YT7G72_HFT_2023241.WpfClient.ViewModels;
using YT7G72_HFT_2023241.WpfClient.Windows;

namespace YT7G72_HFT_2023241.WpfClient.Services.Implementations
{
    public class CurriculumEditorService : ICurriculumEditor
    {
        public void Edit(Curriculum curriculum)
        {
            var vm = new CurriculumEditWindowViewModel(curriculum);
            new CurriculumEditWindow(vm).Show();
        }
    }
}
