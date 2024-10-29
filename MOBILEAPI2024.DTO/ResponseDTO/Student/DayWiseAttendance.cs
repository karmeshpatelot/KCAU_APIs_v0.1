using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Student
{
    public class DayWiseAttendance
    {
        public string UserName { get; set; }
        public string EnrollNo { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
    }
}
