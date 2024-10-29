using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Account
{
    public class LoginDTO
    {

        [Required(ErrorMessage = "UserName is required")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }

        //[Required(ErrorMessage = "IMEINo is required")]
        public string? IMEINo { get; set; }

        //[Required(ErrorMessage = "DeviceID is required")]
        public string? DeviceID { get; set; }

    }
}
