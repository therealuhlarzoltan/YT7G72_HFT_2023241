﻿using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class CourseEditWindowViewModel : ObservableRecipient, IDisposable
    {
        public CourseEditWindowViewModel(Course course)
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
