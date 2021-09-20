using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("GetEmployeeTimeLog/{employeeId}&{date}")]
        public async Task<IActionResult> GetEmployeeTimeLog(int employeeId, DateTime date)
        {
            Serilog.Log.Information("GetEmployeeTimeLog => GET");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _getHrmData.GetEmployeeTimeLog(employeeId,date,"P");

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
