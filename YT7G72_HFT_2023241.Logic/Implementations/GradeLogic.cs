using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;
using YT7G72_HFT_2023241.Repository;

namespace YT7G72_HFT_2023241.Logic
{
    public class GradeLogic : IGradeLogic
    {
        private IRepository<Grade> gradeRepository;

        public GradeLogic(IRepository<Grade> gradeRepository)
        {
            this.gradeRepository = gradeRepository;
        }


    }
}
