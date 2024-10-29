using MOBILEAPI2024.DAL.Entities;
using MOBILEAPI2024.DTO.RequestDTO.Attendance;
using MOBILEAPI2024.DTO.RequestDTO.Employee;
using MOBILEAPI2024.DTO.ResponseDTO.Attendance;
using MOBILEAPI2024.DTO.ResponseDTO.Employee;
using MOBILEAPI2024.DTO.ResponseDTO.Student;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MOBILEAPI2024.DAL.Repositories.IRepositories
{
    public interface IDataRepository : IGenericRepository<ActiveInActiveUser>
    {
        void AddCampusAttendance(AddCampusInOut addCampusInOut);
        void AddLectureAttendance(AddLectureAttendance addLectureAttendance);
        void AddOtp(OTPModel otpModel, string otp);
        InOutRecord CheckCampusAttendance(AddCampusInOut addCampusInOut);
        PunchModel CheckDataExists(string? userId, string? datetime);
        DeviceConfigurationKCA CheckDeviceConfig(string deviceId);
        EmpBasic CheckEnrollNoExists(long? cmp_ID, string? enroll_No);
        bool CheckRecordExistence(Record record, int? cmp_ID, int? emp_ID);
        List<DayWiseAttendance> DayWiseAttendance();
        void DeleteDeviceConfig(string deviceId);
        void DeleteExpiredOTP();
        AdminModel GetAdminUser(string email);
        AttendanceEventKCA GetAttendanceFromDB(string deviceId, EventCollection getAttendance);
        List<CampusAttendance> GetCampusAttendance(string? username, DateTime fromDate, DateTime toDate);
        List<ClassAttendanceForCal> GetClassAttendance(string? username, DateTime fromDate, DateTime toDate);
        AttendanceCount GetDashbordAttendanceReport();
        FeesCounts GetFeesCounts();
        List<AttendanceReport> GetFilteredData(string studentNo, DateTime fromDate, DateTime toDate);
        List<LoginReportKCA> GetLoginReport(DateTime date);
        Staff GetStaffDataFromDB(string? staffNo, int entriesPerPage, int pageNo);
        Students GetStudentsData(string? studentNo, int entriesPerPage, int pageNo);
        List<StudentList> GetStudentsListForFilter();
        void InsertAttendanceEvents(EventCollection getAttendance1,string deviceId);
        void InsertInOutRecord(Record record, int? cmp_ID, int? emp_ID);
        void InsertStaffRecord(Result staff);
        void InsertStudentRecords(Student student);
        OtpVerify OtpVerification(string username, int otpCode);
        void PostDeviceConfig(DeviceConfig deviceConfig);
        void PostInOutPunch(PunchModel punchModel);
        void PostLoginReport(LoginReportKCA loginReportKCA);
        void UpdateDeviceConfig(DeviceConfig deviceConfig);
    }
}
