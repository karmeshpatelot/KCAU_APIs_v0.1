using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Employee
{
    public class AttendanceRegularizeDetails
    {
        public int IOTranId { get; set; }
        public int? EmpID { get; set; }
        public int? CmpID { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime Fromdate { get; set; }
        public DateTime Todate { get; set; }
        public string Type { get; set; }
    }
}
