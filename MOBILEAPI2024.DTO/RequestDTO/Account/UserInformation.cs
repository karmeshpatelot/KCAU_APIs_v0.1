using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Account
{
    public class UserInformation
    {
        public int UserID { get; set; }
        public string IPAddress { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string City { get; set; }
        public string ConnectionType { get; set; }
        public string Browser { get; set; }
        public string OperatingSystem { get; set; }
        public string DeviceType { get; set; }
        public string WeatherInfo { get; set; }
        public string Timezone { get; set; }
        public string Language { get; set; }
    }

}
