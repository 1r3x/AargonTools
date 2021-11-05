using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SetHrmController : ControllerBase
    {
        private readonly ISetEmployeeTimeLogEntry _setHrm;
        public SetHrmController(ISetEmployeeTimeLogEntry setHrm)
        {
            _setHrm = setHrm;
        }


        /// <summary>
        ///  Can set time log for an employee on a specific date-time .
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to set the time log for an employee on a specific date by passing the parameters. You need a valid token
        /// for this endpoint . You can pass the param with API client like  https://g14.aargontools.com/api/SetHrm/SetEmployeeTimeLog/65&amp;Station07&amp;2020-02-22 07:55:23&amp;Reasons 
        /// and please don't forget about valid token.
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="employeeId"> Enter Employee ID</param>
        ///<param name="stationName"> Enter Station Name</param>
        ///<param name="dateTime"> Enter Date format(YYYY-MM-DD HH:MM:SS)</param>
        /// ///<param name="reasons"> Enter Reasons</param>
        /// 

        [ProducesResponseType(typeof(SetEmployeeTimeLogResponse), 200)]

        [HttpPost("SetEmployeeTimeLog/{employeeId}&{stationName}&{dateTime}&{reasons}")]
        public async Task<IActionResult> SetEmployeeTimeLog(int employeeId, string stationName, DateTime dateTime, string reasons)
        {
            Serilog.Log.Information("SetEmployeeTimeLog => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setHrm.SetEmployeeTimeLogEntry(employeeId, stationName,dateTime,reasons, "P");

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
