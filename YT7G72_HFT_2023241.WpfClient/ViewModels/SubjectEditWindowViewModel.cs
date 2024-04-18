﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
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

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class SubjectEditWindowViewModel : ObservableRecipient, IDisposable
    {

        private IMessageBoxService messageBoxService;
        private Subject subject;
        private string preReqFKString;
        public string PreReqFKString
        {
            get { return preReqFKString; }
            set { SetProperty(ref preReqFKString, value); }
        }

        private string teacherIdFKString;
        public string TeacherIdFKString
        {
            get { return teacherIdFKString; }
            set { SetProperty(ref teacherIdFKString, value); }
        }
        public Subject Subject { get { return subject; } set { SetProperty(ref subject, value); } }
        public Requirement[] Requirements { get; set; } = (Requirement[])Enum.GetValues(typeof(Requirement));
        public ICommand SaveChangesCommand { get; set; }

        public SubjectEditWindowViewModel(Subject subject, IMessageBoxService messageBoxService)
        {
            this.messageBoxService = messageBoxService;
            Subject = new Subject()
            { 
                SubjectId = subject.SubjectId,
                SubjectCode = subject.SubjectCode,
                SubjectName = subject.SubjectName,
                Credits = subject.Credits,
                PreRequirementId = subject.PreRequirementId,
                Requirement = subject.Requirement,
                CurriculumId = subject.CurriculumId,
                TeacherId = subject.TeacherId,
            };
            PreReqFKString = subject?.PreRequirementId != null ? subject.PreRequirementId.ToString() : string.Empty;
            TeacherIdFKString = subject?.TeacherId != null ? subject.TeacherId.ToString() : string.Empty;

            SaveChangesCommand = new RelayCommand(
                () =>
                {
                    int? preId = null;
                    int? tId = null;

                    try
                    {
                        if (!string.IsNullOrWhiteSpace(PreReqFKString))
                            preId = int.Parse(PreReqFKString);
                        if (!string.IsNullOrWhiteSpace(TeacherIdFKString))
                            tId = int.Parse(TeacherIdFKString);

                        Subject.PreRequirementId = preId;
                        Subject.TeacherId = tId;
                        this.Messenger.Send(Subject, "SubjectUpdateRequested");

                    }
                    catch (Exception e) when (e is FormatException || e is OverflowException)
                    {
                        messageBoxService.ShowWarning("Invalid argument(s) provided!");
                    }
                }
            );

            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<SubjectEditWindowViewModel, string, string>(this, "FailedToUpdateSubject", (recipient, msg) =>
            {
                messageBoxService.ShowWarning(msg);
            });
            this.Messenger.Register<SubjectEditWindowViewModel, string, string>(this, "SubjectUpdated", (recipient, msg) =>
            {
                messageBoxService.ShowInfo(msg);
            });
        }


        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "SubjectUpdateFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
