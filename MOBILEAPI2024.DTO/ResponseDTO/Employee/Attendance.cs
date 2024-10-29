using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DTO.ResponseDTO.Employee
{
    public class Attendance
    {
        public string?  Date { get; set; }
        public string? InTime { get; set; }
        public string? OutTime { get; set; }
        public string? status { get; set; }
    }

    public class AttendanceDetails
    {
        public int TotalDays { get; set; }
        public int PresentDays { get; set; }
        public int AbsentDays { get; set; }
        public List<Attendance> Attendances { get; set; }

    }

    public class CustomAttendance
    {
        public string id { get; set; }
        public string start { get; set; }
        public string backgroundColor { get; set; }
        public string textColor { get; set; }
        public string color { get; set; }
        public string eventTextColor { get; set; }
        public string display { get; set; }
    }

    public class CustomAttendanceList
    {
        public List<CustomAttendance> attendances { get; set; }
        public List<InOut> inOuts{ get; set; }

    }
    public class InOut
    {
        public int id { get; set; }
        public string InTime { get; set; }
        public string OutTime { get; set; }
        
    }

    public class CustomAttendanceForClass
    {
        public int id { get; set; }
        public string title { get; set; }
        public DateTime start { get; set; }
        public string color { get; set; }
    }

    public class CustomAttendanceForClassList
    {
        public List<CustomAttendanceForClass> customAttendanceForClasses { get; set; }
    }

}
