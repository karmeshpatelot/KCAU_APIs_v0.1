using AutoMapper;
using Microsoft.Extensions.Options;
using MOBILEAPI2024.BLL.Services.IServices;
using MOBILEAPI2024.DAL.Repositories.IRepositories;
using MOBILEAPI2024.DTO.Common;
using MOBILEAPI2024.DTO.RequestDTO.Employee;
using MOBILEAPI2024.DTO.ResponseDTO.Employee;
using System.Collections;
using System.Xml.Linq;

namespace MOBILEAPI2024.BLL.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly AppSettings _appSettings;
        private readonly IMapper _mapper;
        public EmployeeService(IEmployeeRepository employeeRepository,IOptions<AppSettings> appSetting,IMapper mapper)
        {
            _employeeRepository = employeeRepository;
            _appSettings = appSetting.Value;
            _mapper = mapper;
        }

        public dynamic EmployeeDetails(int empId, int cmpId, string empCode)
        {
            EmployeeDetails empDetails = new EmployeeDetails();
            empDetails.Emp_ID = empId;
            empDetails.Cmp_ID = cmpId;
            empDetails.Emp_Code=empCode;
            empDetails.Type = "E";
            var employeeResponse = _employeeRepository.EmployeeDetails(empDetails);
            if(employeeResponse == null || (employeeResponse as ICollection)?.Count == 0)
            {
                return null;
            }
            return employeeResponse;
        }
        
        public dynamic EmployeeDetailsForTally(int cmpId, string branchName)
        {
            var employeeResponse = _employeeRepository.EmployeeDetailsForTally(cmpId,branchName);
            if (employeeResponse == null || (employeeResponse as ICollection)?.Count == 0)
            {
                return null;
            }
            return employeeResponse;
        }

        public dynamic EmployeeDirectoryData(int cmpId)
        {
            List<EmployeeDirectoryDataResponse> employeeResponse = _employeeRepository.EmployeeDirectoryData(cmpId);
            if (employeeResponse != null)
            {
                for (int i = 0; i < employeeResponse.Count(); i++)
                {
                    employeeResponse[0].Image_Path = _appSettings.ImagePath +"App_File/EMPIMAGES/" + employeeResponse[i].Image_Name;
                }

                return employeeResponse;
            }
            return null;
        }

        public List<object> EmployeeList(EmployeeListRequest employeeListRequest)
        {
            EmployeeDetails empDetails = new EmployeeDetails();
            empDetails.Emp_ID = employeeListRequest.EmpId;
            empDetails.Cmp_ID = employeeListRequest.CmpId;
            empDetails.Type = "A";
            var employeeResponse = _employeeRepository.EmployeeDetails(empDetails);
            //foreach (var item in employeeResponse)
            //{
            //    if (!string.IsNullOrEmpty(item.Date_Of_Birth))
            //    {
            //        DateTime dateOfBirth = DateTime.Parse(item.Date_Of_Birth);
            //        item.Date_Of_Birth = dateOfBirth.ToString("dd-MM-yyyy");
            //    }
            //    if (!string.IsNullOrEmpty(item.Date_Of_Join))
            //    {
            //        DateTime dateOfJoin = DateTime.Parse(item.Date_Of_Join);
            //        item.Date_Of_Join = dateOfJoin.ToString("dd-MM-yyyy");
            //    }
            //}

            if (employeeResponse == null || (employeeResponse as ICollection)?.Count == 0)
            {
                return null;
            }
            return employeeResponse;
        }

        public dynamic ManagerApprovalDetails(ManagerApprovalDetailsRequest managerApprovalDetailsRequest)
        {
            var employeeResponse = _employeeRepository.ManagerApprovalDetails(managerApprovalDetailsRequest);
            if (employeeResponse == null || (employeeResponse as ICollection)?.Count == 0)
            {
                return null;
            }
            return employeeResponse;
        }

        public dynamic MyTeamAttendance(int empId, int cmpId)
        {
            AttendanceRegularizeDetails attendanceRegularizeDetails = new();
            attendanceRegularizeDetails.EmpID = empId;
            attendanceRegularizeDetails.CmpID = cmpId;
            attendanceRegularizeDetails.Type = "T";
            attendanceRegularizeDetails.Fromdate = DateTime.Now;
            attendanceRegularizeDetails.Todate = DateTime.Now;
            var employeeResponse = _employeeRepository.AttendanceRegularizeDetails(attendanceRegularizeDetails);

            if (employeeResponse != null)
            {
                UpdateEmployeeDetails updateEmployeeDetails = new();
                updateEmployeeDetails.StrType = "I";
                for (int i = 0; i < employeeResponse.Count(); i++)
                {
                    
                    updateEmployeeDetails.EmpID = employeeResponse[i].Emp_Id;
                    updateEmployeeDetails.CmpID = employeeResponse[i].Cmp_ID;

                    string ImageName = _employeeRepository.UpdateEmployeeDetails(updateEmployeeDetails);
                    employeeResponse[i].Image_Path = _appSettings.ImagePath + "App_File/EMPIMAGES/" + ImageName;
                }
                return employeeResponse;
            }
            return null;
        }
        public static bool IsXmlFormat(string input)
        {
            try
            {
                // Attempt to parse the input string as an XML document
                XDocument.Parse(input);
                return true; // Parsing succeeded, so it's in XML format
            }
            catch (Exception)
            {
                try
                {
                    // Attempt to parse the input string as an XML element
                    XElement.Parse(input);
                    return true; // Parsing succeeded, so it's in XML format
                }
                catch (Exception)
                {
                    return false; // Parsing failed, so it's not in XML format
                }
            }
        }
        public dynamic MyTeamAttendanceInsert(MyTeamAttendanceInsertRequest myTeamAttendanceInsertRequest)
        {
            if (IsXmlFormat(myTeamAttendanceInsertRequest.Details))
            {
                var atendanceResponse = _employeeRepository.MyTeamAttendanceInsert(myTeamAttendanceInsertRequest);

                if (atendanceResponse != null)
                {
                    return atendanceResponse;
                }
                return null;
            }
            else
            {
                return "Invalid data passed. ";
            }
        }

        public dynamic MyTeamDetails(int empId, int cmpId, string status)
        {
            var employeeResponse = _employeeRepository.MyTeamDetails(empId,cmpId,status);
            if (employeeResponse == null || (employeeResponse as ICollection)?.Count == 0)
            {
                return null;
            }
            return employeeResponse;
        }

        public dynamic NewJoiningEmployeeDetails(int cmpId)
        {
            var employeeResponse = _employeeRepository.NewJoiningEmployeeDetails(cmpId);
            if (employeeResponse == null || (employeeResponse as ICollection)?.Count == 0)
            {
                return null;
            }
            return employeeResponse;
        }

        public dynamic UpdateEmpFavDetails(UpdateEmpFavDetailsRequest updateEmpFavDetailsRequest)
        {
            updateEmpFavDetailsRequest.Type = "U";
            var employeeResponse = _employeeRepository.UpdateEmpFavDetails(updateEmpFavDetailsRequest);
            if (employeeResponse == null || (employeeResponse as ICollection)?.Count == 0)
            {
                return null;
            }
            return employeeResponse;
        }

        public dynamic UpdateEmployeeDetails(UpdateEmployeeDetailsRequest updateEmployeeDetailsRequest)
        {
            if (updateEmployeeDetailsRequest.Action == "")
            {
                updateEmployeeDetailsRequest.Type = "U";
                var employeeResponse = _employeeRepository.UpdateEmployeeDetailsMain(updateEmployeeDetailsRequest);
                if (employeeResponse == null || (employeeResponse as ICollection)?.Count == 0)
                {
                    return null;
                }
                return employeeResponse;
            }
            else if(updateEmployeeDetailsRequest.Action == "Delete" || updateEmployeeDetailsRequest.Action == "delete")
            {
                updateEmployeeDetailsRequest.Type = "P";
                var employeeResponse = _employeeRepository.UpdateEmployeeDetailsMain(updateEmployeeDetailsRequest);
                if (employeeResponse == null || (employeeResponse as ICollection)?.Count == 0)
                {
                    return null;
                }
                return employeeResponse;
            }
            return null;

            
        }
    }
}
