using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Employee
{

    public class Staff
    {
        public bool success { get; set; }
        public int pageNo { get; set; }
        public int maxEntriesinthispage { get; set; }
        public int totalentries { get; set; }
        public List<Result> Result { get; set; }
    }

    public class Result
    {
        public string sn { get; set; }
        public string staff_no { get; set; }
        public string first_name { get; set; }
        public string middle_name { get; set; }
        public string last_name { get; set; }
        public string campus { get; set; }
        public string date_joined { get; set; }
        public string department { get; set; }
        public string job_title { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Allow { get; set; }
    }

}
