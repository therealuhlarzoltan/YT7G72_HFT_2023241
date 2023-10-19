using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.Repository;

namespace YT7G72_HFT_2023241.Logic
{
    public class PersonLogic : IPersonLogic
    {
        private IRepository<Student> studentRepository;
        private IRepository<Teacher> teacherRepository;

        public PersonLogic(IRepository<Student> studentRepository, IRepository<Teacher> teacherRepository)
        {
            this.studentRepository = studentRepository;
            this.teacherRepository = teacherRepository;
        }

        public void AddStudent(Student student)
        {
            
            studentRepository.Create(student);
        }

        public void AddTeacher(Teacher teacher)
        {
            
            teacherRepository.Create(teacher);
        }

        public IEnumerable<Student> GetBestStudents()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Teacher> GetBestTeachers()
        {
            throw new NotImplementedException();
        }

        public Student GetStudent(int id)
        {
            var student = studentRepository.Read(id);
            if (student == null)
                throw new PersonNotFoundException();
            return student;
        }

        public IEnumerable<Student> GetStudents()
        {
            return studentRepository.ReadAll();
        }

        public Teacher GetTeacher(int id)
        {
            var teacher = teacherRepository.Read(id);
            if (teacher == null)
                throw new PersonNotFoundException();
            return teacher;
        }

        public IEnumerable<Teacher> GetTeachers()
        {
            return teacherRepository.ReadAll();
        }

        public void RemoveStudent(int id)
        {
            var student = teacherRepository.Read(id);
            if (student == null)
                throw new PersonNotFoundException();
            studentRepository.Delete(id);
        }

        public void RemoveTeacher(int id)
        {
            var teacher = teacherRepository.Read(id);
            if (teacher == null)
                throw new PersonNotFoundException();
            teacherRepository.Delete(id);
        }

        public void UpdateStudent(Student student)
        {
            var originalStudent = studentRepository.Read(student.Id);
            if (originalStudent == null)
                throw new PersonNotFoundException();
            studentRepository.Update(student);
        }

        public void UpdateTeacher(Teacher teacher)
        {
            var originalTeacher = teacherRepository.Read(teacher.Id);
            if (originalTeacher == null)
                throw new PersonNotFoundException();
            teacherRepository.Update(teacher);
        }

        public string GetSchedule(Student student)
        {
            return "";
        }
    }
}
