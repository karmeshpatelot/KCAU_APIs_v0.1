using MOBILEAPI2024.DTO.RequestDTO.Account;
using MOBILEAPI2024.DTO.ResponseDTO.Account;

namespace MOBILEAPI2024.BLL.Services.IServices
{
    public interface IAccountService
    {
        void AddUserInformation(UserInformation userInformation);
        LoginResponseDTO AuthenticateUser(LoginDTO loginDTO);
        string ClientVerification(ClientLogin loginDTO);
        string GenerateOtp();
        string GenerateToken(LoginData? loginData);
        ForgotPasswordInfo GetUserByUserName(string userName);
        string OtpVerification(string userName,int otp);
        string RemoveLoginToken(string loginToken);
        string ResetPassword(ForgotPasswordInfo user, ResetPasswordDTO resetPasswordDTO);
        string UpdateToken(LoginResponseDTO authenticateUser,string password);
    }
}
