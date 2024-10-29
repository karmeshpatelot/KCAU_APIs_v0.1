using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Student
{
    public class DashboardCount
    {
        public int PaidCount { get; set; }
        public int UnPaidCount { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
    }
}
