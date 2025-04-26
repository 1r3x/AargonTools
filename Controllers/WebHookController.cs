using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System;
using AargonTools.Models.WebHook;
using Microsoft.AspNetCore.Http.Extensions;
using AargonTools.Interfaces;
using AargonTools.Interfaces.WebHook;
using AargonTools.Interfaces.Email;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class WebHookController : ControllerBase
    {
        private readonly IComlinkv2 _Comlinkv2;
        private readonly IEmailService _emailService;

        public WebHookController(IComlinkv2 Comlinkv2, IEmailService emailService)
        {
            _Comlinkv2 = Comlinkv2;
            _emailService = emailService;
        }


        [HttpPost]
        [Route("comlinkv2")]
        public async Task<IActionResult> Comlinkv2([FromBody] TextRequestModel textRequestModel)
        {
            Serilog.Log.Information("Entering Comlinkv2 => POST with : {@TextRequestModel}", textRequestModel);
            try
            {
                if (ModelState.IsValid)
                {
                    var clientIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                    Serilog.Log.Information("Comlinkv2 => from IP : {ClientIp}", clientIp);

                    var requestUrl = Request.GetDisplayUrl();
                    Serilog.Log.Information("Comlinkv2 => URL : {RequestUrl}", requestUrl);

                    var result = await _Comlinkv2.ProcessInboundSms(textRequestModel, clientIp, requestUrl);

                    if (!result.Success)
                    {
                        Serilog.Log.Warning("Comlinkv2 => Processing failed with message: {Message}", result.Message);
                        return BadRequest(result.Message);
                    }

                    Serilog.Log.Information("Exiting Comlinkv2 with success: {Message}", result.Message);
                    return Ok(result.Message);
                }
                else
                {
                    Serilog.Log.Warning("Comlinkv2 => Invalid model state: {@ModelState}", ModelState);
                    return BadRequest("Invalid model state");
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "Comlinkv2 => Exception occurred");
                return new JsonResult("Something went wrong") { StatusCode = 500 };
            }
        }



    }
}
