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
    public class SemesterStatisticsDisplay : ISemesterStatisticsDisplay
    {
        public void Display(RestService restService)
        {
            var vm = new SemesterStatisticsWindowViewModel(restService, Ioc.Default.GetService<IMessageBoxService>());
            new SemesterStatisticsWindow(vm).Show();
        }
    }
}
