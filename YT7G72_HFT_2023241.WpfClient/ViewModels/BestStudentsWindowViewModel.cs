using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.ObjectModel;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.WpfClient.Logic;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class BestStudentsWindowViewModel : ObservableRecipient, IDisposable
    {
        private RestService restService;
        private ObservableCollection<AverageByPersonDTO<Student>> bestStudents = new ObservableCollection<AverageByPersonDTO<Student>>();
        public ObservableCollection<AverageByPersonDTO<Student>> BestStudents
        {
            get { return bestStudents; }
            set
            {
                SetProperty(ref bestStudents, value);
            }
        }

        public BestStudentsWindowViewModel(RestService restService)
        {
            this.restService = restService;
            var collection = restService.Get<AverageByPersonDTO<Student>>("People/Students/Best");
            BestStudents.Clear();
            foreach (var e in collection)
                BestStudents.Add(e);

            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<BestStudentsWindowViewModel, string, string>(this, "GradeCreated", (recipient, msg) =>
            {
                var collection = restService.Get<AverageByPersonDTO<Student>>("People/Students/Best");
                BestStudents.Clear();
                foreach (var e in collection)
                    BestStudents.Add(e);
            });

            this.Messenger.Register<BestStudentsWindowViewModel, string, string>(this, "GradeDeleted", (recipient, msg) =>
            {
                var collection = restService.Get<AverageByPersonDTO<Student>>("People/Students/Best");
                BestStudents.Clear();
                foreach (var e in collection)
                    BestStudents.Add(e);
            });

            this.Messenger.Register<BestStudentsWindowViewModel, string, string>(this, "StudentDeleted", (recipient, msg) =>
            {
                var collection = restService.Get<AverageByPersonDTO<Student>>("People/Students/Best");
                BestStudents.Clear();
                foreach (var e in collection)
                    BestStudents.Add(e);
            });

            this.Messenger.Register<BestStudentsWindowViewModel, string, string>(this, "StudentUpdated", (recipient, msg) =>
            {
                var collection = restService.Get<AverageByPersonDTO<Student>>("People/Students/Best");
                BestStudents.Clear();
                foreach (var e in collection)
                    BestStudents.Add(e);
            });

            this.Messenger.Register<BestStudentsWindowViewModel, string, string>(this, "SubjectDeleted", (recipient, msg) =>
            {
                var collection = restService.Get<AverageByPersonDTO<Student>>("People/Students/Best");
                BestStudents.Clear();
                foreach (var e in collection)
                    BestStudents.Add(e);
            });

            this.Messenger.Register<BestStudentsWindowViewModel, string, string>(this, "CurriculumDeleted", (recipient, msg) =>
            {
                var collection = restService.Get<AverageByPersonDTO<Student>>("People/Students/Best");
                BestStudents.Clear();
                foreach (var e in collection)
                    BestStudents.Add(e);
            });

            this.Messenger.Register<BestStudentsWindowViewModel, string, string>(this, "GradeUpdated", (recipient, msg) =>
            {
                var collection = restService.Get<AverageByPersonDTO<Student>>("People/Students/Best");
                BestStudents.Clear();
                foreach (var e in collection)
                    BestStudents.Add(e);
            });
        }

        public void Dispose()
        {
            this.Messenger.UnregisterAll(this);
        }
    }
}