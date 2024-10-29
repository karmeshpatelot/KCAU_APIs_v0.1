using MOBILEAPI2024.DTO.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Student
{
    public class AttendanceReport
    {
        public string UserName { get; set; }
        public string EnrollNo { get; set; }
        public DateTime ForDate { get; set; }
        public DateTime? InTime { get; set; }
        public int? DeviceId { get; set; }
        public string FeesStatus { get; set; }
        public string AttendanceStatus { get; set; }
    }
    public class AttendanceSummery
    {
        public int Absent { get; set; }
        public int Present { get; set; }
        public string Name { get; set; }
        public List<AttendanceReport> Attendances { get; set; }
    }
}