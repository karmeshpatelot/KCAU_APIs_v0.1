using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Employee
{
    public class AttendanceEventKCA
    {
        public int Id { get; set; } // Primary key with identity
        public string RowId { get; set; }
        public DateTime ServerDateTime { get; set; }
        public DateTime DateTime { get; set; } // Changed to DateTime as per SQL type
        public string Parameter { get; set; }
        public string EventIndex { get; set; } // Renamed from index
        public string UserIdName { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string UserPhotoExists { get; set; }
        public string UserGroupId { get; set; }
        public string UserGroupName { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string EventTypeId { get; set; }
        public string EventTypeCode { get; set; }
        public string UserUpdateByDevice { get; set; }
        public string Hint { get; set; }
    }
}
