using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Employee
{
    public class AdminModel
    {
        public int Id { get; set; }
        public string AdminName { get; set; }
        public string Email { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
