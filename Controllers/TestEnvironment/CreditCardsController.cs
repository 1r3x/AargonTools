using System;
using System.Threading.Tasks;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AargonTools.Controllers.TestEnvironment
{
    [Route("api/Test/[controller]")]
    [Produces("application/json")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CreditCardsController : ControllerBase
    {
        private readonly IProcessCcPayment _processCcPayment;
        private readonly ISetCCPayment _setCcPayment;
        private readonly IUniversalCcProcessApiService _processCcUniversal;
        private static GatewaySelectionHelper _gatewaySelectionHelper;

        public CreditCardsController(IProcessCcPayment processCcPayment, ISetCCPayment setCcPayment, IUniversalCcProcessApiService processCcUniversal, GatewaySelectionHelper gatewaySelectionHelper)
        {
            _processCcPayment = processCcPayment;
            _setCcPayment = setCcPayment;
            _processCcUniversal = processCcUniversal;
            _gatewaySelectionHelper = gatewaySelectionHelper;
        }

        /// <summary>
        ///  Can Process a credit card payment.(Test Environment)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can use this end point to Process a credit card payment of any debtor account by passing the parameters. You need a valid token
        /// for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/Test/CreditCards/SetProcessCcPayments
        /// (pass JSON body like the request example)
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>

        /// 
        [ProducesResponseType(typeof(SetProcessCCResponse), 200)]
        [HttpPost("SetProcessCcPayments")]
        public async Task<IActionResult> SetProcessCcPayments([FromBody] ProcessCcPaymentRequestModel requestCcPayment)
        {
            Serilog.Log.Information("Test SetProcessCcPayments => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _processCcPayment.ProcessCcPayment(requestCcPayment, "T");

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
        ///  Can Schedule Post Data.(Test Environment)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can Schedule Post Data of any debtor account by passing the parameters. You need a valid token
        /// for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/Test/CreditCards/SchedulePostData
        /// (pass JSON body like the request example)
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>

        [ProducesResponseType(typeof(SchedulePostDateResponse), 200)]
        [HttpPost("SchedulePostData")]
        public async Task<IActionResult> SchedulePostData([FromBody] SchedulePostDateRequest request)
        {
            Serilog.Log.Information("Test SchedulePostData => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _processCcPayment.SchedulePostDataV2(request, "T");

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
        ///  Can set CC Payments.(Test Environment)
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can set cc payment by passing required parameters. You need a valid token
        /// for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/Test/CreditCards/SetCcPayments
        /// (pass JSON body like the request example)
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>

        ///


        [ProducesResponseType(typeof(SetCcPaymnetResponse), 200)]
        [HttpPost("SetCcPayments")]
        public async Task<IActionResult> SetCcPayments([FromBody] CcPaymnetRequestModel requestCcPayment)
        {
            Serilog.Log.Information("Test SetProcessCcPayments => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = await _setCcPayment.SetCCPayment(requestCcPayment, "T");

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
        ///  Can process CC Payments.(New Test Environment [C-SQLTEST1])
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can process CC payments by passing required parameters.
        /// This endpoint can process through multiple gateways.It will automatically choose the desire gateways by debtor acct number.
        /// You need a valid token for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/Test/CreditCards/ProcessCc
        /// (pass JSON body like the request example)
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>

        ///

        [ProducesResponseType(typeof(ProcessCCResponse), 200)]
        [HttpPost("ProcessCc")]
        public async Task<IActionResult> ProcessCc([FromBody] ProcessCcPaymentUniversalRequestModel requestCcPayment)
        {
            Serilog.Log.Information("ProcessCc => POST");
            try
            {
                if (ModelState.IsValid)
                {


                    if (requestCcPayment.debtorAcc != null)
                    {
                        DateTime scheduleDateTime = DateTime.Now;//todo 
                        var acctLimitTemp = requestCcPayment.debtorAcc.Split('-');
                        var acctLimitCheck = Convert.ToInt64(acctLimitTemp[0] + acctLimitTemp[1]);
                        if (acctLimitCheck >= 4950000001 && acctLimitCheck < 4950999999)
                        {

                            ResponseModel response;

                            if (scheduleDateTime.Date == DateTime.Now.Date)
                            {
                                response = await _processCcUniversal.ProcessSaleTransForInstaMed(requestCcPayment, "CBT");
                            }
                            else
                            {
                                response = await _processCcUniversal.ProcessCardAuthorizationForInstaMed(requestCcPayment, "CBT");
                            }

                            return Ok(response);
                        }
                        if (acctLimitCheck >= 4953000001 && acctLimitCheck < 4953999999)
                        {
                            ResponseModel response;
                            if (scheduleDateTime.Date == DateTime.Now.Date)
                            {
                                response = await _processCcUniversal.ProcessSaleTransForInstaMed(requestCcPayment, "CBT");
                            }
                            else
                            {
                                response = await _processCcUniversal.ProcessCardAuthorizationForInstaMed(requestCcPayment, "CBT");
                            }

                            return Ok(response);
                        }
                        if (acctLimitCheck >= 4514000001 && acctLimitCheck < 4514999999)
                        {
                            ResponseModel response;
                            if (scheduleDateTime.Date == DateTime.Now.Date)
                            {
                                response = await _processCcUniversal.ProcessSaleTransForIProGateway(requestCcPayment, "CBT");
                            }
                            else
                            {
                                response = await _processCcUniversal.ProcessCardAuthorizationForIProGateway(requestCcPayment, "CBT");
                            }

                            return Ok(response);
                        }
                        else
                        {
                            var gatewaySelect = _gatewaySelectionHelper.UniversalCcProcessGatewaySelectionHelper(requestCcPayment.debtorAcc, "CBT");
                            if (gatewaySelect.Result == "ELAVON")
                            {
                                ResponseModel response;
                                if (scheduleDateTime.Date == DateTime.Now.Date)
                                {
                                    response = await _processCcUniversal.ProcessSaleTransForElavon(requestCcPayment, "CBT");
                                }
                                else
                                {
                                    response = await _processCcUniversal.ProcessCardAuthorizationForIProGateway(requestCcPayment, "CBT");
                                }

                                return Ok(response);
                            }

                            //if (gatewaySelct=="")
                            //{

                            //}

                            return Ok();
                        }
                    }
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