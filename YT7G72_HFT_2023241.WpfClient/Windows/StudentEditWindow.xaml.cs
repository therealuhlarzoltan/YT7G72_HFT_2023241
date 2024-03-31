﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YT7G72_HFT_2023241.WpfClient.ViewModels;

namespace YT7G72_HFT_2023241.WpfClient.Windows
{
    /// <summary>
    /// Interaction logic for StudentEditWindow.xaml
    /// </summary>
    public partial class StudentEditWindow : Window
    {
        public StudentEditWindow()
        {
            InitializeComponent();
        }

        public StudentEditWindow(StudentEditWindowViewModel viewModel) : this()
        {
            this.DataContext = viewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            (this.DataContext as StudentEditWindowViewModel)?.Dispose();
            base.OnClosed(e);
        }
    }
}
