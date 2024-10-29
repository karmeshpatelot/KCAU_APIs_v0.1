using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Employee
{
    public class PunchModel
    {
       
            public int Id { get; set; } // Primary key with identity
            public string UserName { get; set; } // nvarchar(max)
            public string EnrollNo { get; set; } // nvarchar(max)
            public string LocationType { get; set; } // nvarchar(max)
            public DateTime ForDate { get; set; } // datetime, can be nullable
            public DateTime InTime { get; set; } // datetime, can be nullable
            public DateTime OutTime { get; set; } // datetime, can be nullable
            public string Duration { get; set; } // nvarchar(max)
            public string IpAddress { get; set; } // nvarchar(max)
            public string DeviceId { get; set; } // nvarchar(max)
            public string FeesStatus { get; set; } // nvarchar(max)
        public DateTime? CreatedDate { get; set; } // datetime, can be nullable
      
    }
}
