using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Attendance
{
    public class DeviceConfig
    {
        public string DeviceId { get; set; }
        public string Location { get; set; }
        public string Type { get; set; }
    }

    public class DeviceConfigurationKCA
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string Location { get; set; }
        public string CampusName { get; set; }
        public string Type { get; set; }
        public DateTime CreatedDate { get; set; }
    }

}
