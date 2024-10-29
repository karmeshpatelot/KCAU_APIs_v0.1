using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace MOBILEAPI2024.DTO.ResponseDTO.Employee
{
    public class UserPreference
    {
        public string Language { get; set; }
        public string Date_Format { get; set; }
        public string Time_Format { get; set; }
        public string Depend_On_Os { get; set; }
        public string Default { get; set; }
        public string Time_Zone { get; set; }
        public string Dst1 { get; set; }
        public string Dst2 { get; set; }
    }

    public class UserRole
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }

    public class LoginAPIResponse
    {
        public string User_Id { get; set; }
        public string Name { get; set; }
        public string Language { get; set; }
        public int Perm_Id { get; set; }
        public UserPreference Preference { get; set; }
        public List<UserRole> Roles { get; set; }
        public string Message { get; set; }
        public string Message_Key { get; set; }
        public string Status_Code { get; set; }
    }

}
