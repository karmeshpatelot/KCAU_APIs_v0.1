using AutoMapper.Execution;
using Azure;
using Azure.Identity;
using BioConnect;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MOBILEAPI2024.BLL.Services.IServices;
using MOBILEAPI2024.DAL.Repositories.IRepositories;
using MOBILEAPI2024.DTO.Common;
using MOBILEAPI2024.DTO.RequestDTO.Account;
using MOBILEAPI2024.DTO.RequestDTO.Attendance;
using MOBILEAPI2024.DTO.RequestDTO.Employee;
using MOBILEAPI2024.DTO.ResponseDTO.Account;
using MOBILEAPI2024.DTO.ResponseDTO.Attendance;
using MOBILEAPI2024.DTO.ResponseDTO.Employee;
using MOBILEAPI2024.DTO.ResponseDTO.Student;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Bcpg;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;

namespace MOBILEAPI2024.BLL.Services
{
    public class DataService : IDataService
    {
        private readonly string _loginUrl = "/api/login";
        private readonly IDataRepository _dataRepository;
        private string _sessionCookie;
        private readonly IEmailService _emailHelper;
        private IHostingEnvironment _environment;
        private readonly AppSettings _appSettings;
        private readonly BioConnect_PortClient _bioConnectClient;
        private readonly IMemoryCache _cache;

        public DataService(IMemoryCache cache, IOptions<AppSettings> appsetting, IHostingEnvironment environment, IEmailService emailHelper, IDataRepository dataRepository)
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.TransportCredentialOnly)
            {
                MaxBufferSize = int.MaxValue,
                MaxReceivedMessageSize = int.MaxValue,
                ReaderQuotas = System.Xml.XmlDictionaryReaderQuotas.Max,
                Security =
                {
                    Mode = BasicHttpSecurityMode.TransportCredentialOnly,
                    Transport = new HttpTransportSecurity
                    {
                        ClientCredentialType = HttpClientCredentialType.Windows
                    }
                }
            };

            var endpointAddress = new EndpointAddress("http://41.89.49.64:7047/DynamicsNAV100/WS/KCA%20ERP/Codeunit/BioConnect");

            _bioConnectClient = new BioConnect_PortClient(binding, endpointAddress);
            _bioConnectClient.ClientCredentials.UserName.UserName = "mfibiometric";
            _bioConnectClient.ClientCredentials.UserName.Password = "MF!c0nn3*t";
            _bioConnectClient.ClientCredentials.Windows.ClientCredential.Domain = "KCAU";
            _bioConnectClient.ClientCredentials.Windows.ClientCredential.UserName = "mfibiometric";
            _bioConnectClient.ClientCredentials.Windows.ClientCredential.Password = "MF!c0nn3*t";

            _dataRepository = dataRepository;
            _emailHelper = emailHelper;
            _environment = environment;
            _appSettings = appsetting.Value;
            _cache = cache;
        }

        public LoginAPIResponse GetLoginUser()
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;

            using (var httpClient = new HttpClient(handler))
            {
                var loginData = new
                {
                    notification_token = "string",
                    mobile_device_type = "ANDROID",
                    mobile_os_version = "string",
                    mobile_app_version = "string",
                    user_id = "admin",
                    password = "admin2024"
                };

                var jsonContent = JsonConvert.SerializeObject(loginData);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                string url = "https://192.168.9.16:3002/tna/login";
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };

                request.Headers.Add("accept", "application/json");

                var response = httpClient.SendAsync(request).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    // Capture session cookie
                    if (response.Headers.TryGetValues("Set-Cookie", out var cookies))
                    {
                        foreach (var cookie in cookies)
                        {
                            _sessionCookie = cookie;
                            break;
                        }
                    }

                    var responseData = response.Content.ReadAsStringAsync().Result;
                    var apiResponse = JsonConvert.DeserializeObject<LoginAPIResponse>(responseData);

                    return apiResponse;
                }
                else
                {
                    return null;
                }
            }
        }

        public (string FromDate, string ToDate) GetFromAndToDate(int month)
        {
            // Get the current year
            int currentYear = DateTime.Now.Year;

            // Get the first day of the month
            DateTime fromDate = new DateTime(currentYear, month, 1);

            // Get the last day of the month by adding 1 month and subtracting 1 day
            DateTime toDate = fromDate.AddMonths(1).AddDays(-1);

            // Format the dates in "yyyy-MM-dd" format
            return (fromDate.ToString("yyyy-MM-dd"), toDate.ToString("yyyy-MM-dd"));
        }

        public int GetTotalDaysInMonth(int? month = null)
        {
            // Get the current year
            int currentYear = DateTime.Now.Year;

            // If month is not provided, use the current month
            int selectedMonth = month ?? DateTime.Now.Month;

            // Get the total number of days in the selected month
            int totalDays = DateTime.DaysInMonth(currentYear, selectedMonth);

            return totalDays;
        }

        public ReportResponse GetInOutData(int month)
        {
            if (string.IsNullOrEmpty(_sessionCookie))
            {
                throw new InvalidOperationException("Session not initialized. Please login first.");
            }
            var (FromDate, ToDate) = GetFromAndToDate(month);
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;

            using (var httpClient = new HttpClient(handler))
            {
                var reportRequest = new
                {
                    limit = 1000,
                    offset = 0,
                    type = "CUSTOM",
                    start_datetime = FromDate,
                    end_datetime = ToDate,
                    group_id_list = new List<string> { "1" },
                    report_type = "REPORT_DAILY",
                    report_filter_type = "",
                    language = "en",
                    rebuild_time_card = true,
                    columns = new List<Dictionary<string, object>> { new Dictionary<string, object>() }
                };


                var jsonContent = JsonConvert.SerializeObject(reportRequest);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                string url = _appSettings.APIUri + ":3002/tna/report.json";
                var request = new HttpRequestMessage(HttpMethod.Post, url)
                {
                    Content = content
                };

                // Add session cookie to request headers
                request.Headers.Add("accept", "application/json");
                request.Headers.Add("Cookie", _sessionCookie);

                var response = httpClient.SendAsync(request).GetAwaiter().GetResult();

                if (response.IsSuccessStatusCode)
                {
                    var responseData = response.Content.ReadAsStringAsync().Result;
                    var reportResponse = JsonConvert.DeserializeObject<ReportResponse>(responseData);

                    return reportResponse;
                }
                else
                {
                    return null;
                }
            }
        }

        public Staff GetStaffFromERP(string? staffNo, int entriesPerPage, int pageNo)
        {
            try
            {
                if (staffNo == null || staffNo == "null")
                {
                    staffNo = "";
                }
                else if (staffNo != null)
                {
                    entriesPerPage = 1;
                    pageNo = 1;
                }

                // Call the service's GetStaff method synchronously
                var response = _bioConnectClient.GetStaffAsync(staffNo, entriesPerPage, pageNo).Result;

                // Extract the return_value (which is a JSON string)
                var jsonResult = response.return_value;

                // Call the GetStudents method synchronously

                // Convert the JSON string to a list of objects
                var students = JsonConvert.DeserializeObject<Staff>(jsonResult);
                foreach (Result staff in students.Result)
                {
                    //_dataRepository.InsertStaffRecord(staff);
                }
                //var response1 = _bioConnectClient.GetStudentsAsync().Result;
                return students;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public Staff GetStaffFromERPForLogin(string? staffNo, int entriesPerPage, int pageNo)
        {
            try
            {
                if (staffNo == null || staffNo == "null")
                {
                    staffNo = "";
                }
                else if (staffNo != null)
                {
                    entriesPerPage = 1;
                    pageNo = 1;
                }

                // Call the service's GetStaff method synchronously
                var response = _bioConnectClient.GetStaffAsync(staffNo, entriesPerPage, pageNo).Result;

                // Extract the return_value (which is a JSON string)
                var jsonResult = response.return_value;

                // Call the GetStudents method synchronously

                // Convert the JSON string to a list of objects
                var students = JsonConvert.DeserializeObject<Staff>(jsonResult);
                //foreach (Result staff in students.Result)
                //{
                //    _dataRepository.InsertStaffRecord(staff);
                //}
                //var response1 = _bioConnectClient.GetStudentsAsync().Result;
                return students;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public void InsertInOutData(Record record)
        {
            PunchModel punchModel = new();


            punchModel.UserName = record.userId;
            punchModel.EnrollNo = record.userName;
            punchModel.LocationType = "Campus";
            punchModel.ForDate = Convert.ToDateTime(record.datetime);
            punchModel.InTime = Convert.ToDateTime(record.inTime);
            punchModel.OutTime = Convert.ToDateTime(record.outTime);
            punchModel.Duration = record.totalWorkTime;
            punchModel.IpAddress = "KCAU";

            _dataRepository.PostInOutPunch(punchModel);

        }

        public Students GetStudents(string? studentNo, int entriesPerPage, int pageNo)
        {
            try
            {
                //if (studentNo == null || studentNo == "null")
                //{
                //    studentNo = "";
                //}
                //else if (studentNo != null)
                //{
                //    entriesPerPage = 1;
                //    pageNo = 1;
                //}
                var response = _bioConnectClient.GetStudentsAsync(studentNo, entriesPerPage, pageNo).Result;
                if (response.Body.return_value != null)
                {
                    var jsonResult = response.Body.return_value;

                    var students = JsonConvert.DeserializeObject<Students>(jsonResult);

                    foreach (Student student in students.Result)
                    {
                        _dataRepository.InsertStudentRecords(student);
                    }

                    return students;
                }
                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public object LoginUser(LoginModel loginModel)
        {
            throw new NotImplementedException();
        }

        public void SendEmailOTP(Student student)
        {

            string otp = GenerateOtp();
            OTPModel oTPModel = new();
            oTPModel.Type = "Student";
            oTPModel.UserName = student.student_no;
            oTPModel.Email = student.Email;
            oTPModel.OTP = otp;

            _dataRepository.AddOtp(oTPModel, otp);

            // Send email 
            var vSubject = "Login OTP";
            string path = Path.Combine(_environment.WebRootPath, "EmailTemplates/Otp.html");
            using var vStreamReader = System.IO.File.OpenText(path);
            var vTemplate = vStreamReader.ReadToEnd();
            vTemplate = vTemplate.Replace("{PageTitle}", vSubject);
            vTemplate = vTemplate.Replace("{OTP}", otp);
            vTemplate = vTemplate.Replace("{Name}", student.name);
            _emailHelper.SendEmail(student.name, "karmesh.p@orangewebtech.com", vSubject, vTemplate);
            _emailHelper.SendEmail(student.name, "p.gawade@groupmfi.com", vSubject, vTemplate);
        }

        public string GenerateOtp()
        {
            // Create a random number generator
            Random rand = new Random();

            // Generate a random 4-digit number
            int otp = rand.Next(1000, 10000);

            // Convert the number to a string and return
            return otp.ToString();
        }

        public object OtpVerification(string userName, int otpCode)
        {
            var verifyOtp = _dataRepository.OtpVerification(userName, otpCode);
            if (verifyOtp != null)
            {
                LoginReportKCA loginReportKCA = new();
                if (verifyOtp.Type == "Student")
                {
                    var user = GetStudents(userName, 1, 1);
                    if (user != null)
                    {
                        loginReportKCA.UserName = user.Result[0].student_no;
                        loginReportKCA.Type = "Student";
                        loginReportKCA.LoginTime = DateTime.UtcNow;
                        _dataRepository.PostLoginReport(loginReportKCA);
                    }
                }
                else if (verifyOtp.Type == "Staff")
                {
                    var staffuser = GetStaffFromERP(userName, 1, 1);
                    if (staffuser != null)
                    {
                        loginReportKCA.UserName = staffuser.Result[0].staff_no;
                        loginReportKCA.Type = "Staff";
                        loginReportKCA.LoginTime = DateTime.UtcNow;
                        _dataRepository.PostLoginReport(loginReportKCA);
                    }
                }
                else if (verifyOtp.Type == "Admin")
                {
                    var adminUser = _dataRepository.GetAdminUser(userName);
                    if (adminUser != null)
                    {
                        loginReportKCA.UserName = adminUser.AdminName;
                        loginReportKCA.Type = "Admin";
                        loginReportKCA.LoginTime = DateTime.UtcNow;
                        _dataRepository.PostLoginReport(loginReportKCA);

                    }
                }
                return verifyOtp;
            }
            return null;
        }

        public void DeleteExpiredOTP()
        {
            _dataRepository.DeleteExpiredOTP();
        }

        public void SendEmailOTPStaff(Result result)
        {
            string otp = GenerateOtp();
            OTPModel oTPModel = new();
            oTPModel.Type = "Staff";
            oTPModel.UserName = result.staff_no;
            oTPModel.Email = result.Email;
            oTPModel.OTP = otp;

            _dataRepository.AddOtp(oTPModel, otp);
            // Send email 
            var vSubject = "Login OTP";
            string path = Path.Combine(_environment.WebRootPath, "EmailTemplates/Otp.html");
            using var vStreamReader = System.IO.File.OpenText(path);
            var vTemplate = vStreamReader.ReadToEnd();
            vTemplate = vTemplate.Replace("{PageTitle}", vSubject);
            vTemplate = vTemplate.Replace("{OTP}", otp);
            vTemplate = vTemplate.Replace("{Name}", result.first_name + " " + result.middle_name + " " + result.last_name);
            _emailHelper.SendEmail(result.first_name + " " + result.middle_name + " " + result.last_name, "karmesh.p@orangewebtech.com", vSubject, vTemplate);
            _emailHelper.SendEmail(result.first_name + " " + result.middle_name + " " + result.last_name, "p.gawade@groupmfi.com", vSubject, vTemplate);
        }

        public PunchModel CheckDataExists(string? userId, string? datetime)
        {
            var checkExists = _dataRepository.CheckDataExists(userId, datetime);
            if (checkExists != null)
            {
                return checkExists;
            }
            return null;
        }

        public void AddCampusAttendance(AddCampusInOut addCampusInOut)
        {
            _dataRepository.AddCampusAttendance(addCampusInOut);
        }

        public void AddLectureAttendance(AddLectureAttendance addLectureAttendance)
        {
            _dataRepository.AddLectureAttendance(addLectureAttendance);
        }

        public LoginResponse GetLoginAccess()
        {
            try
            {
                // Ignore SSL certificate validation (for testing purposes)
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;

                using (var client = new HttpClient(handler))
                {
                    // Set the base address and configure the client
                    client.BaseAddress = new Uri(_appSettings.APIUri + _loginUrl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Define the request body
                    var requestBody = new
                    {
                        User = new
                        {
                            login_id = "admin",
                            password = "admin2024"
                        }
                    };

                    // Serialize the request body to JSON
                    var content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(requestBody));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    // Send the POST request
                    var response = client.PostAsync(_appSettings.APIUri + _loginUrl, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // Extract the bs-session-id from the response headers
                        if (response.Headers.TryGetValues("bs-session-id", out var sessionIdValues))
                        {
                            var sessionId = sessionIdValues.GetEnumerator();
                            sessionId.MoveNext(); // Move to the first (and likely only) value
                            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
                            string key = "SessionId";
                            _cache.Set(key, sessionId.Current, cacheEntryOptions);
                            return new LoginResponse
                            {
                                SessionId = sessionId.Current,
                                Status = "Login successful"
                            };
                        }
                        else
                        {
                            return null;
                        }
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                return null;
            }
        }

        public Students GetStudentsData(string? studentNo, int entriesPerPage, int pageNo)
        {

            if (studentNo != null)
            {
                entriesPerPage = 1;
                pageNo = 1;
            }

            var students = _dataRepository.GetStudentsData(studentNo, entriesPerPage, pageNo);
            if (students != null)
            {
                return students;
            }
            return null;

            //try
            //{
            //    if (studentNo == null || studentNo == "null")
            //    {
            //        studentNo = "";
            //    }
            //    else if (studentNo != null)
            //    {
            //        entriesPerPage = 1;
            //        pageNo = 1;
            //    }
            //    var response = _bioConnectClient.GetStudentsAsync(studentNo, entriesPerPage, pageNo).Result;
            //    if (response.Body.return_value != null)
            //    {
            //        var jsonResult = response.Body.return_value;

            //        var students = JsonConvert.DeserializeObject<Students>(jsonResult);

            //        return students;
            //    }
            //    return null;

            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"An error occurred: {ex.Message}");
            //    return null;
            //}
        }

        public Students GetStudentsForFeesCheck(string? studentNo, int entriesPerPage, int pageNo)
        {
            try
            {
                if (studentNo == null || studentNo == "null")
                {
                    studentNo = "";
                }
                else if (studentNo != null)
                {
                    entriesPerPage = 1;
                    pageNo = 1;
                }
                var response = _bioConnectClient.GetStudentsAsync(studentNo, entriesPerPage, pageNo).Result;
                if (response.Body.return_value != null)
                {
                    var jsonResult = response.Body.return_value;

                    var students = JsonConvert.DeserializeObject<Students>(jsonResult);

                    return students;
                }
                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }

        public AttendanceResponse GetAttendanceFromBS(string sessionId, string deviceId)
        {
            try
            {
                // Create a handler to ignore SSL certificate validation (for testing purposes)
                HttpClientHandler handler = new HttpClientHandler();
                handler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) => true;

                using (var client = new HttpClient(handler))
                {
                    // Set the base address
                    client.BaseAddress = new Uri($"{_appSettings.APIUri}/api/events/search");
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Add the session ID header
                    client.DefaultRequestHeaders.Add("bs-session-id", sessionId);

                    // Create request body
                    var requestBody = new AttendanceRequest
                    {
                        Query = new Query
                        {
                            limit = 1,
                            conditions = new List<Condition>
                    {
                        new Condition
                        {
                            column = "device_id.id",
                            Operator = 0,
                            values = new List<string> { deviceId }
                        }
                    },
                            orders = new List<Order>
                    {
                        new Order
                        {
                            column = "datetime",
                            descending = true
                        }
                    }
                        }
                    };

                    // Serialize request body to JSON
                    var jsonRequestBody = Newtonsoft.Json.JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(jsonRequestBody, System.Text.Encoding.UTF8, "application/json");

                    // Send the POST request
                    var response = client.PostAsync("", content).Result;

                    // Check if the response was successful
                    if (response.IsSuccessStatusCode)
                    {
                        // Deserialize the JSON response into AttendanceResponse model
                        var responseData = response.Content.ReadAsStringAsync().Result;
                        Console.WriteLine("Raw JSON Response: " + responseData);

                        var attendanceResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<AttendanceResponse>(responseData);
                        return attendanceResponse;
                    }
                    else
                    {
                        Console.WriteLine("Request failed with status code: " + response.StatusCode);
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error occurred: " + ex.Message);
                return null;
            }
        }

        public Students GetAttendanceFromDB(EventCollection getAttendance, LoginResponse getLoginAccess, string deviceId)
        {
            if (getAttendance.rows[0].user_id.user_id == null || getAttendance.rows[0].user_id.user_id == "0")
            {
                return null;
            }
            PunchModel punchModel = new();

            punchModel.UserName = getAttendance.rows[0].user_id.user_id;
            punchModel.EnrollNo = getAttendance.rows[0].user_id.name;
            punchModel.LocationType = "Campus";
            punchModel.ForDate = getAttendance.rows[0].server_datetime.Date;
            punchModel.InTime = getAttendance.rows[0].datetime;
            punchModel.OutTime = getAttendance.rows[0].datetime;
            punchModel.Duration = "00.00";
            punchModel.IpAddress = "KCAU";
            punchModel.DeviceId = getAttendance.rows[0].device_id.id;
            punchModel.FeesStatus = "";

            var getAttendance1 = _dataRepository.GetAttendanceFromDB(deviceId, getAttendance);
            if (getAttendance1 != null)
            {
                if (getAttendance1.RowId == getAttendance.rows[0].id)
                {
                    return null;
                }
                string user = null;
                user = getAttendance.rows[0].user_id.user_id.Replace("_", "/");
                var getStudentDetails = GetStudentsForFeesCheck(user, 1, 1);
                if (getStudentDetails.Result.Count() != 0)
                {
                    return getStudentDetails;
                }
                return null;
            }
            else
            {
                string user = null;
                user = getAttendance.rows[0].user_id.user_id.Replace("_", "/");
                var getStudentDetails = GetStudentsForFeesCheck(user, 1, 1);
                if (getStudentDetails.Result.Count() != 0)
                {
                    if (getStudentDetails.Result[0].Allow == "TRUE")
                    {
                        punchModel.FeesStatus = "PAID";
                    }
                    else if (getStudentDetails.Result[0].Allow == "FALSE")
                    {
                        punchModel.FeesStatus = "UNPAID";
                    }
                    _dataRepository.PostInOutPunch(punchModel);
                    _dataRepository.InsertAttendanceEvents(getAttendance,deviceId);
                    return getStudentDetails;
                }
            }
            return null;
        }

        public List<StudentList> GetStudentsListForFilter()
        {
            var students = _dataRepository.GetStudentsListForFilter();
            if (students != null)
            {
                return students;
            }
            return null;
        }

        public AttendanceSummery GetFilteredData(string studentNo, int month, int year, DateTime? selectedDate)
        {
            AttendanceSummery attendanceSummery = new();

            // Set the FromDate and ToDate for the entire month
            DateTime fromDate = new DateTime(year, month, 1);
            DateTime toDate = fromDate.AddMonths(1).AddDays(-1); // End of the month
            int Present = 0;
            int Absent = 0;
            DateTime currentDate = DateTime.Now; // Get the current date

            // Get attendance data for the user within the specified month
            var attendanceData = _dataRepository.GetFilteredData(studentNo, fromDate, toDate);

            List<AttendanceReport> fullMonthAttendance = new List<AttendanceReport>();

            // If a specific date is provided, only process that day
            if (selectedDate.HasValue)
            {
                DateTime day = selectedDate.Value.Date;

                // Check if the selected date is greater than the current date
                if (day > currentDate)
                {
                    // Skip processing if the date is in the future
                    return attendanceSummery;
                }

                // Get attendance data for the specific date
                var attendanceForDay = attendanceData.Where(a => a.ForDate.Date == day.Date).ToList();

                if (attendanceForDay.Any())
                {
                    // Add records for the specific day
                    fullMonthAttendance.AddRange(attendanceForDay.Select(a => new AttendanceReport
                    {
                        UserName = a.UserName,
                        EnrollNo = a.EnrollNo,
                        ForDate = a.ForDate,
                        InTime = a.InTime,
                        DeviceId = a.DeviceId,
                        FeesStatus = a.FeesStatus,
                        AttendanceStatus = a.FeesStatus == "UNPAID" ? "Absent" : "Present"
                    }));

                    // Count each valid punch (attendance record) as Present unless it is "UNPAID"
                    Present += attendanceForDay.Count(a => a.FeesStatus != "UNPAID");

                    // Count any "UNPAID" as Absent
                    Absent += attendanceForDay.Count(a => a.FeesStatus == "UNPAID");
                }
                else
                {
                    // If no attendance data for the selected date, mark it as absent
                    fullMonthAttendance.Add(new AttendanceReport
                    {
                        UserName = studentNo,
                        EnrollNo = studentNo, // Use studentNo as EnrollNo for the absent record
                        ForDate = day,
                        InTime = null, // Set InTime to null for absent
                        FeesStatus = null, // No need to set FeesStatus to "UNPAID"
                        AttendanceStatus = "Absent"
                    });
                    Absent++;
                }
            }
            else
            {
                // Original behavior: Loop through each day of the month
                for (DateTime day = fromDate; day <= toDate; day = day.AddDays(1))
                {
                    // Check if the date is greater than the current date
                    if (day > currentDate)
                    {
                        // Skip marking anything if the date is in the future
                        continue;
                    }

                    // Get attendance data for the current day
                    var attendanceForDay = attendanceData.Where(a => a.ForDate.Date == day.Date).ToList();

                    if (attendanceForDay.Any())
                    {
                        // Add all records for that day
                        fullMonthAttendance.AddRange(attendanceForDay.Select(a => new AttendanceReport
                        {
                            UserName = a.UserName,
                            EnrollNo = a.EnrollNo,
                            ForDate = a.ForDate,
                            InTime = a.InTime,
                            DeviceId = a.DeviceId,
                            FeesStatus = a.FeesStatus,
                            AttendanceStatus = a.FeesStatus == "UNPAID" ? "Absent" : "Present"
                        }));

                        // Count each valid punch (attendance record) as Present unless it is "UNPAID"
                        Present += attendanceForDay.Count(a => a.FeesStatus != "UNPAID");

                        // Count any "UNPAID" as Absent
                        Absent += attendanceForDay.Count(a => a.FeesStatus == "UNPAID");
                    }
                    else
                    {
                        // If no attendance data for the current day, mark it as absent
                        fullMonthAttendance.Add(new AttendanceReport
                        {
                            UserName = studentNo,
                            EnrollNo = studentNo, // Use studentNo as EnrollNo for the absent record
                            ForDate = day,
                            InTime = null, // Set InTime to null for absent
                            FeesStatus = null, // No need to set FeesStatus to "UNPAID"
                            AttendanceStatus = "Absent"
                        });
                        Absent++;
                    }
                }
            }

            attendanceSummery.Attendances = fullMonthAttendance;
            attendanceSummery.Present = Present;
            attendanceSummery.Absent = Absent;
            attendanceSummery.Name = attendanceData.Any() ? attendanceData[0].EnrollNo : studentNo;

            return attendanceSummery;
        }

        public Staff GetStaff(string? staffNo, int entriesPerPage, int pageNo)
        {
            if (staffNo != null)
            {
                entriesPerPage = 1;
                pageNo = 1;
            }
            var GetStaffs = _dataRepository.GetStaffDataFromDB(staffNo, entriesPerPage, pageNo);
            if (GetStaffs != null)
            {
                return GetStaffs;
            }
            return null;
        }

        public List<LoginReportKCA> GetLoginReport(DateTime date)
        {
            var Getlogins = _dataRepository.GetLoginReport(date);
            if (Getlogins != null)
            {
                return Getlogins;
            }
            return null;
        }

        public string SendEmailToAdmin(string email)
        {
            var getAdmin = _dataRepository.GetAdminUser(email);
            if (getAdmin != null)
            {
                string otp = GenerateOtp();
                OTPModel oTPModel = new();
                oTPModel.Type = "Admin";
                oTPModel.UserName = getAdmin.AdminName;
                oTPModel.Email = getAdmin.Email;
                oTPModel.OTP = otp;

                _dataRepository.AddOtp(oTPModel, otp);
                // Send email 
                var vSubject = "Login OTP";
                string path = Path.Combine(_environment.WebRootPath, "EmailTemplates/Otp.html");
                using var vStreamReader = System.IO.File.OpenText(path);
                var vTemplate = vStreamReader.ReadToEnd();
                vTemplate = vTemplate.Replace("{PageTitle}", vSubject);
                vTemplate = vTemplate.Replace("{OTP}", otp);
                vTemplate = vTemplate.Replace("{Name}", getAdmin.AdminName);
                _emailHelper.SendEmail(getAdmin.AdminName, getAdmin.Email, vSubject, vTemplate);
                return "Success";
            }
            return "Fail";
        }

        public List<DayWiseAttendance> DayWiseAttendance()
        {
            var getAttendance = _dataRepository.DayWiseAttendance();
            if (getAttendance != null)
            {
                return getAttendance;
            }
            return null;

        }

        public FeesCounts GetFeesCounts()
        {
            var getFees = _dataRepository.GetFeesCounts();
            if (getFees != null)
            {
                return getFees;
            }
            return null;
        }

        public AttendanceCount GetDashbordAttendanceReport()
        {
            var attendanceCount = _dataRepository.GetDashbordAttendanceReport();
            if (attendanceCount != null)
            {
                return attendanceCount;
            }
            return null;
        }

        public DashboardCount GetDashboardCounts()
        {
            DashboardCount dashboardCount = new();
            var getFees = _dataRepository.GetFeesCounts();
            if (getFees != null)
            {
                dashboardCount.PaidCount = getFees.PaidCount;
                dashboardCount.UnPaidCount = getFees.UnPaidCount;
            }
            else
            {
                dashboardCount.PaidCount = 0;
                dashboardCount.UnPaidCount = 0;
            }
            var attendanceCount = _dataRepository.GetDashbordAttendanceReport();
            if (attendanceCount != null)
            {
                dashboardCount.PresentCount = attendanceCount.PresentCount;
                dashboardCount.AbsentCount = attendanceCount.AbsentCount;
            }
            else
            {
                dashboardCount.PresentCount = 0;
                dashboardCount.AbsentCount = 0;
            }
            return dashboardCount;
        }

        public string CheckCampusAttendance(AddCampusInOut addCampusInOut)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
            var checkAttendance = _dataRepository.CheckCampusAttendance(addCampusInOut);
            if (checkAttendance != null && checkAttendance.InOutTime != addCampusInOut.DateTime)
            {
                _cache.Set("Popup", "Display", cacheEntryOptions);
                return "True";
            }
            return "False";
        }

        public List<ClassAttendanceForCal> GetClassAttendance(string? username, int month)
        {

            // Set the FromDate and ToDate for the entire month
            DateTime fromDate = new DateTime(DateTime.Now.Year, month, 1);
            DateTime toDate = DateTime.Now.Date; // End of the month
            var getAttendance = _dataRepository.GetClassAttendance(username, fromDate, toDate);
            if (getAttendance != null)
            {
                return getAttendance;
            }
            return null;
        }

        public void PostDeviceConfig(DeviceConfig deviceConfig)
        {
            var checkExists = _dataRepository.CheckDeviceConfig(deviceConfig.DeviceId);
            if (checkExists == null)
            {
                _dataRepository.PostDeviceConfig(deviceConfig);
            }
            else
            {
                _dataRepository.UpdateDeviceConfig(deviceConfig);
            }
        }

        public DeviceConfigurationKCA CheckDeviceConfig(string deviceId)
        {
            var getDeviceConfig = _dataRepository.CheckDeviceConfig(deviceId);
            if (getDeviceConfig != null)
            {
                return getDeviceConfig;
            }
            return null;
        }

        public void DeleteDeviceConfig(string deviceId)
        {
            var getDeviceConfig = _dataRepository.CheckDeviceConfig(deviceId);
            if (getDeviceConfig != null)
            {
                _dataRepository.DeleteDeviceConfig(deviceId);
            }
        }

        public List<CampusAttendance> GetCampusAttendance(string? username, int month)
        {
            // Set the FromDate and ToDate for the entire month
            DateTime fromDate = new DateTime(DateTime.Now.Year, month, 1);
            DateTime toDate = DateTime.Now.Date; // End of the month
            var getAttendance = _dataRepository.GetCampusAttendance(username, fromDate, toDate);
            if (getAttendance != null)
            {
                return getAttendance;
            }
            return null;
        }
    }
}
