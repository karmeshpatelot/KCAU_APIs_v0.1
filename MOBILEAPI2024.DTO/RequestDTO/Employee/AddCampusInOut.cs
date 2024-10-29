using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Employee
{
    public class AddCampusInOut
    {
        public string UserName { get; set; }
        public string Campus { get; set; }
        public string IOFlag { get; set; }
        public DateTime DateTime { get; set; }
    }
}
