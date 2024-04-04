using Microsoft.Extensions.DependencyInjection;
using Microsoft.Toolkit.Mvvm.DependencyInjection;
using Microsoft.Toolkit.Mvvm.Messaging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using YT7G72_HFT_2023241.WpfClient.Services.Implementations;
using YT7G72_HFT_2023241.WpfClient.Services.Interfaces;
using YT7G72_HFT_2023241.WpfClient.ViewModels;

namespace YT7G72_HFT_2023241.WpfClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Ioc.Default.ConfigureServices(
                   new ServiceCollection()
                    .AddSingleton<IStudentCreator, StudentCreatorService>()
                    .AddSingleton<IStudentEditor, StudentEditorService>()
                    .AddSingleton<ITeacherCreator, TeacherCreatorService>()
                    .AddSingleton<ITeacherEditor, TeacherEditorService>()
                    .AddSingleton<ISubjectCreator, SubjectCreatorService>()
                    .AddSingleton<ISubjectEditor, SubjectEditorService>()
                    .AddSingleton<ICourseCreator, CourseCreatorService>()
                    .AddSingleton<ICourseEditor, CourseEditorService>()
                    .AddSingleton<IGradeEditor, GradeEditorService>()
                    .AddSingleton<IGradeCreator, GradeCreatorService>()
                    .AddSingleton<ICurriculumCreator, CurriculumCreatorService>()
                    .AddSingleton<ICurriculumEditor, CurriculumEditorService>()
                    .AddSingleton<ISemesterStatisticsDisplay, SemesterStatisticsDisplay>()
                    .AddSingleton<ITeacherStatisticsDisplay, TeacherStatisticsDisplay>()
                    .AddSingleton<IStudentStatisticsDisplay, StudentStatisticsDisplay>()
                    .AddSingleton<IMessenger>(WeakReferenceMessenger.Default)
                    .BuildServiceProvider()
             );
        }
    }
}
