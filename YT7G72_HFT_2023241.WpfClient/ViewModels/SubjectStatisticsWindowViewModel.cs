using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.WpfClient.Logic;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class SubjectStatisticsWindowViewModel : ObservableRecipient, IDisposable
    {
        private RestService restService;
        private int subjectId;
        private SubjectStatistics subjectStatistics;

        public SubjectStatistics SubjectStatistics
        {
            get { return subjectStatistics; }
            set
            {
                SetProperty(ref subjectStatistics, value);
            }
        }

        public SubjectStatisticsWindowViewModel(RestService restService, int subjectId) 
        {
            this.restService = restService;
            this.subjectId = subjectId;
        }

        private void RegisterMessengers()
        {

        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
