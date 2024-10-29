using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.RequestDTO.Employee
{
    public class EmployeeInOutRecord
    {
        public decimal? IO_Tran_Id { get; set; } // numeric(18, 0)
        public decimal? Emp_ID { get; set; } // numeric(18, 0)
        public decimal? Cmp_ID { get; set; } // numeric(18, 0)
        public DateTime? For_Date { get; set; } // datetime
        public DateTime? In_Time { get; set; } // datetime (Checked means non-null in SQL)
        public DateTime? Out_Time { get; set; } // datetime (Checked means non-null in SQL)
        public string? Duration { get; set; } // varchar(10)
        public string? Reason { get; set; } // varchar(100)
        public string? Ip_Address { get; set; } // varchar(50)
        public DateTime? In_Date_Time { get; set; } // datetime
        public DateTime? Out_Date_Time { get; set; } // datetime
        public decimal? Skip_Count { get; set; } // numeric(18, 0)
        public decimal? Late_Calc_Not_App { get; set; } // numeric(1, 0)
        public byte? Chk_By_Superior { get; set; } // tinyint
        public string? Sup_Comment { get; set; } // varchar(100)
        public string? Half_Full_day { get; set; } // varchar(20)
        public byte? Is_Cancel_Late_In { get; set; } // tinyint
        public byte? Is_Cancel_Early_Out { get; set; } // tinyint
        public byte? Is_Default_In { get; set; } // tinyint
        public byte? Is_Default_Out { get; set; } // tinyint
        public decimal? Cmp_prp_in_flag { get; set; } // numeric(5, 0)
        public decimal? Cmp_prp_out_flag { get; set; } // numeric(5, 0)
        public byte? is_Cmp_purpose { get; set; } // tinyint
        public DateTime? App_Date { get; set; } // datetime
        public DateTime? Apr_Date { get; set; } // datetime
        public DateTime? System_date { get; set; } // datetime
        public string? Other_Reason { get; set; } // varchar(MAX)
        public string? ManualEntryFlag { get; set; } // char(3)
        public string? StatusFlag { get; set; } // char(1)
        public string? In_Admin_Time { get; set; } // char(1)
        public string? Out_Admin_Time { get; set; } // char(1)
        public string? Document_No { get; set; } // nvarchar(MAX)
    }
}
