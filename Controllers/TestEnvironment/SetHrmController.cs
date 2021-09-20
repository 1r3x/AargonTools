using System;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AargonTools.Controllers.TestEnvironment
{
    [Route("api/Test/[controller]")]
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



        [HttpPost("SetEmployeeTimeLog/{employeeId}&{stationName}&{dateTime}&{reasons}")]
        public async Task<IActionResult> SetEmployeeTimeLog(int employeeId, string stationName, DateTime dateTime, string reasons)
        {
            Serilog.Log.Information("Test SetEmployeeTimeLog => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setHrm.SetEmployeeTimeLogEntry(employeeId, stationName,dateTime,reasons, "T");

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
