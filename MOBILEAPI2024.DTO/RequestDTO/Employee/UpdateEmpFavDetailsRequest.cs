using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Employee
{
    public class UpdateEmpFavDetailsRequest
    {
        public int EmpID { get; set; }
        public int CmpID { get; set; }
        public int EmpFavSportID { get; set; }
        public string EmpFavSportName { get; set; }
        public int EmpHobbyID { get; set; }
        public string EmpHobbyName { get; set; }
        public string EmpFavFood { get; set; }
        public string EmpFavRestro { get; set; }
        public string EmpFavTrvDest { get; set; }
        public string EmpFavFest { get; set; }
        public string EmpFavSportPerson { get; set; }
        public string EmpFavSinger { get; set; }
        public string? Type { get; set; }
    }
}
