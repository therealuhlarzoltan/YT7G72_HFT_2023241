using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.WpfClient.Logic;

namespace YT7G72_HFT_2023241.WpfClient.Services.Interfaces
{
    interface ITeacherStatisticsDisplay
    {
        void Display(RestService restService);
    }
}
