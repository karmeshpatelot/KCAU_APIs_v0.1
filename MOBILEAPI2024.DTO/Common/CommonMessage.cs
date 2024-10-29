namespace MOBILEAPI2024.DTO.Common
{
    public class CommonMessage
    {
        /// <summary>
        ///  Error
        /// </summary>
        public static string InValidUser = "Invalid user passed.";
        public static string SomethingWrong = "Something went wrong.";
        public static string TokenExpired = "User token expired.";
        public static string InValidToken= "Invalid token passed.";
        public static string NoDataFound = "No data found.";
        public static string UserNameAndPasswordMandatory = "Username and password are mandatory fields.";
        public static string NoUserNamePassed = "Username not passed.";

        /// <summary>
        ///  Success
        /// </summary>
        public static string LoginUser = "User login successfully.";
        public static string Success = "Success";
        public static string AttendanceAlreadyExixts = "Attendance already exists.";
        public static string EmailSent = "Successfully OTP send to email.";
        public static string OTPVerified = "OTP verified successfully.";
        public static string PasswordReset = "Password reset successfully.";
        public static string Logout = "User logout successfully.";
        public static string ClockInAdded = "ClockIn added successfully.";
        public static string ClockOutAdded = "ClockOut added successfully.";


    }
}
