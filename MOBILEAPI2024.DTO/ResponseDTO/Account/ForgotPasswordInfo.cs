using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Account
{
    public class ForgotPasswordInfo
    {
        public int Login_ID { get; set; }
        public string Login_Name { get; set; }
        public int Cmp_ID { get; set; }
        public int Emp_ID { get; set; }
        public string Emp_Left { get; set; }
        public string Work_Email { get; set; }
        public string Other_Email { get; set; }
        public int EMAIL_NTF_SENT { get; set; }
        public string Emp_Full_Name { get; set; }
        public string Mobile_No { get; set; }

    }
}
