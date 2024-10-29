using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
using MOBILEAPI2024.BLL.Services.IServices;
using MOBILEAPI2024.DTO.Common;
using MOBILEAPI2024.DTO.RequestDTO.Employee;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace MOBILEAPI2024.API.Controllers
{
    [Route("api/v1")]
    [Authorize]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        //[HttpGet]
        //[Route(APIUrls.AllEmployeesList)]
        //public IActionResult EmployeeListClient()
        //{
        //    Response response = new Response();
        //    try
        //    {
        //        var authorization = HttpContext.Request.Headers[HeaderNames.Authorization];
        //        if (AuthenticationHeaderValue.TryParse(authorization, out var headerValue))
        //        {
        //            var jToken = headerValue.Parameter;
        //            var handler = new JwtSecurityTokenHandler();

        //            var jsonToken = handler.ReadToken(jToken) as JwtSecurityToken;
        //            if (jsonToken != null)
        //            {
        //                var empId = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "Emp_ID")?.Value;
        //                var cmpId = jsonToken.Claims.FirstOrDefault(claim => claim.Type == "Cmp_ID")?.Value;
        //                if (!string.IsNullOrEmpty(empId) && !string.IsNullOrEmpty(cmpId))
        //                {
        //                    EmployeeListRequest employeeListRequest = new();
        //                    employeeListRequest.EmpId = Convert.ToInt32(empId);
        //                    employeeListRequest.CmpId = 1;

        //                    var employeeResponse = _employeeService.EmployeeList(employeeListRequest);
        //                    if (employeeResponse != null)
        //                    {
        //                        response.code = StatusCodes.Status200OK;
        //                        response.status = true;
        //                        response.message = CommonMessage.Success;
        //                        response.data = employeeResponse;
        //                        return Ok(response);
        //                    }
        //                    response.code = StatusCodes.Status404NotFound;
        //                    response.status = false;
        //                    response.message = CommonMessage.NoDataFound;
        //                    return StatusCode(StatusCodes.Status404NotFound, response);
        //                }
        //            }
        //            else
        //            {
        //                // Handle the case where the token cannot be read as a JWT token
        //                response.code = StatusCodes.Status401Unauthorized;
        //                response.status = false;
        //                response.message = CommonMessage.TokenExpired;
        //                return StatusCode(StatusCodes.Status401Unauthorized, response);
        //            }
        //        }
        //        // Handle the case where the token cannot be read as a JWT token
        //        response.code = StatusCodes.Status401Unauthorized;
        //        response.status = false;
        //        response.message = CommonMessage.TokenExpired;
        //        return StatusCode(StatusCodes.Status401Unauthorized, response);

        //    }
        //    catch (Exception e)
        //    {
        //        response.code = StatusCodes.Status500InternalServerError;
        //        response.status = false;
        //        response.message = CommonMessage.SomethingWrong + " " + e.Message;
        //        return StatusCode(StatusCodes.Status500InternalServerError, response);
        //    }
        //}
    }
}
