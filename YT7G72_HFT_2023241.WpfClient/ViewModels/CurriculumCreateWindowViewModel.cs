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
using YT7G72_HFT_2023241.WpfClient.Services.Interfaces;
using Microsoft.Toolkit.Mvvm.DependencyInjection;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class CurriculumCreateWindowViewModel : ObservableRecipient, IDisposable
    {
        private IMessageBoxService messageBoxService;
        private Curriculum curriculum;
        public Curriculum Curriculum { get { return curriculum; } set { SetProperty(ref curriculum, value); } }
        public ICommand CreateCurriculumCommand { get; set; }

        public CurriculumCreateWindowViewModel() : this(Ioc.Default.GetService<IMessageBoxService>()) { }

        public CurriculumCreateWindowViewModel(IMessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;
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
                messageBoxService.ShowWarning(msg);
            });
            this.Messenger.Register<CurriculumCreateWindowViewModel, string, string>(this, "CurriculumCreated", (recipient, msg) =>
            {
                messageBoxService.ShowInfo(msg);
            });
        }

        public void Dispose()
        {
            this.Messenger.Send("Finished", "CurriculumCreationFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
