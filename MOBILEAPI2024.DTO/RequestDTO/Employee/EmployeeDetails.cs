using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Employee
{
    public class EmployeeDetails
    {
        public int Emp_ID { get; set; }
        public int Cmp_ID { get; set; }
        public int Vertical_ID { get; set; }
        public string Emp_Code { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Pincode { get; set; }
        public string PhoneNo { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string ImageName { get; set; }
        public int Branch_ID { get; set; }
        public int Department_ID { get; set; }
        public string Type { get; set; }
    }
}
