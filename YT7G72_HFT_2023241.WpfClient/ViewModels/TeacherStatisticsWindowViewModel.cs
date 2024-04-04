using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.WpfClient.Logic;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class TeacherStatisticsWindowViewModel : ObservableRecipient, IDisposable
    {
        private RestService restService;
        public ObservableCollection<AverageByPersonDTO<Teacher>> BestTeachers = new ObservableCollection<AverageByPersonDTO<Teacher>>();
        public ICommand ShowBestTeachersCommand { get; set; }
        public ICommand HideBestTeachersCommand { get; set; }
        public ICommand GetTeacherStatistics { get; set; }
        public TeacherStatisticsWindowViewModel(RestService restService)
        {
            this.restService = restService;
        }

        private void RegisterMessengers()
        {

        }

        public void Dispose()
        {
            this.Messenger.UnregisterAll(this);
        }
    }
}
