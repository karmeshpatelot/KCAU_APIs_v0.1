using Azure;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using MOBILEAPI2024.BLL.Services.IServices;
using MOBILEAPI2024.DTO.Common;
using MOBILEAPI2024.DTO.RequestDTO.Attendance;
using MOBILEAPI2024.DTO.RequestDTO.Employee;
using MOBILEAPI2024.DTO.ResponseDTO.Attendance;
using MOBILEAPI2024.DTO.ResponseDTO.Employee;
using Response = MOBILEAPI2024.DTO.Common.Response;

namespace MOBILEAPI2024.API.Controllers
{
    [Route("api/v1")]
    [ApiController]
    public class DataController : ControllerBase
    {
        private readonly IDataService _dataService;
        private readonly IMemoryCache _cache;
        public DataController(IDataService dataService, IMemoryCache cache)
        {
            _dataService = dataService;
            _cache = cache;
        }

        [EnableCors]
        [HttpGet(APIUrls.Login)]
        public IActionResult Login(string username, string type)
        {
            CustomResponse customResponse = new();
            try
            {
                if (type == "student")
                {
                    if (username != null)
                    {
                        var CheckStudentExists = _dataService.GetStudents(username, 1, 1);
                        if (CheckStudentExists.Result.Count() != 0)
                        {
                            _dataService.SendEmailOTP(CheckStudentExists.Result[0]);
                            customResponse.code = StatusCodes.Status200OK;
                            customResponse.status = true;
                            customResponse.message = CommonMessage.EmailSent;

                            return Ok(customResponse);
                        }
                        customResponse.code = StatusCodes.Status404NotFound;
                        customResponse.status = false;
                        customResponse.message = CommonMessage.NoDataFound;
                        return BadRequest(customResponse);
                    }
                }
                else if (type == "staff")
                {
                    if (username != null)
                    {
                        var CheckStudentExists = _dataService.GetStaffFromERPForLogin(username, 1, 1);
                        if (CheckStudentExists.Result.Count() != 0)
                        {
                            _dataService.SendEmailOTPStaff(CheckStudentExists.Result[0]);
                            customResponse.code = StatusCodes.Status200OK;
                            customResponse.status = true;
                            customResponse.message = CommonMessage.EmailSent;

                            return Ok(customResponse);
                        }
                        customResponse.code = StatusCodes.Status404NotFound;
                        customResponse.status = false;
                        customResponse.message = CommonMessage.NoDataFound;
                        return BadRequest(customResponse);
                    }
                }
                else if (type == "admin")
                {
                    if (username != null)
                    {
                        var adminUser = _dataService.SendEmailToAdmin(username);
                        if (adminUser == "Success")
                        {
                            customResponse.code = StatusCodes.Status200OK;
                            customResponse.status = true;
                            customResponse.message = CommonMessage.EmailSent;
                            return Ok(customResponse);
                        }
                    }
                }

                customResponse.code = StatusCodes.Status400BadRequest;
                customResponse.status = false;
                customResponse.message = CommonMessage.InValidUser;
                return BadRequest(customResponse);
            }
            catch (Exception ex)
            {
                customResponse.code = StatusCodes.Status500InternalServerError;
                customResponse.status = false;
                customResponse.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, customResponse);

            }
        }

        [EnableCors]
        [HttpGet(APIUrls.OTPVarification)]
        public IActionResult OtpVerification(string username, int OtpCode)
        {
            CustomResponse customResponse = new();
            try
            {
                if (username != null)
                {
                    var user = _dataService.OtpVerification(username, OtpCode);
                    if (user != null)
                    {
                        customResponse.code = StatusCodes.Status200OK;
                        customResponse.status = true;
                        customResponse.message = CommonMessage.OTPVerified;
                        return Ok(customResponse);
                    }
                    customResponse.code = StatusCodes.Status404NotFound;
                    customResponse.status = false;
                    customResponse.message = CommonMessage.InValidUser;
                    return NotFound(customResponse);
                }
                customResponse.code = StatusCodes.Status400BadRequest;
                customResponse.status = false;
                customResponse.message = CommonMessage.InValidUser;
                return BadRequest(customResponse);

            }
            catch (Exception ex)
            {
                customResponse.code = StatusCodes.Status500InternalServerError;
                customResponse.status = false;
                customResponse.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, customResponse);
            }
        }

        [HttpGet(APIUrls.PostStudentsData)]
        public IActionResult PostStudentsData(string? studentNo, int entriesPerPage, int pageNo)
        {
            try
            {
                if (entriesPerPage == 0)
                {
                    entriesPerPage = 1;
                }
                if (pageNo == 0)
                {
                    pageNo = 1;
                }

                if (studentNo == null || studentNo == "null")
                {
                    studentNo = null;
                }
                var getstudents = _dataService.GetStudentsData(studentNo, entriesPerPage, pageNo);
                if (getstudents.Result.Count() != 0)
                {
                    return Ok(getstudents);
                }
                return NotFound(getstudents);
            }
            catch (Exception ex)
            {
                return Ok(ex);
            }
        }

        [HttpGet(APIUrls.PostStaffsData)]
        public IActionResult PostStaffsData(string? staffNo, int entriesPerPage, int pageNo)
        {
            try
            {
                if (entriesPerPage == 0)
                {
                    entriesPerPage = 1;
                }
                if (pageNo == 0)
                {
                    pageNo = 1;
                }

                if (staffNo == null || staffNo == "null")
                {
                    staffNo = null;
                }

                var getstudents = _dataService.GetStaff(staffNo, entriesPerPage, pageNo);
                if (getstudents.Result.Count() != 0)
                {
                    return Ok(getstudents);
                }
                return NotFound(getstudents);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet(APIUrls.GetAttendance)]
        public IActionResult GetAttendance(string? username, int month)
        {
            DTO.Common.Response response = new();
            try
            {
                string user = null;
                if (username != null)
                {
                    user = username.Replace("/", "_");
                }

                AttendanceDetails attendanceDetails = new();
                List<Attendance> attendancesforUser = new();
                List<Attendance> attendances = new();

                var getloginuser = _dataService.GetLoginUser();
                if (getloginuser != null)
                {
                    var getInoutData = _dataService.GetInOutData(month);
                    if (getInoutData.records.Count() != 0)
                    {
                        int presents = 0;
                        int absents = 0;
                        foreach (Record record in getInoutData.records)
                        {
                            Attendance attendance = new();
                            attendance.Date = record.datetime;
                            attendance.InTime = record.inTime;
                            attendance.OutTime = record.outTime;

                            if (record.userId == user)
                            {
                                if (record.inTime != null && record.outTime != null && record.inTime != "-" && record.outTime != "-")
                                {
                                    attendance.status = "Present";
                                    presents = presents + 1;
                                }
                                else
                                {
                                    attendance.status = "Absent";
                                    absents = absents + 1;
                                }
                                attendancesforUser.Add(attendance);
                            }
                            else
                            {
                                if (record.inTime != null && record.outTime != null && record.inTime != "-" && record.outTime != "-")
                                {
                                    attendance.status = "Present";
                                    presents = presents + 1;
                                }
                                else
                                {
                                    attendance.status = "Absent";
                                    absents = absents + 1;
                                }
                                attendances.Add(attendance);
                            }
                        }

                        if (user != null)
                        {
                            attendanceDetails.Attendances = attendancesforUser;
                            attendanceDetails.PresentDays = presents;
                            attendanceDetails.AbsentDays = absents;
                            attendanceDetails.TotalDays = _dataService.GetTotalDaysInMonth(month);
                        }
                        else
                        {
                            attendanceDetails.Attendances = attendances;
                            attendanceDetails.PresentDays = presents;
                            attendanceDetails.AbsentDays = absents;
                            attendanceDetails.TotalDays = Convert.ToInt32(getInoutData.total);
                        }

                        response.code = StatusCodes.Status200OK;
                        response.status = true;
                        response.message = CommonMessage.Success;
                        response.data = attendanceDetails;
                        return Ok(response);

                    }
                    response.code = StatusCodes.Status404NotFound;
                    response.status = true;
                    response.message = CommonMessage.NoDataFound;
                    return NotFound(response);
                }
                response.code = StatusCodes.Status400BadRequest;
                response.status = true;
                response.message = CommonMessage.InValidToken;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.code = StatusCodes.Status500InternalServerError;
                response.status = false;
                response.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        //[HttpGet(APIUrls.GetClassWiseAttendance)]
        //public IActionResult GetClassWiseAttendance(string? username, int month)
        //{
        //    Response response = new();
        //    try
        //    {
        //        string user = null;
        //        if (username != null)
        //        {
        //            user = username.Replace("/", "_");
        //        }

        //        AttendanceDetails attendanceDetails = new();
        //        List<Attendance> attendancesforUser = new();
        //        List<Attendance> attendances = new();
        //        var getloginuser = _dataService.GetLoginUser();
        //        if (getloginuser != null)
        //        {
        //            var getInoutData = _dataService.GetInOutData(month);
        //            if (getInoutData.records.Count() != 0)
        //            {
        //                int presents = 0;
        //                int absents = 0;
        //                foreach (Record record in getInoutData.records)
        //                {
        //                    Attendance attendance = new();
        //                    attendance.Date = record.datetime;
        //                    attendance.InTime = record.inTime;
        //                    attendance.OutTime = record.outTime;

        //                    if (record.userId == user)
        //                    {
        //                        if (record.inTime != null && record.outTime != null && record.inTime != "-" && record.outTime != "-")
        //                        {
        //                            attendance.status = "Present";
        //                            presents = presents + 1;
        //                        }
        //                        else
        //                        {
        //                            attendance.status = "Absent";
        //                            absents = absents + 1;
        //                        }
        //                        attendancesforUser.Add(attendance);
        //                    }
        //                    else
        //                    {
        //                        if (record.inTime != null && record.outTime != null && record.inTime != "-" && record.outTime != "-")
        //                        {
        //                            attendance.status = "Present";
        //                            presents = presents + 1;
        //                        }
        //                        else
        //                        {
        //                            attendance.status = "Absent";
        //                            absents = absents + 1;
        //                        }
        //                        attendances.Add(attendance);
        //                    }
        //                }

        //                if (user != null)
        //                {
        //                    attendanceDetails.Attendances = attendancesforUser;
        //                    attendanceDetails.PresentDays = presents;
        //                    attendanceDetails.AbsentDays = absents;
        //                    attendanceDetails.TotalDays = _dataService.GetTotalDaysInMonth(month);
        //                }
        //                else
        //                {
        //                    attendanceDetails.Attendances = attendances;
        //                    attendanceDetails.PresentDays = presents;
        //                    attendanceDetails.AbsentDays = absents;
        //                    attendanceDetails.TotalDays = Convert.ToInt32(getInoutData.total);
        //                }

        //                response.code = StatusCodes.Status200OK;
        //                response.status = true;
        //                response.message = CommonMessage.Success;
        //                response.data = attendanceDetails;
        //                return Ok(response);

        //            }
        //            response.code = StatusCodes.Status404NotFound;
        //            response.status = true;
        //            response.message = CommonMessage.NoDataFound;
        //            return NotFound(response);
        //        }
        //        response.code = StatusCodes.Status400BadRequest;
        //        response.status = true;
        //        response.message = CommonMessage.InValidToken;
        //        return BadRequest(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.code = StatusCodes.Status500InternalServerError;
        //        response.status = false;
        //        response.message = ex.Message;
        //        return StatusCode(StatusCodes.Status500InternalServerError, response);
        //    }
        //}

        [EnableCors]
        [HttpGet(APIUrls.DeleteExpiredOTP)]
        public IActionResult DeleteExpiredOTP()
        {
            try
            {
                _dataService.DeleteExpiredOTP();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [EnableCors]
        [HttpGet(APIUrls.SwipeInOut)]
        public IActionResult SwipeInOut()
        {
            Response response = new();
            try
            {
                int month = 9;
                var getloginuser = _dataService.GetLoginUser();
                if (getloginuser != null)
                {
                    var getInoutData = _dataService.GetInOutData(month);
                    if (getInoutData.records.Count() != 0)
                    {

                        foreach (Record record in getInoutData.records)
                        {
                            var checkRecordExists = _dataService.CheckDataExists(record.userId, record.datetime);
                            if (checkRecordExists == null)
                            {
                                _dataService.InsertInOutData(record);
                            }
                        }

                        response.code = StatusCodes.Status200OK;
                        response.status = true;
                        response.message = CommonMessage.Success;
                        response.data = CommonMessage.Success;
                        return Ok(response);

                    }
                    response.code = StatusCodes.Status404NotFound;
                    response.status = true;
                    response.message = CommonMessage.NoDataFound;
                    return NotFound(response);
                }
                response.code = StatusCodes.Status400BadRequest;
                response.status = true;
                response.message = CommonMessage.InValidToken;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.code = StatusCodes.Status500InternalServerError;
                response.status = false;
                response.message = ex.Message;
                return StatusCode(StatusCodes.Status500InternalServerError, response);
            }
        }

        //[HttpGet(APIUrls.GetAttendances)]
        //public IActionResult GetAttendances(string? username, int month)
        //{
        //    Response response = new();
        //    try
        //    {
        //        string user = null;
        //        if (username != null)
        //        {
        //            user = username.Replace("/", "_");
        //        }

        //        AttendanceDetails attendanceDetails = new();
        //        List<Attendance> attendancesforUser = new();
        //        List<Attendance> attendances = new();
        //        List<CustomAttendance> customAttendances = new();
        //        List<CustomAttendance> customAttendancesForUser = new();
        //        CustomAttendanceList customAttendanceList = new();
        //        List<InOut> inOuts = new();
        //        List<InOut> inOutsForUser = new();

        //        var getloginuser = _dataService.GetLoginUser();
        //        if (getloginuser != null)
        //        {
        //            var getInoutData = _dataService.GetInOutData(month);
        //            if (getInoutData.records.Count() != 0)
        //            {
        //                int id = 0;
        //                foreach (Record record in getInoutData.records)
        //                {
        //                    CustomAttendance customAttendance = new();
        //                    Attendance attendance = new();
        //                    InOut inOut = new();
        //                    attendance.Date = record.datetime;
        //                    attendance.InTime = record.inTime;
        //                    attendance.OutTime = record.outTime;

        //                    if (record.userId == user)
        //                    {
        //                        if (record.inTime != null && record.outTime != null && record.inTime != " - " && record.outTime != " - ")
        //                        {
        //                            customAttendance.id = record.inTime + " to " + record.outTime;
        //                            DateTime parsedDate = DateTime.ParseExact(record.datetime, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
        //                            string formattedDate = parsedDate.ToString("yyyy-MM-dd");
        //                            customAttendance.start = formattedDate;
        //                            customAttendance.backgroundColor = "green";
        //                            customAttendance.textColor = "#000";
        //                            customAttendance.color = "#000";
        //                            customAttendance.eventTextColor = "#000";
        //                            customAttendance.display = "background";
        //                        }
        //                        else if (record.inTime != null && record.inTime != "-")
        //                        {
        //                            customAttendance.id = record.inTime + " - ";
        //                            DateTime parsedDate = DateTime.ParseExact(record.datetime, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
        //                            string formattedDate = parsedDate.ToString("yyyy-MM-dd");
        //                            customAttendance.start = formattedDate;
        //                            customAttendance.backgroundColor = "red";
        //                            customAttendance.textColor = "#000";
        //                            customAttendance.color = "#000";
        //                            customAttendance.eventTextColor = "#000";
        //                            customAttendance.display = "background";
        //                        }
        //                        else
        //                        {
        //                            customAttendance.id = " ";
        //                            DateTime parsedDate = DateTime.ParseExact(record.datetime, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
        //                            string formattedDate = parsedDate.ToString("yyyy-MM-dd");
        //                            customAttendance.start = formattedDate;
        //                            customAttendance.backgroundColor = "red";
        //                            customAttendance.textColor = "#000";
        //                            customAttendance.color = "#000";
        //                            customAttendance.eventTextColor = "#000";
        //                            customAttendance.display = "background";
        //                        }
        //                        customAttendancesForUser.Add(customAttendance);

        //                    }
        //                    else
        //                    {
        //                        if (record.inTime != null && record.outTime != null && record.inTime != "-" && record.outTime != "-")
        //                        {
        //                            customAttendance.id = record.inTime + " to " + record.outTime;
        //                            DateTime parsedDate = DateTime.ParseExact(record.datetime, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
        //                            string formattedDate = parsedDate.ToString("yyyy-MM-dd");
        //                            customAttendance.start = formattedDate;
        //                            customAttendance.backgroundColor = "green";
        //                            customAttendance.textColor = "#000";
        //                            customAttendance.color = "#000";
        //                            customAttendance.eventTextColor = "#000";
        //                            customAttendance.display = "background";
        //                        }
        //                        else if (record.inTime != null && record.inTime != "-")
        //                        {
        //                            customAttendance.id = record.inTime + " - ";
        //                            DateTime parsedDate = DateTime.ParseExact(record.datetime, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
        //                            string formattedDate = parsedDate.ToString("yyyy-MM-dd");
        //                            customAttendance.start = formattedDate;
        //                            customAttendance.backgroundColor = "red";
        //                            customAttendance.textColor = "#000";
        //                            customAttendance.color = "#000";
        //                            customAttendance.eventTextColor = "#000";
        //                            customAttendance.display = "background";
        //                        }
        //                        else
        //                        {
        //                            customAttendance.id = record.inTime + " to " + record.outTime;
        //                            DateTime parsedDate = DateTime.ParseExact(record.datetime, "yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture);
        //                            string formattedDate = parsedDate.ToString("yyyy-MM-dd");
        //                            customAttendance.start = formattedDate;
        //                            customAttendance.backgroundColor = "red";
        //                            customAttendance.textColor = "#000";
        //                            customAttendance.color = "#000";
        //                            customAttendance.eventTextColor = "#000";
        //                            customAttendance.display = "background";
        //                        }
        //                        customAttendances.Add(customAttendance);
        //                    }
        //                }

        //                if (user != null)
        //                {
        //                    customAttendanceList.attendances = customAttendancesForUser; customAttendanceList.inOuts = inOutsForUser;
        //                    customAttendanceList.inOuts = inOutsForUser;
        //                }
        //                else
        //                {
        //                    customAttendanceList.attendances = customAttendances;
        //                    customAttendanceList.inOuts = inOuts;
        //                }
        //                response.code = StatusCodes.Status200OK;
        //                response.status = true;
        //                response.message = CommonMessage.Success;
        //                response.data = customAttendances;
        //                return Ok(response);

        //            }
        //            response.code = StatusCodes.Status404NotFound;
        //            response.status = true;
        //            response.message = CommonMessage.NoDataFound;
        //            return NotFound(response);
        //        }
        //        response.code = StatusCodes.Status400BadRequest;
        //        response.status = true;
        //        response.message = CommonMessage.InValidToken;
        //        return BadRequest(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.code = StatusCodes.Status500InternalServerError;
        //        response.status = false;
        //        response.message = ex.Message;
        //        return StatusCode(StatusCodes.Status500InternalServerError, response);
        //    }
        //}

        [HttpGet(APIUrls.GetAttendances)]
        public IActionResult GetAttendances(string? username, int month, string type)
        {
            Response response = new();
            if (type == "campus")
            {
                try
                {
                    string user = null;
                    if (username != null)
                    {
                        user = username.Replace("/", "_");
                    }

                    AttendanceDetails attendanceDetails = new();
                    List<Attendance> attendancesforUser = new();
                    List<Attendance> attendances = new();
                    List<CustomAttendance> customAttendances = new();
                    List<CustomAttendance> customAttendancesForUser = new();
                    CustomAttendanceList customAttendanceList = new();
                    List<InOut> inOuts = new();
                    List<InOut> inOutsForUser = new();

                    var getInoutData = _dataService.GetCampusAttendance(username, month);
                    if (getInoutData != null)
                    {
                        int id = 0;
                        foreach (CampusAttendance campusAttendance in getInoutData)
                        {
                            CustomAttendance customAttendance = new();
                            Attendance attendance = new();
                            InOut inOut = new();
                            attendance.Date = Convert.ToString(campusAttendance.ForDate);
                            attendance.InTime = Convert.ToString(campusAttendance.InTime);
                            attendance.OutTime = Convert.ToString(campusAttendance.OutTime);

                            if (Convert.ToString(campusAttendance.InTime) != null && Convert.ToString(campusAttendance.OutTime) != null && campusAttendance.UserName != null)
                            {
                                customAttendance.id = Convert.ToString(campusAttendance.InTime) + " to " + Convert.ToString(campusAttendance.OutTime);
                                DateTime parsedDate = DateTime.ParseExact(Convert.ToString(campusAttendance.ForDate), "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                                string formattedDate = parsedDate.ToString("yyyy-MM-dd");
                                customAttendance.start = formattedDate;
                                customAttendance.backgroundColor = "green";
                                customAttendance.textColor = "#000";
                                customAttendance.color = "#000";
                                customAttendance.eventTextColor = "#000";
                                customAttendance.display = "background";
                            }
                            else if (Convert.ToString(campusAttendance.InTime) != null)
                            {
                                customAttendance.id = Convert.ToString(campusAttendance.InTime) + " - ";
                                DateTime parsedDate = DateTime.ParseExact(Convert.ToString(campusAttendance.ForDate), "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                                string formattedDate = parsedDate.ToString("yyyy-MM-dd");
                                customAttendance.start = formattedDate;
                                customAttendance.backgroundColor = "red";
                                customAttendance.textColor = "#000";
                                customAttendance.color = "#000";
                                customAttendance.eventTextColor = "#000";
                                customAttendance.display = "background";
                            }
                            else
                            {
                                customAttendance.id = " ";
                                DateTime parsedDate = DateTime.ParseExact(Convert.ToString(campusAttendance.ForDate), "M/d/yyyy h:mm:ss tt", System.Globalization.CultureInfo.InvariantCulture);
                                string formattedDate = parsedDate.ToString("yyyy-MM-dd");
                                customAttendance.start = formattedDate;
                                customAttendance.backgroundColor = "red";
                                customAttendance.textColor = "#000";
                                customAttendance.color = "#000";
                                customAttendance.eventTextColor = "#000";
                                customAttendance.display = "background";
                            }
                            customAttendancesForUser.Add(customAttendance);
                        }
                        customAttendanceList.attendances = customAttendancesForUser;
                        customAttendanceList.inOuts = inOutsForUser;
                        response.code = StatusCodes.Status200OK;
                        response.status = true;
                        response.message = CommonMessage.Success;
                        response.data = customAttendancesForUser;
                        return Ok(response);

                    }
                    response.code = StatusCodes.Status404NotFound;
                    response.status = true;
                    response.message = CommonMessage.NoDataFound;
                    return NotFound(response);

                }
                catch (Exception ex)
                {
                    response.code = StatusCodes.Status500InternalServerError;
                    response.status = false;
                    response.message = ex.Message;
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            else if (type == "class")
            {
                try
                {
                    string user = null;
                    if (username != null)
                    {
                        user = username.Replace("/", "_");
                    }

                    AttendanceDetails attendanceDetails = new();
                    List<Attendance> attendancesforUser = new();
                    List<Attendance> attendances = new();
                    List<CustomAttendanceForClass> customAttendanceForClasses = new();
                    CustomAttendanceList customAttendanceList = new();
                    List<InOut> inOuts = new();
                    List<InOut> inOutsForUser = new();

                    var getInoutData = _dataService.GetClassAttendance(user, month);
                    if (getInoutData != null)
                    {
                        int id = 0;
                        int I = 1;
                        foreach (ClassAttendanceForCal campusAttendance in getInoutData)
                        {
                            CustomAttendanceForClass customAttendance = new();
                            Attendance attendance = new();
                            InOut inOut = new();
                            attendance.Date = Convert.ToString(campusAttendance.ForDate);
                            attendance.InTime = Convert.ToString(campusAttendance.InTime);
                            attendance.OutTime = Convert.ToString(campusAttendance.OutTime);

                            if (Convert.ToString(campusAttendance.InTime) != null)
                            {
                                customAttendance.id = I;
                                customAttendance.title = Convert.ToString(campusAttendance.InTime.TimeOfDay);
                                customAttendance.start = campusAttendance.ForDate;
                                customAttendance.color = "green";
                            }
                            else
                            {
                                customAttendance.id = I;
                                customAttendance.title = "";
                                customAttendance.start = campusAttendance.ForDate;
                                customAttendance.color = "red";
                            }
                            I++;
                            customAttendanceForClasses.Add(customAttendance);
                        }
                        response.code = StatusCodes.Status200OK;
                        response.status = true;
                        response.message = CommonMessage.Success;
                        response.data = customAttendanceForClasses;
                        return Ok(response);

                    }
                    response.code = StatusCodes.Status404NotFound;
                    response.status = true;
                    response.message = CommonMessage.NoDataFound;
                    return NotFound(response);

                }
                catch (Exception ex)
                {
                    response.code = StatusCodes.Status500InternalServerError;
                    response.status = false;
                    response.message = ex.Message;
                    return StatusCode(StatusCodes.Status500InternalServerError, response);
                }
            }
            return BadRequest("No type found.");
        }

        //[HttpPost(APIUrls.AddCampusAttendance)]
        //public IActionResult AddCampusAttendance(string deviceId, string campusName, string IOFlag)
        //{
        //    FeesStatus feesStatus = new();
        //    try
        //    {
        //        string key = "";
        //        if (!_cache.TryGetValue("SessionId", out string value))
        //        {
        //            var getLoginAccess = _dataService.GetLoginAccess();
        //            _cache.TryGetValue("SessionId", out string value1);
        //            key = value1;
        //        }
        //        else
        //        {
        //            _cache.TryGetValue("SessionId", out string value1);
        //            key = value1;
        //        }

        //        var getAttendance = _dataService.GetAttendanceFromBS(key, deviceId);
        //        if (getAttendance.Response.message != "Login required.")
        //        {
        //            foreach (Row row in getAttendance.EventCollection.rows)
        //            {
        //                AddCampusInOut addCampusInOut1 = new();
        //                addCampusInOut1.UserName = row.user_id.name.Replace("_", "/");
        //                addCampusInOut1.Campus = checkDeviceConfig.CampusName;
        //                addCampusInOut1.IOFlag = checkDeviceConfig.Type;
        //                addCampusInOut1.DateTime = row.datetime;
        //                var checkAttendance = _dataService.CheckCampusAttendance(addCampusInOut1);
        //                if (checkAttendance != null)
        //                {
        //                    feesStatus.ShowPopup = "True";
        //                    feesStatus.Status = CommonMessage.AttendanceAlreadyExixts;
        //                    feesStatus.StudentNo = checkAttendance.UserName;
        //                    feesStatus.StudentName = "";

        //                    return Ok(feesStatus);
        //                }
        //                _dataService.AddCampusAttendance(addCampusInOut1);
        //                feesStatus.ShowPopup = "False";
        //                feesStatus.Status = "";
        //                feesStatus.StudentNo = "";
        //                feesStatus.StudentName = "";
        //                return Ok(feesStatus);
        //            }
        //        }
        //        else
        //        {
        //            _cache.Remove("SessionId");
        //            var getLoginAccess = _dataService.GetLoginAccess();
        //            _cache.TryGetValue("SessionId", out string value1);
        //            key = value1;
        //            var getAttendanc = _dataService.GetAttendanceFromBS(key, deviceId);
        //            if (getAttendanc.Response.message != "Login required.")
        //            {
        //                foreach (Row row in getAttendance.EventCollection.rows)
        //                {
        //                    AddCampusInOut addCampusInOut1 = new();
        //                    addCampusInOut1.UserName = row.user_id.name.Replace("_", "/");
        //                    addCampusInOut1.Campus = checkDeviceConfig.CampusName;
        //                    addCampusInOut1.IOFlag = checkDeviceConfig.Type;
        //                    addCampusInOut1.DateTime = row.datetime;
        //                    var checkAttendance = _dataService.CheckCampusAttendance(addCampusInOut1);
        //                    if (checkAttendance != null)
        //                    {
        //                        feesStatus.ShowPopup = "True";
        //                        feesStatus.Status = CommonMessage.AttendanceAlreadyExixts;
        //                        feesStatus.StudentNo = checkAttendance.UserName;
        //                        feesStatus.StudentName = "";

        //                        return Ok(feesStatus);
        //                    }
        //                    _dataService.AddCampusAttendance(addCampusInOut1);
        //                    feesStatus.ShowPopup = "False";
        //                    feesStatus.Status = "";
        //                    feesStatus.StudentNo = "";
        //                    feesStatus.StudentName = "";
        //                    return Ok(feesStatus);
        //                }
        //            }
        //        }
        //        return Ok("Session Expired.");
        //    }
        //    catch (Exception ex)
        //    {
        //        response.code = StatusCodes.Status500InternalServerError;
        //        response.status = false;
        //        response.message = ex.Message;
        //        return StatusCode(StatusCodes.Status500InternalServerError, response);
        //    }
        //}

        //[HttpPost(APIUrls.AddLectureAttendance)]
        //public IActionResult AddLectureAttendance(AddLectureAttendance addLectureAttendance)
        //{
        //    Response response = new();
        //    try
        //    {
        //        TryValidateModel(addLectureAttendance);
        //        if (ModelState.IsValid)
        //        {
        //            _dataService.AddLectureAttendance(addLectureAttendance);
        //            response.code = StatusCodes.Status200OK;
        //            response.status = true;
        //            response.message = CommonMessage.Success;
        //            return Ok(response);
        //        }
        //        response.code = StatusCodes.Status400BadRequest;
        //        response.status = true;
        //        response.message = CommonMessage.InValidToken;
        //        return BadRequest(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.code = StatusCodes.Status500InternalServerError;
        //        response.status = false;
        //        response.message = ex.Message;
        //        return StatusCode(StatusCodes.Status500InternalServerError, response);
        //    }
        //}

        [HttpGet(APIUrls.SwipeStudents)]
        public IActionResult SwipeStudents()
        {
            try
            {
                int perPage = 10;

                for (int j = 1164; j <= 2000; j++)
                {
                    var getstudents = _dataService.GetStudents("", perPage, j);

                }
                for (int j = 2001; j <= 3000; j++)
                {
                    var getstudents = _dataService.GetStudents("", perPage, j);

                }

                return NotFound("No Data Found");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet(APIUrls.SwipeStaffs)]
        public IActionResult SwipeStaffs(string? staffNo, int entriesPerPage, int pageNo)
        {
            try
            {
                if (entriesPerPage == 0)
                {
                    entriesPerPage = 1;
                }
                if (pageNo == 0)
                {
                    pageNo = 1;
                }
                var getstudents = _dataService.GetStaffFromERP("", entriesPerPage, pageNo);
                if (getstudents != null)
                {
                    return Ok(getstudents);
                }

                return NotFound("No Data Found");

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpGet(APIUrls.CheckFeesStatus)]
        //public IActionResult CheckFeesStatus(string deviceId)
        //{
        //    FeesStatus feesStatus = new();
        //    try
        //    {
        //        string key = "";
        //        if (!_cache.TryGetValue("SessionId", out string value))
        //        {
        //            var getLoginAccess = _dataService.GetLoginAccess();
        //            _cache.TryGetValue("SessionId", out string value1);
        //            key = value1;
        //        }
        //        else
        //        {
        //            _cache.TryGetValue("SessionId", out string value1);
        //            key = value1;
        //        }

        //        var getAttendance = _dataService.GetAttendanceFromBS(key, deviceId);
        //        if (getAttendance.Response.message != "Login required.")
        //        {
        //            var getAttendanceData = _dataService.GetAttendanceFromDB(getAttendance.EventCollection, null, deviceId);
        //            if (getAttendanceData != null)
        //            {
        //                if (getAttendanceData.Result[0].Allow == "TRUE")
        //                {
        //                    feesStatus.ShowPopup = "True";
        //                    feesStatus.Status = "Paid";
        //                    feesStatus.StudentNo = getAttendanceData.Result[0].student_no;
        //                    feesStatus.StudentName = getAttendanceData.Result[0].name;
        //                    return Ok(feesStatus);
        //                }
        //                else if (getAttendanceData.Result[0].Allow == "FALSE")
        //                {
        //                    feesStatus.ShowPopup = "True";
        //                    feesStatus.Status = "Unpaid";
        //                    feesStatus.StudentNo = getAttendanceData.Result[0].student_no;
        //                    feesStatus.StudentName = getAttendanceData.Result[0].name;
        //                    return Ok(feesStatus);
        //                }
        //            }
        //            else
        //            {
        //                feesStatus.ShowPopup = "False";
        //                feesStatus.Status = "";
        //                feesStatus.StudentNo = "";
        //                feesStatus.StudentName = "";
        //                return Ok(feesStatus);
        //            }
        //        }
        //        else
        //        {
        //            _cache.Remove("SessionId");
        //            var getLoginAccess = _dataService.GetLoginAccess();
        //            _cache.TryGetValue("SessionId", out string value1);
        //            key = value1;
        //            var getAttendanc = _dataService.GetAttendanceFromBS(key, deviceId);
        //            if (getAttendanc.Response.message != "Login required.")
        //            {
        //                var getAttendanceData = _dataService.GetAttendanceFromDB(getAttendanc.EventCollection, null, deviceId);
        //                if (getAttendanceData != null)
        //                {
        //                    if (getAttendanceData.Result[0].Allow == "TRUE")
        //                    {
        //                        feesStatus.ShowPopup = "True";
        //                        feesStatus.Status = "Paid";
        //                        feesStatus.StudentNo = getAttendanceData.Result[0].student_no;
        //                        feesStatus.StudentName = getAttendanceData.Result[0].name;
        //                        return Ok(feesStatus);
        //                    }
        //                    else if (getAttendanceData.Result[0].Allow == "FALSE")
        //                    {
        //                        feesStatus.ShowPopup = "True";
        //                        feesStatus.Status = "Unpaid";
        //                        feesStatus.StudentNo = getAttendanceData.Result[0].student_no;
        //                        feesStatus.StudentName = getAttendanceData.Result[0].name;
        //                        return Ok(feesStatus);
        //                    }
        //                }
        //                else
        //                {
        //                    feesStatus.ShowPopup = "False";
        //                    feesStatus.Status = "";
        //                    feesStatus.StudentNo = "";
        //                    feesStatus.StudentName = "";
        //                    return Ok(feesStatus);
        //                }
        //            }
        //        }
        //        return Ok("Session Expired.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }

        //}

        [HttpGet(APIUrls.CheckFeesStatus)]
        public IActionResult CheckFeesStatus(string deviceId)
        {
            FeesStatus feesStatus = new();
            var checkDeviceConfig = _dataService.CheckDeviceConfig(deviceId);
            if (checkDeviceConfig != null)
            {
                if (checkDeviceConfig.Location == "lecture")
                {
                    try
                    {
                        string key = "";
                        if (!_cache.TryGetValue("SessionId", out string value))
                        {
                            var getLoginAccess = _dataService.GetLoginAccess();
                            _cache.TryGetValue("SessionId", out string value1);
                            key = value1;
                        }
                        else
                        {
                            _cache.TryGetValue("SessionId", out string value1);
                            key = value1;
                        }

                        var getAttendance = _dataService.GetAttendanceFromBS(key, deviceId);
                        if (getAttendance.Response.message != "Login required.")
                        {
                            var getAttendanceData = _dataService.GetAttendanceFromDB(getAttendance.EventCollection, null, deviceId, checkDeviceConfig);
                            if (getAttendanceData != null)
                            {
                                if (getAttendanceData.Result[0].Allow == "TRUE")
                                {
                                    feesStatus.Type = "lecture";
                                    feesStatus.ShowPopup = "True";
                                    feesStatus.Status = "Paid";
                                    feesStatus.StudentNo = getAttendanceData.Result[0].student_no;
                                    feesStatus.StudentName = getAttendanceData.Result[0].name;
                                    return Ok(feesStatus);
                                }
                                else if (getAttendanceData.Result[0].Allow == "FALSE")
                                {
                                    feesStatus.Type = "lecture";
                                    feesStatus.ShowPopup = "True";
                                    feesStatus.Status = "Unpaid";
                                    feesStatus.StudentNo = getAttendanceData.Result[0].student_no;
                                    feesStatus.StudentName = getAttendanceData.Result[0].name;
                                    return Ok(feesStatus);
                                }
                            }
                            else
                            {
                                feesStatus.Type = "lecture";
                                feesStatus.ShowPopup = "False";
                                feesStatus.Status = "";
                                feesStatus.StudentNo = "";
                                feesStatus.StudentName = "";
                                return Ok(feesStatus);
                            }
                        }
                        else
                        {
                            _cache.Remove("SessionId");
                            var getLoginAccess = _dataService.GetLoginAccess();
                            _cache.TryGetValue("SessionId", out string value1);
                            key = value1;
                            var getAttendanc = _dataService.GetAttendanceFromBS(key, deviceId);
                            if (getAttendanc.Response.message != "Login required.")
                            {
                                var getAttendanceData = _dataService.GetAttendanceFromDB(getAttendanc.EventCollection, null, deviceId,checkDeviceConfig);
                                if (getAttendanceData != null)
                                {
                                    if (getAttendanceData.Result[0].Allow == "TRUE")
                                    {

                                        feesStatus.Type = "lecture";
                                        feesStatus.ShowPopup = "True";
                                        feesStatus.Status = "Paid";
                                        feesStatus.StudentNo = getAttendanceData.Result[0].student_no;
                                        feesStatus.StudentName = getAttendanceData.Result[0].name;
                                        return Ok(feesStatus);
                                    }
                                    else if (getAttendanceData.Result[0].Allow == "FALSE")
                                    {
                                        feesStatus.Type = "lecture";
                                        feesStatus.ShowPopup = "True";
                                        feesStatus.Status = "Unpaid";
                                        feesStatus.StudentNo = getAttendanceData.Result[0].student_no;
                                        feesStatus.StudentName = getAttendanceData.Result[0].name;
                                        return Ok(feesStatus);
                                    }
                                }
                                else
                                {
                                    feesStatus.Type = "lecture";
                                    feesStatus.ShowPopup = "False";
                                    feesStatus.Status = "";
                                    feesStatus.StudentNo = "";
                                    feesStatus.StudentName = "";
                                    return Ok(feesStatus);
                                }
                            }
                        }
                        return Ok("Session Expired.");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
                else if (checkDeviceConfig.Location == "campus")
                {
                    try
                    {
                        string key = "";
                        if (!_cache.TryGetValue("SessionId", out string value))
                        {
                            var getLoginAccess = _dataService.GetLoginAccess();
                            _cache.TryGetValue("SessionId", out string value1);
                            key = value1;
                        }
                        else
                        {
                            _cache.TryGetValue("SessionId", out string value1);
                            key = value1;
                        }

                        var getAttendance = _dataService.GetAttendanceFromBS(key, deviceId);
                        if (getAttendance.Response.message != "Login required.")
                        {
                            var getAttendanceData = _dataService.GetAttendanceFromDB(getAttendance.EventCollection, null, deviceId, checkDeviceConfig);
                            if (getAttendanceData != null)
                            {
                                feesStatus.Type = "campus";
                                feesStatus.ShowPopup = "True";
                                feesStatus.Status = CommonMessage.AttendanceAlreadyExixts;
                                feesStatus.StudentNo = getAttendanceData.Result[0].student_no;
                                feesStatus.StudentName = getAttendanceData.Result[0].name;
                                return Ok(feesStatus);
                            }
                            else
                            {
                                feesStatus.Type = "campus";
                                feesStatus.ShowPopup = "False";
                                feesStatus.Status = "";
                                feesStatus.StudentNo = "";
                                feesStatus.StudentName = "";
                                return Ok(feesStatus);
                            }
                           
                        }
                        else
                        {
                            _cache.Remove("SessionId");
                            var getLoginAccess = _dataService.GetLoginAccess();
                            _cache.TryGetValue("SessionId", out string value1);
                            key = value1;
                            var getAttendanc = _dataService.GetAttendanceFromBS(key, deviceId);
                            if (getAttendanc.Response.message != "Login required.")
                            {
                                var getAttendanceData = _dataService.GetAttendanceFromDB(getAttendance.EventCollection, null, deviceId, checkDeviceConfig);
                                if (getAttendanceData != null)
                                {
                                    feesStatus.Type = "campus";
                                    feesStatus.ShowPopup = "True";
                                    feesStatus.Status = CommonMessage.AttendanceAlreadyExixts;
                                    feesStatus.StudentNo = getAttendanceData.Result[0].student_no;
                                    feesStatus.StudentName = getAttendanceData.Result[0].name;
                                    return Ok(feesStatus);
                                }
                                else
                                {
                                    feesStatus.Type = "campus";
                                    feesStatus.ShowPopup = "False";
                                    feesStatus.Status = "";
                                    feesStatus.StudentNo = "";
                                    feesStatus.StudentName = "";
                                    return Ok(feesStatus);
                                }
                            }
                        }
                        return Ok("Session Expired.");
                    }
                    catch (Exception ex)
                    {
                        return BadRequest(ex.Message);
                    }
                }
            }
            feesStatus.Type = "stop";
            feesStatus.ShowPopup = "False";
            feesStatus.Status = "Punch stopped in biotrack app.";
            feesStatus.StudentNo = "";
            feesStatus.StudentName = "";
            return Ok(feesStatus);

        }

        [HttpGet(APIUrls.StudentReport)]
        public IActionResult StudentReport(string StudentNo, int Month, int Year, DateTime? Date,string feesStatus)
        {
            Response response = new();
            try
            {
                var getFilteredData = _dataService.GetFilteredData(StudentNo, Month, Year, Date, feesStatus);
                if (getFilteredData != null)
                {

                    response.code = StatusCodes.Status200OK;
                    response.status = true;
                    response.message = CommonMessage.Success;
                    response.data = getFilteredData;
                    return Ok(response);
                }
                response.code = StatusCodes.Status404NotFound;
                response.status = false;
                response.message = CommonMessage.NoDataFound;
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.code = StatusCodes.Status500InternalServerError;
                response.status = false;
                response.message = ex.Message;
                return BadRequest(response);
            }

        }

        [HttpGet(APIUrls.GetStudentsForFilter)]
        public IActionResult GetStudentsForFilter(string? feesStatus)
        {
            Response response = new();
            try
            {
                var students = _dataService.GetStudentsListForFilter(feesStatus);
                if (students.Count() != 0)
                {
                    response.code = StatusCodes.Status200OK;
                    response.status = true;
                    response.message = CommonMessage.Success;
                    response.data = students;
                    return Ok(response);
                }
                response.code = StatusCodes.Status404NotFound;
                response.status = false;
                response.message = CommonMessage.NoDataFound;
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.code = StatusCodes.Status500InternalServerError;
                response.status = false;
                response.message = ex.Message;
                return BadRequest(response);
            }

        }

        [HttpGet(APIUrls.GetLoginReport)]
        public IActionResult GetLoginReport(DateTime date)
        {
            Response response = new();
            try
            {
                var students = _dataService.GetLoginReport(date);
                if (students.Count() != 0)
                {
                    response.code = StatusCodes.Status200OK;
                    response.status = true;
                    response.message = CommonMessage.Success;
                    response.data = students;
                    return Ok(response);
                }
                response.code = StatusCodes.Status404NotFound;
                response.status = false;
                response.message = CommonMessage.NoDataFound;
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.code = StatusCodes.Status500InternalServerError;
                response.status = false;
                response.message = ex.Message;
                return BadRequest(response);
            }

        }

        [HttpGet(APIUrls.DayWiseAttendance)]
        public IActionResult DayWiseAttendance()
        {
            Response response = new();
            try
            {
                var attendances = _dataService.DayWiseAttendance();
                if (attendances != null)
                {
                    response.code = StatusCodes.Status200OK;
                    response.status = true;
                    response.message = CommonMessage.Success;
                    response.data = attendances;
                    return Ok(response);
                }
                response.code = StatusCodes.Status404NotFound;
                response.status = false;
                response.message = CommonMessage.NoDataFound;
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.code = StatusCodes.Status500InternalServerError;
                response.status = false;
                response.message = ex.Message;
                return BadRequest(response);
            }

        }

        [HttpGet(APIUrls.DashboardCounts)]
        public IActionResult DashboardCounts()
        {
            Response response = new();
            try
            {
                var fees = _dataService.GetDashboardCounts();
                if (fees != null)
                {
                    response.code = StatusCodes.Status200OK;
                    response.status = true;
                    response.message = CommonMessage.Success;
                    response.data = fees;
                    return Ok(response);
                }
                response.code = StatusCodes.Status404NotFound;
                response.status = false;
                response.message = CommonMessage.NoDataFound;
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.code = StatusCodes.Status500InternalServerError;
                response.status = false;
                response.message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet(APIUrls.AddDeviceConfigue)]
        public IActionResult AddDeviceConfigue(string DeviceId, string Location, string Type)
        {

            Response response = new();
            try
            {
                DeviceConfig deviceConfig = new();
                deviceConfig.DeviceId = DeviceId;
                deviceConfig.Location = Location;
                deviceConfig.Type = Type;
                TryValidateModel(deviceConfig);
                if (ModelState.IsValid)
                {
                    _dataService.PostDeviceConfig(deviceConfig);
                    response.code = StatusCodes.Status200OK;
                    response.status = true;
                    response.message = CommonMessage.Success;
                    return Ok(response);
                }
                response.code = StatusCodes.Status400BadRequest;
                response.status = false;
                response.message = CommonMessage.NoDataFound;
                response.data = deviceConfig;
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.code = StatusCodes.Status500InternalServerError;
                response.status = false;
                response.message = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet(APIUrls.DeleteDeviceConfig)]
        public IActionResult DeleteDeviceConfig(string DeviceId)
        {

            Response response = new();
            try
            {
                if (DeviceId != null)
                {
                    _dataService.DeleteDeviceConfig(DeviceId);
                    response.code = StatusCodes.Status200OK;
                    response.status = true;
                    response.message = CommonMessage.Success;
                    return Ok(response);
                }
                response.code = StatusCodes.Status400BadRequest;
                response.status = false;
                response.message = CommonMessage.NoDataFound;
                return NotFound(response);
            }
            catch (Exception ex)
            {
                response.code = StatusCodes.Status500InternalServerError;
                response.status = false;
                response.message = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
