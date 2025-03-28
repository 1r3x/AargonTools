using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.Interfaces.ProcessCC;
using AargonTools.Manager.GenericManager;
using AargonTools.Manager.ProcessCCManager;
using AargonTools.Models;
using AargonTools.Models.Helper;
using AargonTools.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

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
        private readonly IUniversalCcProcessApiService _processCcUniversal;
        private readonly IPreSchedulePaymentProcessing _preSchedulePaymentProcessing;
        private static GatewaySelectionHelper _gatewaySelectionHelper;
        private static ResponseModel _response;
        private readonly AdoDotNetConnection _adoConnection;
        private static ICryptoGraphy _crypto;
        private static ICardTokenizationDataHelper _cardTokenizationHelper;
        private static IProcessCcPayment _usaEPay;
        private readonly GetTheCompanyFlag _getTheCompanyFlag;
        private static IViewingSchedulePayments _viewingSchedulePaymnets;
        private readonly PaymentGatewayFactory _gatewayFactory;
        private readonly IServiceProvider _serviceProvider;

        public CreditCardsController(IProcessCcPayment processCcPayment, ISetCCPayment setCcPayment,
            IUniversalCcProcessApiService processCcUniversal, GatewaySelectionHelper gatewaySelectionHelper, IPreSchedulePaymentProcessing preSchedulePaymentProcessing,
            ResponseModel response, AdoDotNetConnection adoConnection, ICryptoGraphy crypto, ICardTokenizationDataHelper cardTokenizationHelper,
            IProcessCcPayment usaEPayIMlementtaions, GetTheCompanyFlag getTheCompanyFlag, IViewingSchedulePayments viewingSchedulePaymnets, PaymentGatewayFactory gatewayFactory,
            IServiceProvider serviceProvider)
        {
            _processCcPayment = processCcPayment;
            _setCcPayment = setCcPayment;
            _processCcUniversal = processCcUniversal;
            _gatewaySelectionHelper = gatewaySelectionHelper;
            _preSchedulePaymentProcessing = preSchedulePaymentProcessing;
            _response = response;
            _adoConnection = adoConnection;
            _crypto = crypto;
            _cardTokenizationHelper = cardTokenizationHelper;
            _usaEPay = usaEPayIMlementtaions;
            _getTheCompanyFlag = getTheCompanyFlag;
            _viewingSchedulePaymnets = viewingSchedulePaymnets;
            _gatewayFactory = gatewayFactory;
            _serviceProvider = serviceProvider;
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
        public async Task<IActionResult> SchedulePostData([FromBody] SchedulePostDateRequest request)
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

        public async Task<IActionResult> SetCcPayments([FromBody] CcPaymnetRequestModel requestCcPayment)
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

        /// <summary>
        ///  Can process CC Payments.
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

                    if (requestCcPayment.debtorAcct != null)
                    {

                        var scheduleDateTime = DateTime.Now;//todo 
                        var acctLimitTemp = requestCcPayment.debtorAcct.Split('-');
                        var acctLimitCheck = Convert.ToInt64(acctLimitTemp[0] + acctLimitTemp[1]);

                        var patientBalanceCheck = await _getTheCompanyFlag.GetFlagForDebtorAccount(requestCcPayment.debtorAcct, "P")
                            .Result.Where(x => x.DebtorAcct == requestCcPayment.debtorAcct).Select(i =>
                                   new DebtorAcctInfoT()
                                   {
                                       SuppliedAcct = i.SuppliedAcct,
                                       Balance = i.Balance
                                   }).SingleOrDefaultAsync();

                        if (Convert.ToDecimal(requestCcPayment.numberOfPayments) * Convert.ToDecimal(requestCcPayment.amount) <= patientBalanceCheck?.Balance)
                        {




                            if (acctLimitCheck >= 4950000001 && acctLimitCheck < 4950999999 || acctLimitCheck >= 4984000001 && acctLimitCheck < 4984999999)
                            {

                                ResponseModel response;

                                if (scheduleDateTime.Date == DateTime.Now.Date)
                                {
                                    response = await _processCcUniversal.ProcessSaleTransForInstaMed(requestCcPayment, "P");
                                }
                                else
                                {
                                    response = await _processCcUniversal.ProcessCardAuthorizationForInstaMed(requestCcPayment, "P");
                                }

                                return Ok(response);

                            }

                            if (acctLimitCheck >= 4953000001 && acctLimitCheck < 4953999999 || acctLimitCheck >= 4985000001 && acctLimitCheck < 4985999999)
                            {

                                ResponseModel response;
                                if (scheduleDateTime.Date == DateTime.Now.Date)
                                {
                                    response = await _processCcUniversal.ProcessSaleTransForInstaMed(requestCcPayment, "P");
                                }
                                else
                                {
                                    response = await _processCcUniversal.ProcessCardAuthorizationForInstaMed(requestCcPayment, "P");
                                }

                                return Ok(response);

                            }

                            if (acctLimitCheck >= 4514000001 && acctLimitCheck < 4514999999)
                            {

                                ResponseModel response;
                                if (scheduleDateTime.Date == DateTime.Now.Date)
                                {
                                    response = await _processCcUniversal.ProcessSaleTransForIProGateway(requestCcPayment, "P");
                                }
                                else
                                {
                                    response = await _processCcUniversal.ProcessCardAuthorizationForIProGateway(requestCcPayment, "P");
                                }

                                return Ok(response);
                            }

                            else
                            {
                                var gatewaySelect = _gatewaySelectionHelper.UniversalCcProcessGatewaySelectionHelper(requestCcPayment.debtorAcct, "P");

                                if (gatewaySelect.Result == "ELAVON" || acctLimitCheck >= 1902000001 && acctLimitCheck < 1902999999)//for staging 
                                {
                                    ResponseModel response;
                                    if (scheduleDateTime.Date == DateTime.Now.Date)
                                    {
                                        response = await _processCcUniversal.ProcessSaleTransForElavon(requestCcPayment, "P");
                                    }
                                    else
                                    {
                                        response = await _processCcUniversal.ProcessCardAuthorizationForIProGateway(requestCcPayment, "P");
                                    }

                                    return Ok(response);
                                }

                                if (gatewaySelect.Result == "TMCBONHAMELAVON")
                                {
                                    ResponseModel response;
                                    if (scheduleDateTime.Date == DateTime.Now.Date)
                                    {
                                        response = await _processCcUniversal.ProcessSaleTransForTmcElavon(requestCcPayment, "P");
                                    }
                                    else
                                    {
                                        response = await _processCcUniversal.ProcessCardAuthorizationForIProGateway(requestCcPayment, "P");
                                    }

                                    return Ok(response);
                                }



                                if (gatewaySelect.Result == "")
                                {

                                    ProcessCcPaymentRequestModel requestForUsaEPay = new()
                                    {
                                        ccNumber = requestCcPayment.ccNumber,
                                        amount = requestCcPayment.amount,
                                        cvv = requestCcPayment.cvv,
                                        debtorAcc = requestCcPayment.debtorAcct,
                                        expiredDate = requestCcPayment.expiredDate,
                                        hsa = requestCcPayment.hsa,
                                        numberOfPayments = requestCcPayment.numberOfPayments
                                    };

                                    //must be break the implementtaion 
                                    //var data = await _processCcPayment.ProcessCcPayment(requestForUsaEPay, "P");
                                    //
                                    var tokenizeDataJsonResult = _usaEPay.TokenizeCc(requestForUsaEPay.debtorAcc, requestForUsaEPay.ccNumber, requestForUsaEPay.expiredDate,
                                        requestForUsaEPay.hsa != null && (bool)requestForUsaEPay.hsa, "P").Result;
                                    SaveCard tokenizeCObj = new SaveCard()
                                    {
                                        CardNumber = "",
                                        Key = "",
                                        Type = ""
                                    };
                                    if (tokenizeDataJsonResult.Data != null)
                                    {
                                        tokenizeCObj = JsonConvert.DeserializeObject<SaveCard>(tokenizeDataJsonResult.Data.ToString() ?? string.Empty);
                                    }




                                    var processTransactionJsonResult = _usaEPay.ProcessingTransactionV2(tokenizeCObj.Key,
                                        (decimal)requestForUsaEPay.amount, requestForUsaEPay.hsa != null && (bool)requestForUsaEPay.hsa,
                                         requestForUsaEPay.debtorAcc, requestCcPayment.cardHolderName, "P").Result.Data;

                                    //

                                    var json = JsonConvert.SerializeObject(processTransactionJsonResult, Formatting.Indented);

                                    var obj = JsonConvert.DeserializeObject<SetProcessCCResponse.TransactionDetails>(json);
                                    var res = new CommonResponseModelForCCProcess()
                                    {
                                        AuthorizationNumber = obj.result_code,
                                        ResponseCode = obj.authcode,
                                        ResponseMessage = obj.result,
                                        TransactionId = obj.refnum
                                    };

                                    //updated 

                                    var ccNUmber = requestForUsaEPay.ccNumber;



                                    var cardInfoObj = new LcgCardInfo()
                                    {
                                        IsActive = true,
                                        EntryMode = "key",
                                        BinNumber = requestForUsaEPay.cvv,
                                        ExpirationMonth = 1,//todo 
                                        ExpirationYear = 2,//todo
                                        LastFour = ccNUmber.Substring(ccNUmber.Length - 4),
                                        PaymentMethodId = tokenizeCObj.Key,
                                        Type = tokenizeCObj.Type,
                                        AssociateDebtorAcct = requestForUsaEPay.debtorAcc,
                                        CardHolderName = requestCcPayment.cardHolderName
                                    };


                                    await _cardTokenizationHelper.CreateCardInfo(cardInfoObj, "P");


                                    var paymentScheduleObj = new LcgPaymentSchedule()
                                    {
                                        CardInfoId = cardInfoObj.Id,
                                        IsActive = true,
                                        EffectiveDate = DateTime.Now,
                                        NumberOfPayments = (int)requestForUsaEPay.numberOfPayments,
                                        PatientAccount = "", //todo patient account
                                        Amount = requestForUsaEPay.amount
                                    };

                                    int _USAePayPaymentScheduleId = 0;

                                    var paymentDate = paymentScheduleObj.EffectiveDate;
                                    for (var i = 1; i <= requestForUsaEPay.numberOfPayments; i++)
                                    {
                                        var lcgPaymentScheduleObj = new LcgPaymentSchedule()
                                        {
                                            CardInfoId = paymentScheduleObj.CardInfoId,
                                            EffectiveDate = paymentDate,
                                            IsActive = true,
                                            NumberOfPayments = i,
                                            PatientAccount = paymentScheduleObj.PatientAccount,
                                            Amount = paymentScheduleObj.Amount
                                        };
                                        await _cardTokenizationHelper.CreatePaymentSchedule(lcgPaymentScheduleObj, "P");

                                        if (i == 1)
                                        {
                                            _USAePayPaymentScheduleId = lcgPaymentScheduleObj.Id;
                                        }

                                        paymentDate = paymentDate.AddMonths(1);

                                    }


                                    var paymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
                                    {
                                        ResponseCode = res.ResponseCode,
                                        AuthorizationNumber = res.AuthorizationNumber,
                                        AuthorizationText = "_username", //todo user name 
                                        ResponseMessage = res.ResponseMessage,
                                        PaymentScheduleId = _USAePayPaymentScheduleId,
                                        TransactionId = res.TransactionId,
                                        TimeLog = DateTime.Now
                                    };

                                    await _cardTokenizationHelper.CreatePaymentScheduleHistory(paymentScheduleHistoryObj, "P");


                                    //cc payment insert 
                                    await _setCcPayment.SetCCPayment(new CcPaymnetRequestModel()
                                    {
                                        debtorAcc = requestCcPayment.debtorAcct,
                                        approvalCode = "",
                                        approvalStatus = "APPROVED",
                                        chargeTotal = (decimal)requestCcPayment.amount,
                                        company = "AARGON AGENCY",
                                        sif = "Y",
                                        paymentDate = DateTime.Now,
                                        refNo = "USAEPAY2",
                                        orderNumber = res.TransactionId,
                                        userId = "WEB",
                                    }, "T");


                                    //

                                    var response = _response.Response(true, true, res);

                                    return Ok(response);


                                }

                                return Ok("Oops.. something went wrong. Please submit the report with request example.");
                            }
                        }
                        else
                        {
                            return new JsonResult("Balance Verification Failed") { StatusCode = 500 };
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.InnerException, e.Message, e.Data);
                return Ok(e);
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };
        }


        /// <summary>
        ///  Can process CC Payments.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can process CC payments by passing required parameters.
        /// This endpoint can process through multiple gateways.It will automatically choose the desire gateways by debtor acct number.
        /// You need a valid token for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/CreditCards/ProcessCcV2
        /// (pass JSON body like the request example)
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        ///

        [ProducesResponseType(typeof(ProcessCCResponse), 200)]
        [HttpPost("ProcessCcV2")]
        public async Task<IActionResult> ProcessCcV2([FromBody] ProcessCcPaymentUniversalRequestModel requestCcPayment)
        {
            Serilog.Log.Information("ProcessCcV2 => POST request received with Debtor Account: {@debtorAcct}", requestCcPayment.debtorAcct);
            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(requestCcPayment.debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", requestCcPayment.debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }

            // Remove any non-digit characters

            requestCcPayment.ccNumber = Regex.Replace(requestCcPayment.ccNumber, @"\D", "");

            // Regular expression to match credit card numbers

            string pattern = @"^(?:4[0-9]{12}(?:[0-9]{3})?          # Visa
                           |  5[1-5][0-9]{14}                  # MasterCard
                           |  3[47][0-9]{13}                   # American Express
                           |  3(?:0[0-5]|[68][0-9])[0-9]{11}   # Diners Club
                           |  6(?:011|5[0-9]{2})[0-9]{12}      # Discover
                           |  (?:2131|1800|35\d{3})\d{11}      # JCB
                          )$";

            // Validate the card number format

            if (!Regex.IsMatch(requestCcPayment.ccNumber, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                Serilog.Log.Warning("Invalid credit card format for ccNumber: {CcNumber}", requestCcPayment.ccNumber);
                return BadRequest("Invalid credit card format.");
            }

            // Validate cvv
            var cvvRegex = new Regex(@"^\d{3,4}$");
            if (!cvvRegex.IsMatch(requestCcPayment.cvv))
            {
                Serilog.Log.Warning("Invalid CVV format for cvv: {cvv}", requestCcPayment.cvv);
                return BadRequest("Invalid CVV format. It must be 3 or 4 digits.");
            }



            try
            {
                if (ModelState.IsValid)
                {
                    var gateway = _gatewayFactory.GetPaymentGateway(requestCcPayment.debtorAcct, "P");
                    var response = await gateway.ProcessPayment(requestCcPayment, "P");
                    //if transaction is successful
                    if (response is ResponseWithTransaction responseWithTransaction)
                    {
                        if (responseWithTransaction.TransactionStatus == true)
                        {
                            Serilog.Log.Information("Transaction successful. Saving card info...");
                            var status = await gateway.SaveCardInfo(requestCcPayment, "P");
                            Serilog.Log.Information("Card info saved");
                        }
                    }
                    Serilog.Log.Information("Returning response: {@Response}", response);
                    return Ok(response);
                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing ProcessCcV2 request with Debtor Account: {@debtorAcct}", requestCcPayment.debtorAcct);
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };


        }

        /// <summary>
        ///  Can process TCR Payments.
        /// </summary>
        /// 
        /// <remarks>
        /// **Details**:
        /// You can process TCR payments by passing required parameters.
        /// This endpoint can process through multiple gateways.It will automatically choose the desire gateways by debtor acct number.
        /// You need a valid token for this endpoint .
        ///You can pass the parameter with API client like https://g14.aargontools.com/api/CreditCards/TCRProcessCC
        /// (pass JSON body like the request example)
        /// </remarks>
        /// <response code="200">Execution Successful</response>
        /// <response code="401">Unauthorized , please login or refresh your token.</response>
        /// 
        ///

        [ProducesResponseType(typeof(ProcessCCResponse), 200)]
        [HttpPost("TCRProcessCC")]
        public async Task<IActionResult> TCRProcessCC([FromBody] ProcessCcPaymentUniversalRequestModel requestCcPayment)
        {
            Serilog.Log.Information("TCRProcessCC => POST request received with Debtor Account: {@debtorAcct}", requestCcPayment.debtorAcct);

            // Validate debtor account format
            var debtorAcctRegex = new Regex(@"^\d{4}-\d{6}$");
            if (!debtorAcctRegex.IsMatch(requestCcPayment.debtorAcct))
            {
                Serilog.Log.Warning("Invalid debtor account format for debtorAcct: {DebtorAcct}", requestCcPayment.debtorAcct);
                return BadRequest("Invalid debtor account format. It must be in the format 0000-000000.");
            }

            // Remove any non-digit characters

            requestCcPayment.ccNumber = Regex.Replace(requestCcPayment.ccNumber, @"\D", "");

            // Regular expression to match credit card numbers

            string pattern = @"^(?:4[0-9]{12}(?:[0-9]{3})?          # Visa
                           |  5[1-5][0-9]{14}                  # MasterCard
                           |  3[47][0-9]{13}                   # American Express
                           |  3(?:0[0-5]|[68][0-9])[0-9]{11}   # Diners Club
                           |  6(?:011|5[0-9]{2})[0-9]{12}      # Discover
                           |  (?:2131|1800|35\d{3})\d{11}      # JCB
                          )$";

            // Validate the card number format

            if (!Regex.IsMatch(requestCcPayment.ccNumber, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                Serilog.Log.Warning("Invalid credit card format for ccNumber: {CcNumber}", requestCcPayment.ccNumber);
                return BadRequest("Invalid credit card format.");
            }

            // Validate cvv
            var cvvRegex = new Regex(@"^\d{3,4}$");
            if (!cvvRegex.IsMatch(requestCcPayment.cvv))
            {
                Serilog.Log.Warning("Invalid CVV format for cvv: {cvv}", requestCcPayment.cvv);
                return BadRequest("Invalid CVV format. It must be 3 or 4 digits.");
            }



            try
            {
                var companyFlagString = await _getTheCompanyFlag.GetStringFlag(requestCcPayment.debtorAcct, "P");

                if (ModelState.IsValid)
                {
                    if (companyFlagString == "T")
                    {
                        var gateway = _serviceProvider.GetService<UsaEPayManager>();
                        var response = await gateway.ProcessPayment(requestCcPayment, "P");
                        //if transaction is successful
                        if (response is ResponseWithTransaction responseWithTransaction)
                        {
                            if (responseWithTransaction.TransactionStatus == true)
                            {
                                Serilog.Log.Information("Transaction successful. Saving card info...");
                                var status = await gateway.SaveCardInfo(requestCcPayment, "P");
                                Serilog.Log.Information("Card info saved");
                            }
                        }
                        Serilog.Log.Information("Returning response: {@Response}", response);
                        return Ok(response);
                    }
                    else
                    {
                        Serilog.Log.Warning("Invalid debtor account as TCR: {debtorAcct}", requestCcPayment.debtorAcct);
                        return BadRequest("Invalid debtor account as TCR");
                    }

                }
            }
            catch (Exception e)
            {
                Serilog.Log.Error(e, "An error occurred while processing TCRProcessCC request with Debtor Account: {@debtorAcct}", requestCcPayment.debtorAcct);
                return StatusCode(500, "An internal server error occurred. Please try again later.");
            }


            return new JsonResult("Something went wrong") { StatusCode = 500 };


        }

        //[ProducesResponseType(typeof(SetProcessCCResponse), 200)]
        [HttpPost("ViewingSchedulePayments")]
        public IActionResult ViewingSchedulePayments([FromBody] ViewingSchedulePaymentsRequestModel viewingRequest)
        {
            Serilog.Log.Information("ViewingSchedulePayments => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _viewingSchedulePaymnets.GetSchedulePayments(viewingRequest, "P");

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



        //[ProducesResponseType(typeof(SetProcessCCResponse), 200)]
        [HttpPost("UpdatingSchedulePayments")]
        public IActionResult UpdatingSchedulePayments([FromBody] UpdatingSchedulePayments updatingSchedulePaymentsRequest)
        {
            Serilog.Log.Information("UpdatingSchedulePayments => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _viewingSchedulePaymnets.UpdatingSchedulePayment(updatingSchedulePaymentsRequest, "P");

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

        //[ProducesResponseType(typeof(SetProcessCCResponse), 200)]
        [HttpPost("DeleteSchedulePayments")]
        public IActionResult DeleteSchedulePayments([FromBody] DeleteSchedulePayments deleteSchedulePaymentsRequest)
        {
            Serilog.Log.Information("DeleteSchedulePayments => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _viewingSchedulePaymnets.DeleteSchedulePayment(deleteSchedulePaymentsRequest, "P");

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


        //[ProducesResponseType(typeof(SetProcessCCResponse), 200)]
        [HttpPost("ViewingPaymentHistory")]
        public IActionResult ViewingPaymentHistory([FromBody] ViewingSchedulePaymentsRequestModel viewingRequest)
        {
            Serilog.Log.Information("ViewingPaymentHistory => POST");
            try
            {
                if (ModelState.IsValid)
                {
                    var data = _viewingSchedulePaymnets.ViewPaymentHistory(viewingRequest, "P");

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

    internal class SaveCard
    {
        public string Type { get; set; }
        public string Key { get; set; }
        public string CardNumber { get; set; }
    }








}
