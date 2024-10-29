using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Employee
{
    public class Students
    {
        public bool success { get; set; }
        public int pageNo { get; set; }
        public int maxEntriesinthispage { get; set; }
        public int totalentries { get; set; }
        public List<Student> Result { get; set; }
    }

    public class Student
    {
        public string sn { get; set; }
        public string student_no { get; set; }
        public string name { get; set; }
        public string campus { get; set; }
        public string date_registered { get; set; }
        public string intake_period { get; set; }
        public string dob { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Allow { get; set; }
        public decimal Bal { get; set; }
    }
}
