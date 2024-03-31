using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class CurriculumEditWindowViewModel : ObservableRecipient, IDisposable
    {
        private Curriculum curriculum;
        public Curriculum Curriculum { get { return curriculum; } set { SetProperty(ref curriculum, value); } }
        public ICommand SaveChangesCommand { get; set; }

        public CurriculumEditWindowViewModel(Curriculum curriculum)
        {
            Curriculum = new Curriculum()
            {
                CurriculumId = curriculum.CurriculumId,
                CurriculumCode = curriculum.CurriculumCode,
                CurriculumName = curriculum.CurriculumName
            };

            SaveChangesCommand = new RelayCommand(
                () =>
                {
                    this.Messenger.Send(Curriculum, "CurriculumUpdateRequested");
                }
            );

            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<CurriculumEditWindowViewModel, string, string>(this, "FailedToUpdateCurriculum", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
            this.Messenger.Register<CurriculumEditWindowViewModel, string, string>(this, "CurriculumUpdated", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send("Finished", "CurriculumUpdateFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
