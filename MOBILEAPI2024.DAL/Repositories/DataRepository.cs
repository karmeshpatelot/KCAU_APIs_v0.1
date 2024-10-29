using Dapper;
using MOBILEAPI2024.DAL.Entities;
using MOBILEAPI2024.DAL.Repositories.IRepositories;
using MOBILEAPI2024.DTO.RequestDTO.Attendance;
using MOBILEAPI2024.DTO.RequestDTO.Employee;
using MOBILEAPI2024.DTO.ResponseDTO.Attendance;
using MOBILEAPI2024.DTO.ResponseDTO.Employee;
using MOBILEAPI2024.DTO.ResponseDTO.Student;
using System;
using static System.Net.WebRequestMethods;

namespace MOBILEAPI2024.DAL.Repositories
{
    public class DataRepository : SqlDbRepository<ActiveInActiveUser>, IDataRepository
    {
        public DataRepository(string connectionString) : base(connectionString)
        {
        }

        public void AddCampusAttendance(AddCampusInOut addCampusInOut)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@UserName", addCampusInOut.UserName);
            vParams.Add("@Campus", addCampusInOut.Campus);
            vParams.Add("@In_Out_Time", addCampusInOut.DateTime);
            vParams.Add("@ForDate", addCampusInOut.DateTime.Date);
            vParams.Add("@IO_Flag", addCampusInOut.IOFlag);
            string Query = @" INSERT INTO Campus_Attendance_KCA
                (UserName, Campus, In_Out_Time, ForDate, IO_Flag, CreatedDate) 
            VALUES 
                (@UserName, @Campus, @In_Out_Time, @ForDate, @IO_Flag, GETDATE());
            ";
            vconn.Execute(Query, vParams);
        }

        public void AddLectureAttendance(AddLectureAttendance addLectureAttendance)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@UserName", addLectureAttendance.UserName);
            vParams.Add("@Campus", addLectureAttendance.CampusName);
            vParams.Add("@LectureName", addLectureAttendance.LectureName);
            vParams.Add("@InOutTime", addLectureAttendance.DateTime);
            vParams.Add("@ForDate", addLectureAttendance.DateTime.Date);
            string Query = @" INSERT INTO [dbo].[Lecture_Attendance_KCA] 
                    (
                        [UserName],
                        [LectureName],
                        [Campus],
                        [InOutTime],
                        [ForDate],
                        [CreatedDate]
                    )
                    VALUES 
                    (
                        @UserName,
                        @LectureName,
                        @Campus,
                        @InOutTime,
                        @ForDate,
                        GETDATE() -- CreatedDate (current date and time)
                    );
            ";
            vconn.Execute(Query, vParams);
        }

        public void AddOtp(OTPModel oTPmodel, string otp)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@Type", oTPmodel.Type);
            vParams.Add("@UserName", oTPmodel.UserName);
            vParams.Add("@Email", oTPmodel.Email);
            vParams.Add("@Otp", otp);
            string Query = @" INSERT INTO KCA_OTP_VERIFY (Type,UserName, Email, OTP, IsVerify, CreatedDate)
                              VALUES (@Type,@UserName, @Email, @OTP, 0, GETDATE());";
            vconn.Execute(Query, vParams);
        }

        public InOutRecord CheckCampusAttendance(AddCampusInOut addCampusInOut)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@ForDate", addCampusInOut.DateTime.Date);
            vParams.Add("@UserName", addCampusInOut.UserName);
            vParams.Add("@IOFlag", addCampusInOut.IOFlag);
            vParams.Add("@Campus", addCampusInOut.Campus);
            string query = @"select * from Campus_Attendance_KCA where ForDate = @ForDate and IO_Flag = @IOFlag and UserName = @UserName;";
            var data = vconn.QueryFirstOrDefault<InOutRecord>(query, vParams);
            return data;

        }

        public PunchModel CheckDataExists(string? userId, string? datetime)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@UserName", userId);
            vParams.Add("ForDate", Convert.ToDateTime(datetime));
            string query = @"select * from 	User_InOut_Records_KCA where UserName = @UserName and ForDate = @ForDate";
            var data = vconn.QueryFirstOrDefault<PunchModel>(query, vParams);
            return data;
        }

        public DeviceConfigurationKCA CheckDeviceConfig(string deviceId)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@DeviceId", deviceId);
            string query = @"select * from Device_Configuration_KCA where DeviceId = @DeviceId";
            var data = vconn.QueryFirstOrDefault<DeviceConfigurationKCA>(query, vParams);
            return data;

        }

        public EmpBasic CheckEnrollNoExists(long? cmp_ID, string? enroll_No)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@Enroll_No", enroll_No);
            vParams.Add("@Cmp_ID", cmp_ID);

            string Query = @" select Emp_ID,Cmp_ID,Emp_code from T0080_EMP_MASTER where cmp_id=@Cmp_ID and Enroll_No=@Enroll_No;";
            var empData = vconn.QueryFirstOrDefault<EmpBasic>(Query, vParams);
            if (empData != null)
            {
                return empData;
            }
            return null;
        }

        public bool CheckRecordExistence(Record record, int? cmp_ID, int? emp_Id)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@Emp_ID", emp_Id);
            vParams.Add("@Cmp_ID", cmp_ID);
            vParams.Add("@For_Date", Convert.ToDateTime(record.datetime));

            string Query = @"select * from T0150_EMP_INOUT_RECORD where Emp_ID = @Emp_ID and Cmp_ID = @Cmp_ID and For_Date = @For_Date";
            var empData = vconn.QueryFirstOrDefault(Query, vParams);
            if (empData != null)
            {
                return true;
            }
            return false;
        }

        public List<DayWiseAttendance> DayWiseAttendance()
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();

            vParams.Add("@ForDate", DateTime.Now.Date);

            string Query = @"
                            SELECT 
                            EnrollNo AS UserName, 
                        	UserName As EnrollNo,
                            SUM(CASE WHEN FeesStatus = 'PAID' THEN 1 ELSE 0 END) AS PresentCount,
                            SUM(CASE WHEN FeesStatus = 'UNPAID' THEN 1 ELSE 0 END) AS AbsentCount
                        FROM 
                            User_InOut_Records_KCA
                        WHERE 
                            CONVERT(DATE, ForDate) = @ForDate  
                        GROUP BY 
                            UserName,
                        	EnrollNo;";
            var getAttendance = vconn.Query<DayWiseAttendance>(Query, vParams).ToList();
            return getAttendance;
        }

        public void DeleteDeviceConfig(string deviceId)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@DeviceId", deviceId);
            string Query = @"delete from Device_Configuration_KCA where DeviceId = @DeviceId";
            vconn.Execute(Query, vParams);
        }

        public void DeleteExpiredOTP()
        {
            using var vconn = GetOpenConnection();
            string Query = @"DELETE FROM KCA_OTP_VERIFY
                    WHERE CreatedDate < DATEADD(MINUTE, -5, GETDATE());";
            vconn.Execute(Query);
        }

        public AdminModel GetAdminUser(string email)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@Email", email);
            string query = @"select * from Admin_Users_KCA where Email = @Email and Status = 1;";
            var data = vconn.QueryFirstOrDefault<AdminModel>(query, vParams);
            return data;
        }

        public AttendanceEventKCA GetAttendanceFromDB(string deviceId, EventCollection getAttendance)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@RowId", getAttendance.rows[0].id);
            vParams.Add("@DeviceId", deviceId);
            string query = @"select * from Attendance_Events_KCA where RowId = @RowId And DeviceId  = @DeviceId;";
            var data = vconn.QueryFirstOrDefault<AttendanceEventKCA>(query, vParams);
            return data;
        }

        public List<CampusAttendance> GetCampusAttendance(string? username, DateTime fromDate, DateTime toDate)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@UserName", username);
            vParams.Add("@FromDate", fromDate.Date);
            vParams.Add("@ToDate", toDate.Date);
            string query = @"WITH DateRange AS (
                            SELECT CAST(@FromDate AS DATE) AS ForDate
                            UNION ALL
                            SELECT DATEADD(DAY, 1, ForDate)
                            FROM DateRange
                            WHERE ForDate < CAST(@ToDate AS DATE)
                        )
                        SELECT 
                            dr.ForDate,
                            c.UserName,
                            ISNULL(MIN(CASE WHEN c.IO_Flag = 'IN' THEN c.In_Out_Time END), '') AS InTime,
                            ISNULL(MAX(CASE WHEN c.IO_Flag = 'OUT' THEN c.In_Out_Time END), '') AS OutTime
                        FROM 
                            DateRange dr
                        LEFT JOIN 
                            Campus_Attendance_KCA c ON c.ForDate = dr.ForDate AND c.UserName = @UserName
                        WHERE 
                            dr.ForDate BETWEEN @FromDate AND @ToDate
                        GROUP BY 
                            dr.ForDate, c.UserName
                        ORDER BY 
                            dr.ForDate
                        OPTION (MAXRECURSION 0);
                        
                        ";
            string query2 = @"
                select UserName,ForDate,InTime,OutTime from User_InOut_Records_KCA where LocationType = 'campus' and UserName =@UserName and ForDate between @FromDate and @ToDate";
            var data = vconn.Query<CampusAttendance>(query, vParams).ToList();
            return data;
        }

        public List<ClassAttendanceForCal> GetClassAttendance(string? username, DateTime fromDate, DateTime toDate)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@UserName", username);
            vParams.Add("@FromDate", fromDate.Date);
            vParams.Add("@ToDate", toDate.Date);
            string query = @"WITH DateRange AS (
                            SELECT CAST('2024-10-01' AS DATE) AS ForDate
                            UNION ALL
                            SELECT DATEADD(DAY, 1, ForDate)
                            FROM DateRange
                            WHERE ForDate < CAST('2024-10-25' AS DATE)
                        )
                        SELECT 
                            dr.ForDate,
                            c.UserName,
                            c.InTime,
                            c.OutTime,
                            c.FeesStatus
                        FROM 
                            DateRange dr
                        INNER JOIN 
                            User_InOut_Records_KCA c ON c.ForDate = dr.ForDate AND c.UserName = @UserName AND c.LocationType = 'lecture'
                        WHERE 
                            dr.ForDate BETWEEN @FromDate AND @ToDate
                        ORDER BY 
                            dr.ForDate, c.InTime
                        OPTION (MAXRECURSION 0);
                                                
                        ";
            var data = vconn.Query<ClassAttendanceForCal>(query, vParams).ToList();
            return data;
        }

        public AttendanceCount GetDashbordAttendanceReport()
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@ForDate", DateTime.Now.Date);
            string query = @"SELECT 
                            SUM(CASE WHEN FeesStatus = 'PAID' THEN 1 ELSE 0 END) AS PresentCount,
                            SUM(CASE WHEN FeesStatus = 'UNPAID' THEN 1 ELSE 0 END) AS AbsentCount
                        FROM 
                            User_InOut_Records_KCA
                        WHERE 
                            CONVERT(DATE, ForDate) = @ForDate  ;";
            var data = vconn.QueryFirstOrDefault<AttendanceCount>(query, vParams);
            return data;
        }

        public FeesCounts GetFeesCounts()
        {
            FeesCounts feesCounts = new();
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            string Query1 = @"select Count(*) FeesPaidCount from Students_KCA where Allow = 'TRUE'";
            string Query2 = @"select Count(*) FeesPaidCount from Students_KCA where Allow = 'FALSE'";
            int paid = vconn.QueryFirstOrDefault<int>(Query1, vParams);
            int unpaid = vconn.QueryFirstOrDefault<int>(Query2, vParams);
            feesCounts.PaidCount = paid;
            feesCounts.UnPaidCount = unpaid;
            return feesCounts;
        }

        public List<AttendanceReport> GetFilteredData(string studentNo, DateTime fromDate, DateTime toDate, string feesStatus)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@UserName", studentNo);
            vParams.Add("@FromDate", fromDate.Date);
            vParams.Add("@ToDate", toDate.Date);
            vParams.Add("@FeesStatus", feesStatus);
            string Query = @"SELECT UserName, EnrollNo, ForDate, InTime, DeviceId, FeesStatus
                        FROM User_InOut_Records_KCA
                        WHERE UserName = @UserName
                        AND ForDate >= @FromDate
                        AND ForDate <= @ToDate
                        AND FeesStatus = @FeesStatus
                         ";
            var data = vconn.Query<AttendanceReport>(Query, vParams).ToList();
            return data;
        }

        public List<LoginReportKCA> GetLoginReport(DateTime date)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@Date", date.Date);
            string Query = @"SELECT * 
                        FROM Login_Report_KCA
                        WHERE CAST(LoginTime AS DATE) = @Date; ";
            var logins = vconn.Query<LoginReportKCA>(Query, vParams).ToList();
            return logins;
        }

        public Staff GetStaffDataFromDB(string? staffNo, int entriesPerPage, int pageNo)
        {
            Staff staff = new();
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@StaffNo", staffNo);
            vParams.Add("@EntryPerPage", entriesPerPage);
            vParams.Add("@PageNo", pageNo);
            vParams.Add("@StartRow", (pageNo - 1) * entriesPerPage);
            string Query = @"SELECT *
                            FROM Staff_KCA
                            WHERE (@StaffNo IS NULL OR staff_no = @StaffNo)
                            ORDER BY Id
                            OFFSET @StartRow ROWS
                            FETCH NEXT @EntryPerPage ROWS ONLY;
                         ";
            string Query1 = @"select Count(*) from Staff_KCA";
            var data = vconn.Query<Result>(Query, vParams).ToList();
            staff.Result = data;
            staff.success = true;
            staff.pageNo = pageNo;
            staff.maxEntriesinthispage = entriesPerPage;
            staff.totalentries = vconn.QueryFirstOrDefault<int>(Query1);
            return staff;
        }

        public Students GetStudentsData(string? studentNo, int entriesPerPage, int pageNo)
        {
            Students students = new();
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@StudentNo", studentNo);
            vParams.Add("@EntryPerPage", entriesPerPage);
            vParams.Add("@PageNo", pageNo);
            vParams.Add("@StartRow", (pageNo - 1) * entriesPerPage);
            string Query = @"SELECT *
                            FROM Students_KCA
                            WHERE (@StudentNo IS NULL OR student_no = @StudentNo)
                            ORDER BY Id
                            OFFSET @StartRow ROWS
                            FETCH NEXT @EntryPerPage ROWS ONLY;
                         ";
            string Query1 = @"select Count(*) from Students_KCA";
            var data = vconn.Query<Student>(Query, vParams).ToList();
            students.Result = data;
            students.success = true;
            students.pageNo = pageNo;
            students.maxEntriesinthispage = entriesPerPage;
            students.totalentries = vconn.QueryFirstOrDefault<int>(Query1);
            return students;
        }

        public List<StudentList> GetStudentsListForFilter(string? feesStatus)
        {
            string Query;
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            if (feesStatus == null || feesStatus == "")
            {
                Query = @"Select Distinct UserName, EnrollNo from User_InOut_Records_KCA ";
                var data = vconn.Query<StudentList>(Query).ToList();
                return data;
            }
            else
            {
                vParams.Add("@FeesStatus", feesStatus);
                Query = @"Select Distinct UserName, EnrollNo from User_InOut_Records_KCA where FeesStatus = @FeesStatus";
                var data = vconn.Query<StudentList>(Query, vParams).ToList();
                return data;
            }
        }


        public void InsertAttendanceEvents(EventCollection getAttendance1, string deviceId)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();

            // Add parameters for insertion based on the staff data
            vParams.Add("@RowId", getAttendance1.rows[0].id);
            vParams.Add("@ServerDateTime", getAttendance1.rows[0].server_datetime);
            vParams.Add("@Date_Time", getAttendance1.rows[0].datetime);
            vParams.Add("@Parameter", getAttendance1.rows[0].parameter);
            vParams.Add("@EventIndex", getAttendance1.rows[0].index);
            vParams.Add("@UserIdName", getAttendance1.rows[0].user_id_name);
            vParams.Add("@UserId", getAttendance1.rows[0].user_id.user_id);
            vParams.Add("@UserName", getAttendance1.rows[0].user_id.name);
            vParams.Add("@UserPhotoExists", getAttendance1.rows[0].user_id.photo_exists);
            vParams.Add("@UserGroupId", getAttendance1.rows[0].user_group_id.id);
            vParams.Add("@UserGroupName", getAttendance1.rows[0].user_group_id.name);
            vParams.Add("@DeviceId", deviceId);
            vParams.Add("@DeviceName", getAttendance1.rows[0].device_id.name);
            vParams.Add("@EventTypeId", getAttendance1.rows[0].event_type_id.code);
            vParams.Add("@EventTypeCode", getAttendance1.rows[0].event_type_id.code);
            vParams.Add("@UserUpdateByDevice", getAttendance1.rows[0].user_update_by_device);
            vParams.Add("@Hint", getAttendance1.rows[0].hint);

            // Query to insert staff data into Staff_KCA table
            string query = @"
        INSERT INTO Attendance_Events_KCA (
                    RowId,
                    ServerDateTime,
                    Date_Time,
                    Parameter,
                    EventIndex,
                    UserIdName,
                    UserId,
                    UserName,
                    UserPhotoExists,
                    UserGroupId, 
                    UserGroupName,
                    DeviceId,
                    DeviceName,
                    EventTypeId,
                    EventTypeCode,
                    UserUpdateByDevice,
                    Hint
                ) VALUES (
                    @RowId,
                    @ServerDateTime,
                    @Date_Time,
                    @Parameter,
                    @EventIndex,
                    @UserIdName,
                    @UserId,
                    @UserName,
                    @UserPhotoExists,
                    @UserGroupId,
                    @UserGroupName,
                    @DeviceId,
                    @DeviceName,
                    @EventTypeId,
                    @EventTypeCode,
                    @UserUpdateByDevice,
                    @Hint
                );";
            vconn.Execute(query, vParams);
        }

        public void InsertInOutRecord(Record record, int? cmp_ID, int? emp_ID)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();

            // Query to get Employee ID and Company ID based on Employee Code
            string Query1 = @"Select Max(IO_Tran_Id) from T0150_EMP_INOUT_RECORD";

            // Query to get Employee ID and Company ID based on Employee Code
            string Query = @"INSERT INTO [dbo].[T0150_EMP_INOUT_RECORD]
                             (
                              [IO_Tran_Id]
                             ,[Emp_ID]
                             ,[Cmp_ID]
                             ,[For_Date]
                             ,[In_Time]
                             ,[Out_Time]
                             ,[Duration]
                             ,[Reason]
                             ,[Ip_Address]
                             ,[In_Date_Time]
                             ,[Out_Date_Time]
                             ,[Skip_Count]
                             ,[Late_Calc_Not_App]
                             ,[Chk_By_Superior]
                             ,[Sup_Comment]
                             ,[Half_Full_day]
                             ,[Is_Cancel_Late_In]
                             ,[Is_Cancel_Early_Out]
                             ,[Is_Default_In]
                             ,[Is_Default_Out]
                             ,[Cmp_prp_in_flag]
                             ,[Cmp_prp_out_flag]
                             ,[is_Cmp_purpose]
                             ,[App_Date]
                             ,[Apr_Date]
                             ,[System_date]
                             ,[Other_Reason]
                             ,[ManualEntryFlag]
                             ,[StatusFlag]
                             ,[In_Admin_Time]
                             ,[Out_Admin_Time]
                             )
                             VALUES
                             (
                              @IO_Tran_Id
                             ,@Emp_ID
                             ,@Cmp_ID
                             ,@For_Date
                             ,@In_Time
                             ,@Out_Time
                             ,@Duration
                             ,NULL
                             ,'192.168.1.200'
                             ,@In_Time
                             ,@Out_Time
                             ,NULL
                             ,0
                             ,0
                             ,NULL
                             ,NULL
                             ,0
                             ,0
                             ,0
                             ,0
                             ,0
                             ,0
                             ,0
                             ,NULL
                             ,NULL
                             ,@System_Date
                             ,NULL
                             ,'New'
                             ,NULL
                             ,'A'
                             ,'A'
                             )";
            vParams.Add("@IO_Tran_Id", Convert.ToInt32(vconn.QueryFirstOrDefault<int>(Query1)) + 1);
            vParams.Add("@Emp_ID", emp_ID);
            vParams.Add("@Cmp_ID", cmp_ID);
            vParams.Add("@For_Date", record.datetime);
            vParams.Add("@In_Time", record.inTime);
            vParams.Add("@Out_Time", record.outTime);
            TimeSpan duration = (TimeSpan)(Convert.ToDateTime(record.outTime) - Convert.ToDateTime(record.inTime));
            vParams.Add("@Duration", $"{(int)duration.TotalHours:D2}:{duration.Minutes:D2}");

            vParams.Add("@System_Date", DateTime.Now);

            vconn.Execute(Query, vParams);

        }

        public void InsertStaffRecord(Result staff)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();

            // Add parameters for insertion based on the staff data
            vParams.Add("@sn", staff.sn);
            vParams.Add("@staff_no", staff.staff_no);
            vParams.Add("@first_name", staff.first_name);
            vParams.Add("@middle_name", staff.middle_name);
            vParams.Add("@last_name", staff.last_name);
            vParams.Add("@campus", staff.campus);
            vParams.Add("@date_joined", staff.date_joined); // Ensure the date is in the correct format
            vParams.Add("@department", staff.department);
            vParams.Add("@job_title", staff.job_title);
            vParams.Add("@Gender", staff.Gender);
            vParams.Add("@Allow", staff.Allow);

            // Query to insert staff data into Staff_KCA table
            string query = @"
        INSERT INTO Staff_KCA (sn, staff_no, first_name, middle_name, last_name, campus, date_joined, department, job_title, Gender, Allow) 
        VALUES (@sn, @staff_no, @first_name, @middle_name, @last_name, @campus, @date_joined, @department, @job_title, @Gender, @Allow);";

            // Execute the query with the provided parameters
            vconn.Execute(query, vParams);
        }

        public void InsertStudentRecords(Student student)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();

            // Add parameters for insertion based on the staff data
            vParams.Add("@sn", student.sn);
            vParams.Add("@student_no", student.student_no);
            vParams.Add("@name", student.name);
            vParams.Add("@campus", student.campus);
            vParams.Add("@date_registered", student.date_registered);
            vParams.Add("@intake_period", student.intake_period);
            vParams.Add("@dob", student.dob); // Ensure the date is in the correct format
            vParams.Add("@Gender", student.Gender);
            vParams.Add("@Email", student.Email);
            vParams.Add("@Password", student.Password);
            vParams.Add("@Allow", student.Allow);
            vParams.Add("@Bal", student.Bal);

            // Query to insert staff data into Staff_KCA table
            string query = @"
                          INSERT INTO Students_KCA 
                          (sn, student_no, name, campus, date_registered, intake_period, dob, Gender, Email, Password, Allow, Bal, CreatedDate) 
                          VALUES 
                          (@sn, @student_no, @name, @campus, @date_registered, @intake_period, @dob, @Gender, @Email, @Password, @Allow, @Bal, GetDate()) ";
            // Execute the query with the provided parameters
            vconn.Execute(query, vParams);
        }

        public OtpVerify OtpVerification(string username, int otpCode)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@UserName", username);
            vParams.Add("@Otp", Convert.ToString(otpCode));
            string Query = @"SELECT TOP 1 *
                        FROM KCA_OTP_VERIFY
                        WHERE UserName = @UserName AND OTP = @Otp
                        ORDER BY CreatedDate DESC;";
            var verify = vconn.QueryFirstOrDefault<OtpVerify>(Query, vParams);
            if (verify != null)
            {
                return verify;
            }
            return null;
        }

        public void PostDeviceConfig(DeviceConfig deviceConfig)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@DeviceId", deviceConfig.DeviceId);
            vParams.Add("@Location", deviceConfig.Location);
            vParams.Add("@CampusName", "Campus");
            vParams.Add("@Type", deviceConfig.Type);
            vParams.Add("@CreatedDate", DateTime.UtcNow);
            string query = @"INSERT INTO Device_Configuration_KCA (DeviceId, Location, CampusName, Type, CreatedDate)
                    VALUES (@DeviceId, @Location, @CampusName, @Type, @CreatedDate);
                    ";

            vconn.Execute(query, vParams);

        }

        public void PostInOutPunch(PunchModel punchModel)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            // Employee exists, proceed with the insert query
            var insertParams = new DynamicParameters();
            insertParams.Add("@UserName", punchModel.UserName);
            insertParams.Add("@EnrollNo", punchModel.EnrollNo);
            insertParams.Add("@LocationType", punchModel.LocationType);
            insertParams.Add("@ForDate", punchModel.ForDate);
            insertParams.Add("@InTime", punchModel.InTime);
            insertParams.Add("@OutTime", punchModel.OutTime);
            insertParams.Add("@Duration", punchModel.Duration);
            insertParams.Add("@IpAddress", punchModel.IpAddress);
            insertParams.Add("@DeviceId", punchModel.DeviceId);
            insertParams.Add("@FeesStatus", punchModel.FeesStatus);
            insertParams.Add("@CreatedDate", DateTime.UtcNow);

            string insertQuery = @"INSERT INTO User_InOut_Records_KCA
                                   (
                                       UserName,
                                       EnrollNo,
                                       LocationType,
                                       ForDate,
                                       InTime,
                                       OutTime,
                                       Duration,
                                       IpAddress,
                                       DeviceId,
                                       FeesStatus,
                                       CreatedDate
                                   )
                                   VALUES
                                   (
                                       @UserName,
                                       @EnrollNo,
                                       @LocationType,
                                       @ForDate,
                                       @InTime,
                                       @OutTime,
                                       @Duration,
                                       @IpAddress,
                                       @DeviceId,
                                       @FeesStatus,
                                       @CreatedDate
                                   );
                                   ";

            // Execute the insert query
            vconn.Execute(insertQuery, insertParams);

            // Return true if insertion is successful
        }

        public void PostLoginReport(LoginReportKCA loginReportKCA)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            // Employee exists, proceed with the insert query
            var insertParams = new DynamicParameters();
            insertParams.Add("@UserName", loginReportKCA.UserName);
            insertParams.Add("@Type", loginReportKCA.Type);
            insertParams.Add("@LoginTime", loginReportKCA.LoginTime);

            string insertQuery = @"Insert into Login_Report_KCA (UserName,Type,LoginTime) values (@UserName,@Type,@LoginTime);";

            // Execute the insert query
            vconn.Execute(insertQuery, insertParams);

        }

        public void UpdateDeviceConfig(DeviceConfig deviceConfig)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            var vParams = new DynamicParameters();
            vParams.Add("@DeviceId", deviceConfig.DeviceId);
            vParams.Add("@Location", deviceConfig.Location);
            vParams.Add("@CampusName", "Campus");
            vParams.Add("@Type", deviceConfig.Type);
            vParams.Add("@CreatedDate", DateTime.UtcNow);
            string query = @"Update Device_Configuration_KCA set Location = @Location, Type = @Type , CampusName = @CampusName where DeviceId = @DeviceId;
                    ";
            vconn.Execute(query, vParams);
        }

        public void UpdateInOutPunch(PunchModel punchModel)
        {
            using var vconn = GetOpenConnection(); // Open a connection to the database
            // Employee exists, proceed with the insert query
            var insertParams = new DynamicParameters();
            insertParams.Add("@UserName", punchModel.UserName);
            insertParams.Add("@LocationType", punchModel.LocationType);
            insertParams.Add("@ForDate", punchModel.ForDate);
            insertParams.Add("@OutTime", punchModel.OutTime);

            string insertQuery = @"Update User_InOut_Records_KCA set OutTime = @OutTime where ForDate = @ForDate and UserName = @UserName and LocationType = @LocationType
                                   ";

            // Execute the insert query
            vconn.Execute(insertQuery, insertParams);
        }
    }

}