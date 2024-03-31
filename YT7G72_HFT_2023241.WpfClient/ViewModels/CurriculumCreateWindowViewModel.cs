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
    public class CurriculumCreateWindowViewModel : ObservableRecipient, IDisposable
    {
        private Curriculum curriculum;
        public Curriculum Curriculum { get { return curriculum; } set { SetProperty(ref curriculum, value); } }
        public ICommand CreateCurriculumCommand { get; set; }

        public CurriculumCreateWindowViewModel()
        {
            Curriculum = new Curriculum();

            CreateCurriculumCommand = new RelayCommand(
                () =>
                {
                    this.Messenger.Send(Curriculum, "CurriculumCreationRequested");
                }
            );

            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<CurriculumCreateWindowViewModel, string, string>(this, "FailedToCreateCurriculum", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
            this.Messenger.Register<CurriculumCreateWindowViewModel, string, string>(this, "CurriculumCreated", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send("Finished", "CurriculumCreationFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
