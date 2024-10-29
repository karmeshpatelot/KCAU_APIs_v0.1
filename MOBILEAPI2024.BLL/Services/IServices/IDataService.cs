using MOBILEAPI2024.DTO.RequestDTO.Account;
using MOBILEAPI2024.DTO.RequestDTO.Attendance;
using MOBILEAPI2024.DTO.RequestDTO.Employee;
using MOBILEAPI2024.DTO.ResponseDTO.Account;
using MOBILEAPI2024.DTO.ResponseDTO.Attendance;
using MOBILEAPI2024.DTO.ResponseDTO.Employee;
using MOBILEAPI2024.DTO.ResponseDTO.Student;
using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MOBILEAPI2024.BLL.Services.IServices
{
    public interface IDataService
    {
        void DeleteExpiredOTP();
        ReportResponse GetInOutData(int month);
        LoginAPIResponse GetLoginUser();
        Staff GetStaff(string? staffNo, int entriesPerPage, int pageNo);
        Staff GetStaffFromERP(string? staffNo, int entriesPerPage, int pageNo);
        Staff GetStaffFromERPForLogin(string? staffNo, int entriesPerPage, int pageNo);
        Students GetStudents(string? studentNo, int entriesPerPage, int pageNo);
        void InsertInOutData(Record record);
        object LoginUser(LoginModel loginModel);
        object OtpVerification(string userName, int otpCode);
        void SendEmailOTP(Student student);
        int GetTotalDaysInMonth(int? month = null);
        void SendEmailOTPStaff(Result result);
        PunchModel CheckDataExists(string? userId, string? datetime);
        void AddCampusAttendance(AddCampusInOut addCampusInOut);
        void AddLectureAttendance(AddLectureAttendance addLectureAttendance);
        LoginResponse GetLoginAccess();
        Students GetStudentsData(string? studentNo, int entriesPerPage, int pageNo);
        AttendanceResponse GetAttendanceFromBS(string sessionId, string deviceId);
        Students GetAttendanceFromDB(EventCollection getAttendance, LoginResponse getLoginAccess, string deviceId, DeviceConfigurationKCA checkDeviceConfig);
        List<StudentList> GetStudentsListForFilter();
        AttendanceSummery GetFilteredData(string studentNo, int month, int year,DateTime? date);
        List<LoginReportKCA> GetLoginReport(DateTime date);
        string SendEmailToAdmin(string username);
        List<DayWiseAttendance> DayWiseAttendance();
        DashboardCount GetDashboardCounts();
        string CheckCampusAttendance(AddCampusInOut addCampusInOut);
        List<ClassAttendanceForCal> GetClassAttendance(string? username, int month);
        List<CampusAttendance> GetCampusAttendance(string? username, int month);
        void PostDeviceConfig(DeviceConfig deviceConfig);
        DeviceConfigurationKCA CheckDeviceConfig(string deviceId);
        void DeleteDeviceConfig(string deviceId);
    }
}
