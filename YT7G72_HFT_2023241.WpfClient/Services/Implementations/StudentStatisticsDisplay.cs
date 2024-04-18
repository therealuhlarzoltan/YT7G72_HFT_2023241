using Microsoft.Toolkit.Mvvm.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.WpfClient.Logic;
using YT7G72_HFT_2023241.WpfClient.Services.Interfaces;
using YT7G72_HFT_2023241.WpfClient.ViewModels;
using YT7G72_HFT_2023241.WpfClient.Windows;

namespace YT7G72_HFT_2023241.WpfClient.Services.Implementations
{
    class StudentStatisticsDisplay : IStudentStatisticsDisplay
    {
        public void Display(RestService restService)
        {
            var vm = new BestStudentsWindowViewModel(restService, Ioc.Default.GetService<IMessageBoxService>());
            new BestStudentsWindow(vm).Show();
        }
    }
}
