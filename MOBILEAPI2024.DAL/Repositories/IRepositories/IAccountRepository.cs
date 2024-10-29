using MOBILEAPI2024.DAL.Entities;
using MOBILEAPI2024.DTO.RequestDTO.Account;
using MOBILEAPI2024.DTO.ResponseDTO.Account;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOBILEAPI2024.DAL.Repositories.IRepositories
{
    public interface IAccountRepository : IGenericRepository<ActiveInActiveUser>
    {
        void AddOtp(ForgotPasswordInfo user, string otp);
        void AddUserInformation(UserInformation userInformation);
        ClientVerification ClientVerification(ClientLogin loginDTO,string ClientName);
        ForgotPasswordInfo GetUserByUserName(string userName);
        LoginResponseDTO LoginCheck(LoginDTO loginDTO);
        string OtpVerification(ForgotPasswordInfo user, int otp);
        string RemoveLoginToken(string loginToken);
        void ResetPassword(ForgotPasswordInfo user, ResetPasswordDTO resetPasswordDTO);
        string UpdateToken(LoginResponseDTO authenticateUser,string Password);
    }
}
