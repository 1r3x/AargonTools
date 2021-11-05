using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AargonTools.Controllers.prod_old
{
    [Route("api/prod_old/[controller]")]
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
        ///  Can Process a credit card payment(prod_old).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to Process a credit card payment of any debtor account by passing the parameters. You need a valid token
        /// for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/CreditCards/SetProcessCcPayments
        /// (pass JSON body like the request example)
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>

        /// 
        [ProducesResponseType(typeof(SetProcessCCResponse), 200)]
        [HttpPost("SetProcessCcPayments")]
        public async Task<IActionResult> SetProcessCcPayments([FromBody] ProcessCcPaymentRequestModel requestCcPayment)
        {
            Serilog.Log.Information("prod_old SetProcessCcPayments => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _processCcPayment.ProcessCcPayment(requestCcPayment, "PO");

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
        ///  Can Schedule Post Data (prod_old).
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can Schedule Post Data of any debtor account by passing the parameters. You need a valid token
        /// for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/CreditCards/SchedulePostData/0001-000001&amp;21-12-2012&amp;10&amp;4929000000006&amp;7&amp;12&amp;2020
        /// (pass parameters separated by '&amp;')
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
      
        [ProducesResponseType(typeof(SchedulePostDateResponse), 200)]

        [HttpPost("SchedulePostData")]
        public async Task<IActionResult> SchedulePostData([FromBody] SchedulePostDateRequest request)
        {
            Serilog.Log.Information("prod_old SchedulePostData => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _processCcPayment.SchedulePostDataV2(request, "PO");

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
        ///  Can set CC Payments.(prod_old Environment)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can set cc payment by passing required parameters. You need a valid token
        /// for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/prod_old/CreditCards/SetCcPayments
        /// (pass JSON body like the request example)
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>

        ///

        [ProducesResponseType(typeof(SetCcPaymnetResponse), 200)]
        [HttpPost("SetCcPayments")]
        public async Task<IActionResult> SetCcPayments([FromBody] CcPaymnetRequestModel requestCcPayment)
        {
            Serilog.Log.Information(" prod_old SetProcessCcPayments => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setCcPayment.SetCCPayment(requestCcPayment, "PO");

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
