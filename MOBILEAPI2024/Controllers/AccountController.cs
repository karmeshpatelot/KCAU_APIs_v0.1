using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MOBILEAPI2024.BLL.Services.IServices;

namespace MOBILEAPI2024.API.Controllers
{
    [Authorize]
    [Route("api/v1")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly HttpClient _httpClient;
        public AccountController(IAccountService accountService, HttpClient httpClient)
        {
            _accountService = accountService;
            _httpClient = httpClient;
        }

        //[AllowAnonymous]
        //[HttpPost]
        //[Route(APIUrls.GenerateToken)]
        //public async Task<IActionResult> SignIn(ClientLogin loginDTO)
        //{
        //    Response response = new Response();
        //    try
        //    {
        //        var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
        //        var userAgent = Request.Headers["User-Agent"].ToString();
        //        var uaParser = Parser.GetDefault();
        //        ClientInfo clientInfo = uaParser.Parse(userAgent);

        //        var geolocationResponse = await _httpClient.GetAsync($"https://freegeoip.app/json/{ipAddress}");
        //        var geolocationContent = await geolocationResponse.Content.ReadAsStringAsync();
        //        var geolocationInfo = JsonSerializer.Deserialize<LocationInfo>(geolocationContent);

        //        UserInformation userInformation = new UserInformation()
        //        {
        //            IPAddress = ipAddress,
        //            Country = geolocationInfo.CountryName,
        //            Region = geolocationInfo.RegionName,
        //            City = geolocationInfo.City,
        //            ConnectionType = geolocationInfo.ConnectionType,
        //            Browser = clientInfo.UserAgent.Family,
        //            OperatingSystem = clientInfo.OS.Family,
        //            DeviceType = clientInfo.Device.Family,
        //            WeatherInfo = "",
        //            Timezone = "",
        //            Language = ""
        //        };

        //        TryValidateModel(loginDTO);
        //        if (ModelState.IsValid)
        //        {
        //            string verify = _accountService.ClientVerification(loginDTO); 
        //            if (verify == "Success")
        //            {
        //                _accountService.AddUserInformation(userInformation);
        //                LoginData loginData = new LoginData();
        //                loginData.Emp_ID = 0;
        //                loginData.Cmp_ID = 187;
        //                loginData.DEPT_Id = 0;
        //                loginData.Login_ID = 0;
        //                loginData.Alpha_Emp_Code = "aaaa";
        //                loginData.Emp_Full_Name = "cera";
        //                loginData.Dept_Name = "aaaa";
        //                loginData.Desig_Name = "aaaa";
        //                string token = _accountService.GenerateToken(loginData);
        //                response.code = StatusCodes.Status200OK;
        //                response.status = true;
        //                response.message = CommonMessage.LoginUser;
        //                response.data = token;
        //                return Ok(response);
        //            }
        //        }
        //        response.code = StatusCodes.Status400BadRequest;
        //        response.status = false;
        //        response.message = CommonMessage.InValidUser;
        //        return BadRequest(response);
        //    }
        //    catch (Exception ex)
        //    {
        //        response.code = StatusCodes.Status500InternalServerError;
        //        response.status = false;
        //        response.message = CommonMessage.SomethingWrong + ex.Message;
        //        return StatusCode(StatusCodes.Status500InternalServerError, response);
        //    }
        //}
    }
}
