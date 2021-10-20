using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AargonTools.Controllers.prod_old
{
    [Route("api/prod_old/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HrmController : ControllerBase
    {
        private readonly IGetHrm _getHrmData;
        public HrmController(IGetHrm getHrm)
        {
            _getHrmData = getHrm;
        }

        /// <summary>
        ///  Returns time log for an employee on a specific date(prod_old environment).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to check the time log for an employee on a specific date by passing the parametrize emp id ann date. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/prod_old/Hrm/GetEmployeeTimeLog/65&amp;2020-02-22 
        /// and please don't forget about valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="employeeId"> Enter Employee ID</param>
        ///<param name="date"> Enter Date format(YYYY-MM-DD)</param>
        /// 

        [ProducesResponseType(typeof(GetEmployeeTimeLogResponse), 200)]

        [HttpGet("GetEmployeeTimeLog/{employeeId}&{date}")]

        public async Task<IActionResult> GetEmployeeTimeLog(int employeeId, DateTime date)
        {
            Serilog.Log.Information("prod_old GetEmployeeTimeLog => GET");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _getHrmData.GetEmployeeTimeLog(employeeId,date,"PO");

                    return Ok(data);

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                throw;
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }






    }
}
