using Dapper;
using Microsoft.Extensions.Options;
using MOBILEAPI2024.DAL.Entities;
using MOBILEAPI2024.DAL.Repositories.IRepositories;
using MOBILEAPI2024.DTO.Common;
using MOBILEAPI2024.DTO.RequestDTO.Account;
using MOBILEAPI2024.DTO.ResponseDTO.Account;
using System.Data;
using static Dapper.SqlMapper;

namespace MOBILEAPI2024.DAL.Repositories
{
    public class AccountRepository : SqlDbRepository<ActiveInActiveUser>, IAccountRepository
    {
        AppEncrypt _appEncrypt;
        public AccountRepository(string connectionString) : base(connectionString)
        {
            _appEncrypt = new AppEncrypt();
        }

        public void AddOtp(ForgotPasswordInfo user, string otp)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@Otp_ID", Convert.ToInt32(0));
            vParams.Add("@Otp_TypeID", Convert.ToInt32(1));
            vParams.Add("@Emp_ID", user.Emp_ID);
            vParams.Add("@Cmp_ID", user.Cmp_ID);
            vParams.Add("@Otp_Code", Convert.ToInt32(otp));
            vParams.Add("@Email", user.Work_Email);
            vParams.Add("@MobileNo", user.Mobile_No);
            vParams.Add("@IsVerified", Convert.ToInt32(0));
            vParams.Add("@Tran_Type", "I");
            vconn.Execute("P0011_OTP_TRANSACTIONS", vParams, commandType: CommandType.StoredProcedure);
        }

        public void AddUserInformation(UserInformation userInformation)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@IPAddress", userInformation.IPAddress);
            vParams.Add("@Country", userInformation.Country);
            vParams.Add("@Region", userInformation.Region);
            vParams.Add("@City", userInformation.City);
            vParams.Add("@ConnectionType", userInformation.ConnectionType);
            vParams.Add("@Browser", userInformation.Browser);
            vParams.Add("@OperatingSystem", userInformation.OperatingSystem);
            vParams.Add("@DeviceType", userInformation.DeviceType);
            vParams.Add("@WeatherInfo", userInformation.WeatherInfo);
            vParams.Add("@Timezone", userInformation.Timezone);
            vParams.Add("@Language", userInformation.Language);
            var user = vconn.Execute("SP_Store_UserInformation", vParams, commandType: CommandType.StoredProcedure);
        }

        public ClientVerification ClientVerification(ClientLogin loginDTO, string ClientName)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@ClientName", ClientName);
            vParams.Add("@UserName", Convert.ToString(loginDTO.UserName));
            vParams.Add("@Password", _appEncrypt.EncryptTripleDES(Convert.ToString(loginDTO.Password)));
            
            var user = vconn.QueryFirstOrDefault<ClientVerification>("Sp_Client_Verification", vParams, commandType: CommandType.StoredProcedure);
            return user;
        }

        public ForgotPasswordInfo GetUserByUserName(string userName)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@UserName", Convert.ToString(userName));
            vParams.Add("@Password", _appEncrypt.EncryptTripleDES(Convert.ToString("")));
            vParams.Add("@DeviceID", Convert.ToString(""));
            vParams.Add("@NewPassword", Convert.ToString(""));
            vParams.Add("@Login_ID", Convert.ToInt32("0"));
            vParams.Add("@Emp_ID", Convert.ToInt32("0"));
            vParams.Add("@Cmp_ID", Convert.ToInt32("0"));
            vParams.Add("@Type", Convert.ToString("F"));
            vParams.Add("@Result", Convert.ToString(""));
            var user = vconn.QueryFirstOrDefault<ForgotPasswordInfo>("SP_Mobile_HRMS_WebService_Login", vParams, commandType: CommandType.StoredProcedure);
            return user;
        }

        public LoginResponseDTO LoginCheck(LoginDTO loginDTO)
        {
            LoginResponseDTO loginResponseDTO = new LoginResponseDTO();
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@UserName", Convert.ToString(loginDTO.UserName));
            vParams.Add("@Password", _appEncrypt.EncryptTripleDES(Convert.ToString(loginDTO.Password)));
            vParams.Add("@DeviceID", Convert.ToString(loginDTO.DeviceID));
            vParams.Add("@NewPassword", Convert.ToString(""));
            vParams.Add("@Login_ID", Convert.ToInt32("0"));
            vParams.Add("@Emp_ID", Convert.ToInt32("0"));
            vParams.Add("@Cmp_ID", Convert.ToInt32("0"));
            vParams.Add("@Type", Convert.ToString("L"));
            vParams.Add("@Result", Convert.ToString(""));

            var multiResult = vconn.QueryMultiple("SP_Mobile_HRMS_WebService_Login", vParams, commandType: CommandType.StoredProcedure);

            loginResponseDTO.LoginData = multiResult.Read<LoginData>().FirstOrDefault();
            loginResponseDTO.Details = multiResult.Read<Detail>().ToList();
            return loginResponseDTO;

        }

        public string OtpVerification(ForgotPasswordInfo user, int otp)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@Emp_ID", user.Emp_ID);
            vParams.Add("@Cmp_ID", user.Cmp_ID);
            vParams.Add("@Otp_Code", otp);
            vParams.Add("@Msg", "");
            vParams.Add("@User_Type", "");
            vParams.Add("@Email", user.Work_Email);

            string verified = vconn.QueryFirst<string>("SP_OTP_TRANSACTIONS_VALIDATE_Mobile", vParams, commandType: CommandType.StoredProcedure);
            return verified;
        }

        public string RemoveLoginToken(string loginToken)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@UserName", "");
            vParams.Add("@Password", "");
            vParams.Add("@DeviceID", "");
            vParams.Add("@NewPassword", Convert.ToString(""));
            vParams.Add("@Login_ID", Convert.ToInt32("0"));
            vParams.Add("@Emp_ID", Convert.ToInt32("0"));
            vParams.Add("@Cmp_ID", Convert.ToInt32("0"));
            vParams.Add("@Type", Convert.ToString("D"));
            vParams.Add("@token", loginToken);
            vParams.Add("@Result", Convert.ToString(""));
            string removeToken = vconn.QueryFirstOrDefault<string>("SP_Mobile_HRMS_WebAPI_Login", vParams, commandType: CommandType.StoredProcedure);
            return removeToken;
        }

        public void ResetPassword(ForgotPasswordInfo user, ResetPasswordDTO resetPasswordDTO)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@UserName", Convert.ToString(resetPasswordDTO.UserName));
            vParams.Add("@Password", "");
            vParams.Add("@IMEINo", Convert.ToString(""));
            vParams.Add("@DeviceID", Convert.ToString(""));
            vParams.Add("@NewPassword", _appEncrypt.EncryptTripleDES(Convert.ToString(resetPasswordDTO.NewPassword)));
            vParams.Add("@Login_ID", Convert.ToInt32(user.Login_ID));
            vParams.Add("@Emp_ID", Convert.ToInt32(user.Emp_ID));
            vParams.Add("@Cmp_ID", Convert.ToInt32(user.Cmp_ID));
            vParams.Add("@Type", Convert.ToString("R"));
            vParams.Add("@Result", Convert.ToString(""));

            vconn.QueryFirstOrDefault("SP_Mobile_HRMS_WebService_Login", vParams, commandType: CommandType.StoredProcedure);
            //return resetPassword;
        }

        public string UpdateToken(LoginResponseDTO authenticateUser, string Password)
        {
            using var vconn = GetOpenConnection();
            var vParams = new DynamicParameters();
            vParams.Add("@UserName", Convert.ToString(authenticateUser.LoginData.Login_Name));
            vParams.Add("@Password", _appEncrypt.EncryptTripleDES(Convert.ToString(Password)));
            vParams.Add("@NewPassword", Convert.ToString(""));
            vParams.Add("@Login_ID", Convert.ToInt32("0"));
            vParams.Add("@Emp_ID", Convert.ToInt32("0"));
            vParams.Add("@Cmp_ID", Convert.ToInt32(authenticateUser.LoginData.Cmp_ID));
            vParams.Add("@Type", Convert.ToString("U"));
            vParams.Add("@token", authenticateUser.Token);
            vParams.Add("@Result", Convert.ToString(""));

            string updateToken = vconn.QueryFirst<string>("SP_Mobile_HRMS_WebAPI_Login", vParams, commandType: CommandType.StoredProcedure);

            return updateToken;
        }
    }
}
