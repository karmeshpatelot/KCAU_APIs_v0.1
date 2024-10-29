using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Employee
{
    public class OtpVerify
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string OTP { get; set; }
        public bool IsVerify { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
