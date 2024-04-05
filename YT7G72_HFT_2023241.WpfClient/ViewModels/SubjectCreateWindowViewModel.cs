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

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class SubjectCreateWindowViewModel : ObservableRecipient, IDisposable
    {
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
        private Subject subject;
        public Subject Subject { get { return subject; } set { SetProperty(ref subject, value); } }
        public Requirement[] Requirements { get; set; } = (Requirement[])Enum.GetValues(typeof(Requirement));
        public ICommand CreateSubjectCommand { get; set; }

        public SubjectCreateWindowViewModel()
        {
            Subject = new Subject();

            TeacherIdFKString = string.Empty;
            PreReqFKString = string.Empty;

            CreateSubjectCommand = new RelayCommand(
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
                        this.Messenger.Send(Subject, "SubjectCreationRequested");
                    }
                    catch (Exception e) when (e is FormatException || e is OverflowException)
                    {
                        MessageBox.Show("Invalid argument(s) provided!", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            );

            RegisterMessengers();
        }

        private void RegisterMessengers()
        {
            this.Messenger.Register<SubjectCreateWindowViewModel, string, string>(this, "FailedToCreateSubject", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            });
            this.Messenger.Register<SubjectCreateWindowViewModel, string, string>(this, "SubjectCreated", (recipient, msg) =>
            {
                MessageBox.Show(msg, "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        }


        public void Dispose()
        {
            this.Messenger.Send<string, string>("Finished", "SubjectCreationFinished");
            this.Messenger.UnregisterAll(this);
        }
    }
}
