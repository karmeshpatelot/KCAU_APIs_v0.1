﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.Common
{
    public class EmailServiceOptionsSecond
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public bool UseSsl { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string CopyAddress { get; set; }
    }
}
