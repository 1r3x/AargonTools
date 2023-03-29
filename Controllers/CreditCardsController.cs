using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace AargonTools.Controllers
{
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CreditCardsController : ControllerBase
    {
        private readonly IProcessCcPayment _processCcPayment;
        private readonly ISetCCPayment _setCcPayment;


        public CreditCardsController(IProcessCcPayment processCcPayment, ISetCCPayment setCcPayment)
        {
            _processCcPayment = processCcPayment;
            _setCcPayment = setCcPayment;
        }

        /// <summary>
        ///  Can Process a credit card payment.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Process a credit card payment of any debtor account by passing the parameters. A valid token is required for sending the data. 
        /// for this endpoint .
        /// You can pass the parameter with API client like https://g14.aargontools.com/api/CreditCards/SetProcessCcPayments 
        /// (pass JSON body like the request example)
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>

        /// 
        [ProducesResponseType(typeof(SetProcessCCResponse), 200)]
        [HttpPost("SetProcessCcPayments")]
        public async Task<IActionResult> SetProcessCcPayments([FromBody] ProcessCcPaymentRequestModel requestCcPayment)
        {
            Serilog.Log.Information("SetProcessCcPayments => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _processCcPayment.ProcessCcPayment(requestCcPayment, "P");

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

        /// <summary>
        ///  Can Schedule Post Data.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Schedule Post Data of a debtor account by passing the parameters. A valid token is required for sending the data.
        /// 
        /// Pass the parameter with API client like https://g14.aargontools.com/api/CreditCards/SchedulePostData
        /// (pass JSON body like the request example)
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///
        [ProducesResponseType(typeof(SchedulePostDateResponse), 200)]

        [HttpPost("SchedulePostData")]
        public async Task<IActionResult> SchedulePostData([FromBody]SchedulePostDateRequest request)
        {
            Serilog.Log.Information("SchedulePostData => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _processCcPayment.SchedulePostDataV2(request, "P");

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

        /// <summary>
        ///  Can set CC Payments.(Prod.)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// Set cc payment by passing required parameters. A valid token is required for sending the data.
        ///Pass the parameter with API client like https://g14.aargontools.com/api/CreditCards/SetCcPayments
        /// (pass JSON body like the request example)
        /// 
        /// 
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///

        [ProducesResponseType(typeof(SetCcPaymnetResponse), 200)]
        [HttpPost("SetCcPayments")]
        public async Task<IActionResult> SetCcPayments([FromBody]CcPaymnetRequestModel requestCcPayment)
        {
            Serilog.Log.Information("SetProcessCcPayments => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setCcPayment.SetCCPayment(requestCcPayment, "P");

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
