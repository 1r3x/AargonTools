using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.Models;
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
        ///  Can Process a credit card payment.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to Process a credit card payment of any debtor account by passing the parameters. You need a valid token
        /// for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/CreditCards/SetProcessCcPayments/0001-000001&amp;4929000000006&amp;1221&amp;124&amp;1&amp;9.99
        /// (pass parameters separated by '&amp;')
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcc"> Enter Debtor Account</param>
        ///<param name="ccNumber"> Enter CC number</param>
        ///<param name="expiredDate"> Enter expiration date format: 1222 [12-2022]</param>
        ///<param name="cvv"> Enter CVV</param>
        ///<param name="numberOfPayments"> Enter number of payments</param>
        ///<param name="amount"> Enter amount</param>
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
        ///  Can Schedule Post Data.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can Schedule Post Data of any debtor account by passing the parameters. You need a valid token
        /// for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/CreditCards/SchedulePostData/0001-000001&amp;21-12-2012&amp;10&amp;4929000000006&amp;7&amp;12&amp;2020
        /// (pass parameters separated by '&amp;')
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        ///<param name="debtorAcct"> Enter Debtor Account</param>
        ///<param name="postDate"> Enter post date</param>
        ///<param name="amount"> Enter amount</param>
        ///<param name="cardNumber"> Enter card number </param>
        ///<param name="numberOfPayments"> Enter number of payments</param>
        ///<param name="expMonth"> Enter month ex: 3</param>
        ///<param name="expYear"> Enter year ex: 12</param>
        ///
        [ProducesResponseType(typeof(SchedulePostDateResponse), 200)]

        [HttpPost("SchedulePostData/{debtorAcct}&{postDate}&{amount}&{cardNumber}&{numberOfPayments}&{expMonth}&{expYear}")]
        public async Task<IActionResult> SchedulePostData(string debtorAcct, DateTime postDate, decimal amount, string cardNumber, int numberOfPayments,
            string expMonth, string expYear)
        {
            Serilog.Log.Information("SchedulePostData => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _processCcPayment.SchedulePostData(debtorAcct, postDate, amount, cardNumber, numberOfPayments, expMonth, expYear, "P");

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
        ///  Can set CC Payments.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can set cc payment by passing required parameters. You need a valid token
        /// for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/CreditCards/SetCcPayments/0001-000001&amp;AARGON AGENCY&amp;12&amp;12&amp;2021-10-8&amp;APPROVED&amp;1234&amp;124&amp;Y
        /// (pass parameters separated by '&amp;')
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
