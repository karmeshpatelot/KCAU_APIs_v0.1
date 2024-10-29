using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Attendance
{
    public class InOutRecord
    {
        public int Id { get; set; }              // Record ID
        public string UserName { get; set; }      // User Name or Enrollment Number
        public string Campus { get; set; }        // Campus Name
        public DateTime InOutTime { get; set; }   // Timestamp for In/Out
        public DateTime ForDate { get; set; }     // Date for the record
        public string IOFlag { get; set; }        // IN/OUT flag
        public DateTime CreatedDate { get; set; } // Record creation timestamp
    }

}
