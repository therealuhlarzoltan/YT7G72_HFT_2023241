using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.Repository;

namespace YT7G72_HFT_2023241.Logic
{
    public class EducationLogic : IEducationLogic
    {
        private IRepository<Student> studentRepository;
        private IRepository<Course> courseRepository;
        private IRepository<Subject> subjectRepository;
        private IRepository<Curriculum> curriculumRepository;

        public EducationLogic(IRepository<Student> studentRepository, IRepository<Course> courseRepository, IRepository<Subject> subjectRepository, IRepository<Curriculum> curriculumRepository)
        {
            this.studentRepository = studentRepository;
            this.courseRepository = courseRepository;
            this.subjectRepository = subjectRepository;
            this.curriculumRepository = curriculumRepository;
        }
    }
}
