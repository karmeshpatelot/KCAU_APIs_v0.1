﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Employee
{
    public class LoginReportKCA
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Type { get; set; }
        public DateTime LoginTime { get; set; }
    }
}