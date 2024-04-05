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
            SubjectStatistics = this.restService.GetSingle<SubjectStatistics>($"Grades/Subjects/Statistics/{subjectId}");

            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<SubjectStatisticsWindowViewModel, string, string>(this, "GradeCreated", (recipient, msg) =>
            {
                SubjectStatistics = this.restService.GetSingle<SubjectStatistics>($"Grades/Subjects/Statistics/{subjectId}");
            });

            this.Messenger.Register<SubjectStatisticsWindowViewModel, string, string>(this, "GradeDeleted", (recipient, msg) =>
            {
                SubjectStatistics = this.restService.GetSingle<SubjectStatistics>($"Grades/Subjects/Statistics/{subjectId}");
            });

            this.Messenger.Register<SubjectStatisticsWindowViewModel, string, string>(this, "SubjectDeleted", (recipient, msg) =>
            {
                SubjectStatistics = this.restService.GetSingle<SubjectStatistics>($"Grades/Subjects/Statistics/{subjectId}");
            });

            this.Messenger.Register<SubjectStatisticsWindowViewModel, string, string>(this, "SubjectUpdated", (recipient, msg) =>
            {
                SubjectStatistics = this.restService.GetSingle<SubjectStatistics>($"Grades/Subjects/Statistics/{subjectId}");
            });

            this.Messenger.Register<SubjectStatisticsWindowViewModel, string, string>(this, "StudentDeleted", (recipient, msg) =>
            {
                SubjectStatistics = this.restService.GetSingle<SubjectStatistics>($"Grades/Subjects/Statistics/{subjectId}");
            });

            this.Messenger.Register<SubjectStatisticsWindowViewModel, string, string>(this, "CurriculumDeleted", (recipient, msg) =>
            {
                SubjectStatistics = this.restService.GetSingle<SubjectStatistics>($"Grades/Subjects/Statistics/{subjectId}");
            });

            this.Messenger.Register<SubjectStatisticsWindowViewModel, string, string>(this, "GradeUpdated", (recipient, msg) =>
            {
                SubjectStatistics = this.restService.GetSingle<SubjectStatistics>($"Grades/Subjects/Statistics/{subjectId}");
            });
        }
        public void Dispose()
        {
            this.Messenger.UnregisterAll(this);
            
        }
    }
}
