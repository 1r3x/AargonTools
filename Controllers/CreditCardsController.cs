﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
    public class CreditCardsController : ControllerBase
    {
        private readonly IProcessCcPayment _processCcPayment;

        public CreditCardsController(IProcessCcPayment processCcPayment)
        {
            _processCcPayment = processCcPayment;
        }

        /// <summary>
        ///  Can Process a credit card payment.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to Process a credit card payment of any debtor account by passing the parameters. You need a valid token
        /// for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/SetAccountsDetails/SetBadNumbers/0001-000001&amp;4929000000006&amp;1221&amp;124&amp;1&amp;9.99
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
        [HttpPost("SetProcessCcPayments/{debtorAcc}&{ccNumber}&{expiredDate},&{cvv}&{numberOfPayments}&{amount}")]
        public async Task<IActionResult> SetProcessCcPayments(string debtorAcc, string ccNumber, string expiredDate, string cvv, int numberOfPayments, decimal amount)
        {
            Serilog.Log.Information("SetProcessCcPayments => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _processCcPayment.ProcessCcPayment(debtorAcc, ccNumber, expiredDate, cvv, numberOfPayments, amount, "P");

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
