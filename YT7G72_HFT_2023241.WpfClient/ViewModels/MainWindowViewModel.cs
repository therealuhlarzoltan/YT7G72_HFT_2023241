using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;
using YT7G72_HFT_2023241.WpfClient.Logic;
using YT7G72_HFT_2023241.Models;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System.Threading;
using System.Runtime.CompilerServices;
using YT7G72_HFT_2023241.WpfClient.Services;

namespace YT7G72_HFT_2023241.WpfClient.ViewModels
{
    public class MainWindowViewModel : ObservableRecipient, INotifyPropertyChanged
    {
        private string errorMessage;
        private RestCollection<Student> students;
        private RestCollection<Teacher> teachers;
        private RestCollection<Subject> subjects;
        private RestCollection<Course> courses;
        private RestCollection<Grade> grades;
        private RestCollection<Curriculum> curriculums;
        private Student selectedStudent;
        private Teacher selectedTeacher;
        private Subject selectedSubject;
        private Course selectedCourse;
        private Grade selectedGrade;
        private Curriculum selectedCurriculum;
        public event PropertyChangedEventHandler? PropertyChanged;

        public RestCollection<Student> Students { get { return students; } set { SetProperty(ref students, value); } }
        public RestCollection<Teacher> Teachers { get { return teachers; } set { SetProperty(ref teachers, value);} }
        public RestCollection<Subject> Subjects { get { return subjects; } set { SetProperty(ref subjects, value); } }
        public RestCollection<Course> Courses { get { return courses; } set { SetProperty(ref courses, value); } }
        public RestCollection<Grade> Grades { get { return grades; } set { SetProperty(ref grades, value); } }
        public RestCollection<Curriculum> Curriculums { get { return curriculums; } set { SetProperty(ref curriculums, value); } }
        public Student SelectedStudent
        {
            get
            {
                return selectedStudent;
            }
            set { 
                SetProperty(ref selectedStudent, value);
                (UpdateStudentCommand as RelayCommand).NotifyCanExecuteChanged();
                (DeleteStudentCommand as RelayCommand).NotifyCanExecuteChanged();
            }
        }
        public ICommand CreateStudentCommand { get; set; }
        public ICommand UpdateStudentCommand { get; set; }

        public ICommand DeleteStudentCommand { get; set; }
        public ICommand CreateTeacherCommand { get; set; }
        public ICommand UpdateTeacherCommand { get; set; }

        public ICommand DeleteTeacherCommand { get; set; }
        public ICommand CreateSubjectCommand { get; set; }
        public ICommand UpdateSubjectCommand { get; set; }

        public ICommand DeleteSubjectCommand { get; set; }
        public ICommand CreateCourseCommand { get; set; }
        public ICommand UpdateCourseCommand { get; set; }

        public ICommand DeleteCourseCommand { get; set; }
        public ICommand CreateGradeCommand { get; set; }
        public ICommand UpdateGradeCommand { get; set; }

        public ICommand DeleteGradeCommand { get; set; }
        public ICommand CreateCurriculumCommand { get; set; }
        public ICommand UpdateCurriculumCommand { get; set; }

        public ICommand DeleteCurriculumCommand { get; set; }



        public string ErrorMessage
        {
            get { return errorMessage; }
            set { SetProperty(ref errorMessage, value); }
        }

        public static bool IsInDesignMode
        {
            get
            {
                var prop = DesignerProperties.IsInDesignModeProperty;
                return (bool)DependencyPropertyDescriptor.FromProperty(prop, typeof(FrameworkElement)).Metadata.DefaultValue;
            }
        }


        public  MainWindowViewModel()
        {
            if (!IsInDesignMode)
            {
                Students = new RestCollection<Student>("http://localhost:4180/", "people/students", "hub");
                Thread.Sleep(2000);
                Teachers = new RestCollection<Teacher>("http://localhost:4180/", "people/teachers", "hub");
                Thread.Sleep(2000);
                Subjects = new RestCollection<Subject>("http://localhost:4180/", "education/subjects", "hub");
                Thread.Sleep(2000);
                Courses = new RestCollection<Course>("http://localhost:4180/", "education/courses", "hub");
                Thread.Sleep(2000);
                Grades = new RestCollection<Grade>("http://localhost:4180/", "grades", "hub");
                Thread.Sleep(2000);
                Curriculums = new RestCollection<Curriculum>("http://localhost:4180/", "curriculums", "hub");
                UpdateStudentCommand = new RelayCommand(
                    () => new StudentEditorService().Edit(Students, SelectedStudent),
                    () => selectedStudent != null
                );
                DeleteStudentCommand = new RelayCommand(
                    () => Students.Delete(SelectedStudent.StudentId),
                    () => SelectedStudent != null
                ); 

            }
        }


        private void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
