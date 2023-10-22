﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YT7G72_HFT_2023241.Models;

namespace YT7G72_HFT_2023241.Logic
{ 
    public class PreRequirementsNotMetException : Exception
    {
        public Student Student { get; }
        public Subject Subject { get; }

        public PreRequirementsNotMetException(Student student, Subject subject) : base()
        {
            Student = student;
            Subject = subject;
        }

        public override string ToString()
        {
            return $"Student {Student} did not meet the prerequirements for subject {Subject}";
        }
    }
}