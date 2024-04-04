using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.WpfClient.Logic;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
     public class SemesterStatisticsWindowViewModel : ObservableRecipient, IDisposable
     {
        private RestService restService;
        private string semesterString;
        private Visibility semesterStatisticsVisibility;

        public Visibility SemesterStatisticsVisibility
        {
            get { return semesterStatisticsVisibility; }
            set { 
                SetProperty(ref semesterStatisticsVisibility, value); 
            }
        }

        public string SemesterString
        {
            get { return semesterString; }
            set { SetProperty(ref semesterString, value); (GetSemesterStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged(); }
        }
        private SemesterStatistics semesterStatistics;
        public SemesterStatistics SemesterStatistics
        {
            get { return semesterStatistics; }
            set { SetProperty(ref semesterStatistics, value); }
        }

        private ObservableCollection<SemesterStatistics> allSemesterStatistics = new ObservableCollection<SemesterStatistics>();

        public ObservableCollection<SemesterStatistics> AllSemesterStatistics
        {
            get { return allSemesterStatistics; }
            set { SetProperty(ref allSemesterStatistics, value); }
        }
        public ICommand GetSemesterStatisticsCommand { get; set; }
        public ICommand GetAllSemesterStatisticsCommand { get; set; }
        public ICommand HideSemesterStatisticsCommand { get; set; }
        public ICommand HideAllSemesterStatisticsCommand { get; set; }

        public SemesterStatisticsWindowViewModel(RestService restService)
        {
            this.restService = restService;
            SemesterStatisticsVisibility = Visibility.Collapsed;

            GetSemesterStatisticsCommand = new RelayCommand(
                () => {
                    SemesterString = SemesterString.Replace("/", "-");
                    var stat = this.restService.GetSingle<SemesterStatistics>($"Grades/Semester/Statistics/{SemesterString}");
                    SemesterStatistics = stat;
                    SemesterStatisticsVisibility = Visibility.Visible;
                    (HideSemesterStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                },
                () => SemesterString != null && !string.IsNullOrWhiteSpace(SemesterString)
             );

            GetAllSemesterStatisticsCommand = new RelayCommand(
                () => {
                    var collection = this.restService.Get<SemesterStatistics>($"Grades/Semester/Statistics");
                    AllSemesterStatistics.Clear();
                    foreach (var e in collection)
                        AllSemesterStatistics.Add(e);
                    (HideAllSemesterStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }
             );

            HideSemesterStatisticsCommand = new RelayCommand(
                () =>
                {
                    SemesterStatistics = null;
                    SemesterStatisticsVisibility = Visibility.Collapsed;
                    (HideSemesterStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                },
                () => SemesterStatistics != null);

            HideAllSemesterStatisticsCommand = new RelayCommand(
                () =>
                {
                    AllSemesterStatistics.Clear();
                    (HideAllSemesterStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                },
                () => AllSemesterStatistics.Count >  0);

            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<SemesterStatisticsWindowViewModel, string, string>(this, "GradeCreated", (receipient, msg) =>
            {
                if (SemesterStatistics != null)
                {
                    var year = SemesterStatistics.Semester.Replace("/", "-");
                    var stat = this.restService.GetSingle<SemesterStatistics>($"Grades/Semester/Statistics/{year}");
                    SemesterStatistics = stat;
                    (HideSemesterStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }

                if (AllSemesterStatistics.Count > 0)
                {
                    var collection = this.restService.Get<SemesterStatistics>($"Grades/Semester/Statistics");
                    AllSemesterStatistics.Clear();
                    foreach (var e in collection)
                        AllSemesterStatistics.Add(e);
                    (HideAllSemesterStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }
            });

            this.Messenger.Register<SemesterStatisticsWindowViewModel, string, string>(this, "GradeUpdated", (receipient, msg) =>
            {
                if (SemesterStatistics != null)
                {
                    var year = SemesterStatistics.Semester.Replace("/", "-");
                    var stat = this.restService.GetSingle<SemesterStatistics>($"Grades/Semester/Statistics/{year}");
                    SemesterStatistics = stat;
                    (HideSemesterStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }

                if (AllSemesterStatistics.Count > 0)
                {
                    var collection = this.restService.Get<SemesterStatistics>($"Grades/Semester/Statistics");
                    AllSemesterStatistics.Clear();
                    foreach (var e in collection)
                        AllSemesterStatistics.Add(e);
                    (HideAllSemesterStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }
            });
        }

        public void Dispose()
        {
            this.Messenger.UnregisterAll(this);
        }
     }
}
