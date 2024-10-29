using MOBILEAPI2024.DAL.Entities;
using MOBILEAPI2024.DTO.RequestDTO.Employee;
using MOBILEAPI2024.DTO.ResponseDTO.Employee;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DAL.Repositories.IRepositories
{
    public interface IEmployeeRepository : IGenericRepository<ActiveInActiveUser>
    {
        List<object> EmployeeDetails(EmployeeDetails empDetails);
        dynamic EmployeeDetailsForTally(int cmpId, string branchName);
        dynamic EmployeeDirectoryData(int cmpId);
        dynamic ManagerApprovalDetails(ManagerApprovalDetailsRequest managerApprovalDetailsRequest);
        List<AttendanceRegularizeDetailsResponse> AttendanceRegularizeDetails(AttendanceRegularizeDetails attendanceRegularizeDetails);
        string UpdateEmployeeDetails(UpdateEmployeeDetails updateEmployeeDetails);
        dynamic MyTeamDetails(int empId, int cmpId, string status);
        dynamic NewJoiningEmployeeDetails(int cmpId);
        dynamic UpdateEmpFavDetails(UpdateEmpFavDetailsRequest updateEmpFavDetailsRequest);
        dynamic UpdateEmployeeDetailsMain(UpdateEmployeeDetailsRequest updateEmployeeDetailsRequest);
        dynamic MyTeamAttendanceInsert(MyTeamAttendanceInsertRequest myTeamAttendanceInsertRequest);
    }
}
