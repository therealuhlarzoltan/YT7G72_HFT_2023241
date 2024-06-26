﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YT7G72_HFT_2023241.WpfClient.Services.Interfaces
{
    public interface IMessageBoxService
    {
        public void ShowWarning(string message);
        public void ShowInfo(string message);
        public void ShowInfo(string message, string header);
    }
}
