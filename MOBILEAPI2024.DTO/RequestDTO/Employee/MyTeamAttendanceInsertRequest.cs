using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Employee
{
    public class MyTeamAttendanceInsertRequest
    {
        public int EmpId { get; set; }
        public int CmpId { get; set; }
        public string Details { get; set; }
        public string Address { get; set; }
        public string ImageName { get; set; }
        public string strType { get; set; }
    }
}
