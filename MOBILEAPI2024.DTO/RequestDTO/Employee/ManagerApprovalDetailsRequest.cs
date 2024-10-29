using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Employee
{
    public class ManagerApprovalDetailsRequest
    {
        public int TravelApplicationId { get; set; }
        public int CmpId { get; set; }
        public int EmpId { get; set; }
        public int ClaimAppId { get; set; }
        public int LeaveApplicationId { get; set; }
        public string Flag { get; set; }
    }
}
