using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Attendance
{
    public class CampusAttendance
    {
        public string UserName { get; set; }
        public DateTime ForDate { get; set; }
        public DateTime? InTime { get; set; }
        public DateTime? OutTime { get; set; }

        public string? Duration { get; set; }
        public string? Status { get; set; }
    }
}
