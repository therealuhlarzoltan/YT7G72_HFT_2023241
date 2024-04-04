using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.WpfClient.Logic;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class TeacherStatisticsWindowViewModel : ObservableRecipient, IDisposable
    {
        private RestService restService;
        private AcademicRank? selectedAcademicRank = null;
        private ObservableCollection<AverageByPersonDTO<Teacher>> bestTeachers = new ObservableCollection<AverageByPersonDTO<Teacher>>();
        private ObservableCollection<AverageByPersonDTO<Teacher>> teacherStatistics = new ObservableCollection<AverageByPersonDTO<Teacher>>();
        public ObservableCollection<AverageByPersonDTO<Teacher>> TeacherStatistics
        {
            get { return teacherStatistics; }
            set { SetProperty(ref teacherStatistics, value); }
        }

        public ObservableCollection<AverageByPersonDTO<Teacher>> BestTeachers
        {
            get { return bestTeachers; }
            set { SetProperty(ref bestTeachers, value); }
        }

        public AcademicRank? SelectedAcademicRank
        {
            get { return selectedAcademicRank; }
            set { SetProperty(ref selectedAcademicRank, value); (ShowBestTeachersCommand as RelayCommand)?.NotifyCanExecuteChanged(); }
        }

        public AcademicRank[] AcademicRanks { get; set; } = (AcademicRank[])Enum.GetValues(typeof(AcademicRank));
        public ICommand ShowBestTeachersCommand { get; set; }
        public ICommand HideBestTeachersCommand { get; set; }
        public ICommand GetTeacherStatisticsCommand { get; set; }
        public ICommand HideTeacherStatisticsCommand { get; set; }
        public TeacherStatisticsWindowViewModel(RestService restService)
        {
            this.restService = restService;

            GetTeacherStatisticsCommand = new RelayCommand(
                () => {
                    var collection = this.restService.Get<AverageByPersonDTO<Teacher>>($"People/Teachers/Best");
                    TeacherStatistics.Clear();
                    foreach (var e in collection)
                        TeacherStatistics.Add(e);
                    (HideTeacherStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                    if (TeacherStatistics.Count == 0)
                        MessageBox.Show("No statistics were found", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                 }
            );


            HideTeacherStatisticsCommand = new RelayCommand(
                () =>
                {
                    TeacherStatistics.Clear();
                    (HideTeacherStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                },
                () => TeacherStatistics.Count > 0);


            ShowBestTeachersCommand = new RelayCommand(
              () => {
                  var collection = this.restService.Get<AverageByPersonDTO<Teacher>>($"People/Teachers/Best/{(int)SelectedAcademicRank}");
                  BestTeachers.Clear();
                  foreach (var e in collection)
                      BestTeachers.Add(e);
                  (HideBestTeachersCommand as RelayCommand)?.NotifyCanExecuteChanged();
                  if (BestTeachers.Count == 0)
                      MessageBox.Show("No statistics were found", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
              },
              () => SelectedAcademicRank != null
            );

            HideBestTeachersCommand = new RelayCommand(
                () =>
                {
                    BestTeachers.Clear();
                    (HideBestTeachersCommand as RelayCommand)?.NotifyCanExecuteChanged();
                },
                () => BestTeachers.Count > 0);

            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<TeacherStatisticsWindowViewModel, string, string>(this, "GradeCreated", (recipient, msg) =>
            {
                if (BestTeachers.Count > 0 || SelectedAcademicRank != null)
                {
                    var collection = this.restService.Get<AverageByPersonDTO<Teacher>>($"People/Teachers/Best/{(int)SelectedAcademicRank}");
                    BestTeachers.Clear();
                    foreach (var e in collection)
                        BestTeachers.Add(e);
                    (HideBestTeachersCommand as RelayCommand)?.NotifyCanExecuteChanged();
                    if (BestTeachers.Count == 0)
                        MessageBox.Show("No statistics were found", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                if (TeacherStatistics.Count > 0)
                {
                    var collection = this.restService.Get<AverageByPersonDTO<Teacher>>($"People/Teachers/Best");
                    TeacherStatistics.Clear();
                    foreach (var e in collection)
                        TeacherStatistics.Add(e);
                    (HideTeacherStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                    if (TeacherStatistics.Count == 0)
                        MessageBox.Show("No statistics were found", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });

            this.Messenger.Register<TeacherStatisticsWindowViewModel, string, string>(this, "GradeUpdated", (recipient, msg) =>
            {
                if (BestTeachers.Count > 0 || SelectedAcademicRank != null)
                {
                    var collection = this.restService.Get<AverageByPersonDTO<Teacher>>($"People/Teachers/Best/{(int)SelectedAcademicRank}");
                    BestTeachers.Clear();
                    foreach (var e in collection)
                        BestTeachers.Add(e);
                    (HideBestTeachersCommand as RelayCommand)?.NotifyCanExecuteChanged();
                    if (BestTeachers.Count == 0)
                        MessageBox.Show("No statistics were found", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }

                if (TeacherStatistics.Count > 0)
                {
                    var collection = this.restService.Get<AverageByPersonDTO<Teacher>>($"People/Teachers/Best");
                    TeacherStatistics.Clear();
                    foreach (var e in collection)
                        TeacherStatistics.Add(e);
                    (HideTeacherStatisticsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                    if (TeacherStatistics.Count == 0)
                        MessageBox.Show("No statistics were found", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            });
        }

        public void Dispose()
        {
            this.Messenger.UnregisterAll(this);
        }
    }
}
