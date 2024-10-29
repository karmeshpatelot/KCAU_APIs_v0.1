using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Account
{
    public class LoginModel
    {
        public string Type { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
