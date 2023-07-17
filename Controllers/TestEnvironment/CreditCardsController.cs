using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models.Helper;
using AargonTools.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        private readonly IPreSchedulePaymentProcessing _preSchedulePaymentProcessing;
        private static GatewaySelectionHelper _gatewaySelectionHelper;
        private static ResponseModel _response;
        private readonly AdoDotNetConnection _adoConnection;

        public CreditCardsController(IProcessCcPayment processCcPayment, ISetCCPayment setCcPayment,
            IUniversalCcProcessApiService processCcUniversal, GatewaySelectionHelper gatewaySelectionHelper, IPreSchedulePaymentProcessing preSchedulePaymentProcessing,
            ResponseModel response, AdoDotNetConnection adoConnection)
        {
            _processCcPayment = processCcPayment;
            _setCcPayment = setCcPayment;
            _processCcUniversal = processCcUniversal;
            _gatewaySelectionHelper = gatewaySelectionHelper;
            _preSchedulePaymentProcessing = preSchedulePaymentProcessing;
            _response = response;
            _adoConnection = adoConnection;
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

                        var scheduleDateTime = DateTime.Now;//todo 
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

                            if (gatewaySelect.Result == "")
                            {

                                ProcessCcPaymentRequestModel requestForUsaEPay = new()
                                {
                                    ccNumber = requestCcPayment.ccNumber,
                                    amount = requestCcPayment.amount,
                                    cvv = requestCcPayment.cvv,
                                    debtorAcc = requestCcPayment.debtorAcc,
                                    expiredDate = requestCcPayment.expiredDate,
                                    hsa = requestCcPayment.hsa,
                                    key = requestCcPayment.key,
                                    numberOfPayments = requestCcPayment.numberOfPayments,
                                    pin = requestCcPayment.pin
                                };


                                var data = await _processCcPayment.ProcessCcPayment(requestForUsaEPay, "T");

                                var json = JsonConvert.SerializeObject(data.Data, Formatting.Indented);

                                var obj = JsonConvert.DeserializeObject<SetProcessCCResponse.TransactionDetails>(json);
                                var res = new CommonResponseModelForCCProcess()
                                {
                                    AuthorizationNumber = obj.result_code,
                                    ResponseCode = obj.authcode,
                                    ResponseMessage = obj.result,
                                    TransactionId = obj.key
                                };
                                var response = _response.Response(true, true, res);

                                return Ok(response);


                            }

                            return Ok("Oops.. something went wrong. Please submit the report with request example.");
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
        ///

        [ProducesResponseType(typeof(ProcessCCResponse), 200)]
        [HttpPost("ProcessCcQA")]
        public async Task<IActionResult> ProcessCcQA([FromBody] ProcessCcPaymentUniversalRequestModel requestCcPayment)
        {
            Serilog.Log.Information("ProcessCcQA => POST");
            try
            {
                if (ModelState.IsValid)
                {

                    if (requestCcPayment.debtorAcc != null)
                    {

                        var scheduleDateTime = DateTime.Now;//todo 
                        var acctLimitTemp = requestCcPayment.debtorAcc.Split('-');
                        var acctLimitCheck = Convert.ToInt64(acctLimitTemp[0] + acctLimitTemp[1]);
                        if (acctLimitCheck >= 4950000001 && acctLimitCheck < 4950999999)
                        {

                            ResponseModel response;

                            if (scheduleDateTime.Date == DateTime.Now.Date)
                            {
                                response = await _processCcUniversal.ProcessSaleTransForInstaMedQA(requestCcPayment, "CBT");
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
                                response = await _processCcUniversal.ProcessSaleTransForInstaMedQA(requestCcPayment, "CBT");
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
                                response = await _processCcUniversal.ProcessSaleTransForIProGatewayQA(requestCcPayment, "CBT");
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
                                    response = await _processCcUniversal.ProcessSaleTransForElavonQA(requestCcPayment, "CBT");
                                }
                                else
                                {
                                    response = await _processCcUniversal.ProcessCardAuthorizationForIProGateway(requestCcPayment, "CBT");
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
                                    debtorAcc = requestCcPayment.debtorAcc,
                                    expiredDate = requestCcPayment.expiredDate,
                                    hsa = requestCcPayment.hsa,
                                    key = requestCcPayment.key,
                                    numberOfPayments = requestCcPayment.numberOfPayments,
                                    pin = requestCcPayment.pin
                                };


                                var data = await _processCcPayment.ProcessCcPayment(requestForUsaEPay, "T");

                                var json = JsonConvert.SerializeObject(data.Data, Formatting.Indented);
                                if (json.Contains("Oops something went wrong"))
                                {
                                    return Ok(Response.StatusCode = 404);
                                }
                                else
                                {
                                    var obj = JsonConvert.DeserializeObject<SetProcessCCResponse.TransactionDetails>(json);
                                    var res = new CommonResponseModelForCCProcess()
                                    {
                                        AuthorizationNumber = obj.result_code,
                                        ResponseCode = obj.authcode,
                                        ResponseMessage = obj.result,
                                        TransactionId = obj.key
                                    };


                                    //cc payment insert 
                                    await _setCcPayment.SetCCPayment(new CcPaymnetRequestModel()
                                    {
                                        debtorAcc = requestCcPayment.debtorAcc,
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



                                    //this segment is for QA
                                    //cc_payment variables
                                    string idCc = "";
                                    string debtor_acctCc = "";
                                    string companyCc = "";
                                    string user_idCc = "";
                                    string user_nameCc = "";
                                    string subtotalCc = "";
                                    string charge_totalCc = "";
                                    string payment_dateCc = "";
                                    string approval_statusCc = "";
                                    string approval_codeCc = "";
                                    string error_codeCc = "";
                                    string order_numberCc = "";
                                    string ref_numberCc = "";
                                    string sifCc = "";







                                    var qaCC = _adoConnection.GetData("select * from cc_payment cc where cc.order_number='" + res.TransactionId + "'", "T");
                                    if (qaCC.Rows.Count > 0)
                                    {
                                        idCc = Convert.ToString(qaCC.Rows[0]["id"]); // 4
                                        debtor_acctCc = Convert.ToString(qaCC.Rows[0]["debtor_acct"]); // 3
                                        companyCc = Convert.ToString(qaCC.Rows[0]["company"]); // 2
                                        user_idCc = Convert.ToString(qaCC.Rows[0]["user_id"]); // 5
                                        user_nameCc = Convert.ToString(qaCC.Rows[0]["user_name"]); // 6
                                        subtotalCc = Convert.ToString(qaCC.Rows[0]["subtotal"]); // 1
                                        charge_totalCc = Convert.ToString(qaCC.Rows[0]["charge_total"]); // 1
                                        payment_dateCc = Convert.ToString(qaCC.Rows[0]["payment_date"]); // 1
                                        approval_statusCc = Convert.ToString(qaCC.Rows[0]["approval_status"]); // 1
                                        approval_codeCc = Convert.ToString(qaCC.Rows[0]["approval_code"]); // 1
                                        error_codeCc = Convert.ToString(qaCC.Rows[0]["error_code"]); // 1
                                        order_numberCc = Convert.ToString(qaCC.Rows[0]["order_number"]); // 1
                                        ref_numberCc = Convert.ToString(qaCC.Rows[0]["ref_number"]); // 1
                                        sifCc = Convert.ToString(qaCC.Rows[0]["sif"]); // 1
                                    }

                                    var ccPaymnetTable = "cc_payment >>>>" + "--id-->" + idCc + "-- debtor_acct-->" + debtor_acctCc + "-- company -->" + companyCc + "-- user_id -->" +
                                                         user_idCc + "-- user_name -->" + user_nameCc + "-- subtotal -->" + subtotalCc + "-- charge_total -->" + charge_totalCc
                                                         + "-- payment_date -->" +
                                                         payment_dateCc + "-- approval_status -->" + approval_statusCc + "-- approval_code -->" +
                                                         approval_codeCc + "-- error_code -->" + error_codeCc + "-- order_number -->" + order_numberCc + "-- ref_number -->"
                                                         + ref_numberCc + "--sif  -->" + sifCc;

                                    //[UCG_PaymentScheduleHistory] variables
                                    string Idhs = "";
                                    string PaymentScheduleIdhs = "";
                                    string TransactionIdhs = "";
                                    string ResponseCodehs = "";
                                    string ResponseMessagehs = "";
                                    string AuthorizationNumberhs = "";
                                    string AuthorizationTexths = "";
                                    string TimeLoghs = "";


                                    var qaHistory = _adoConnection.GetData("select * from UCG_PaymentScheduleHistory hs where hs.TransactionId='" + res.TransactionId + "'", "T");

                                    if (qaHistory.Rows.Count > 0)
                                    {
                                        Idhs = Convert.ToString(qaHistory.Rows[0]["Id"]); // 4
                                        PaymentScheduleIdhs = Convert.ToString(qaHistory.Rows[0]["PaymentScheduleId"]); // 3
                                        TransactionIdhs = Convert.ToString(qaHistory.Rows[0]["TransactionId"]); // 2
                                        ResponseCodehs = Convert.ToString(qaHistory.Rows[0]["ResponseCode"]); // 5
                                        ResponseMessagehs = Convert.ToString(qaHistory.Rows[0]["ResponseMessage"]); // 6
                                        AuthorizationNumberhs = Convert.ToString(qaHistory.Rows[0]["AuthorizationNumber"]); // 1
                                        AuthorizationTexths = Convert.ToString(qaHistory.Rows[0]["AuthorizationText"]); // 1
                                        TimeLoghs = Convert.ToString(qaHistory.Rows[0]["TimeLog"]); // 1
                                    }
                                    var PaymentScheduleHistoryTable = "UCG_PaymentScheduleHistory >>>>" + "--id-->" + Idhs + "--id-->" + PaymentScheduleIdhs + "--TransactionIdhs-->" + TransactionIdhs
                                                                      + "--ResponseCode-->" + ResponseCodehs + "--ResponseMessage-->" + ResponseMessagehs + "--AuthorizationNumber-->" + AuthorizationNumberhs
                                                                      + "--AuthorizationText-->" + AuthorizationTexths + "--TimeLog-->" + TimeLoghs;

                                    //[UCG_PaymentSchedule] variables
                                    string IdPs = "";
                                    string PatientAccountPs = "";
                                    string EffectiveDatePs = "";
                                    string CardInfoIdPs = "";
                                    string NumberOfPaymentsPs = "";
                                    string AmountPs = "";
                                    string IsActivePs = "";


                                    var qaPaymentScheduleTEmp = _adoConnection.GetData("select * from UCG_PaymentSchedule hs where hs.Id='" + PaymentScheduleIdhs + "'", "T");

                                    string CardInfoIdTemp = "";
                                    if (qaPaymentScheduleTEmp.Rows.Count>0)
                                    {
                                        CardInfoIdTemp = Convert.ToString(qaPaymentScheduleTEmp.Rows[0]["CardInfoId"]); // 5
                                    }

                                    var qaPaymentSchedule = _adoConnection.GetData("select * from UCG_PaymentSchedule hs where hs.CardInfoId='" + CardInfoIdTemp + "'", "T");
                                    var qaPaymentScheduleStringList = new List<string>
                                    {
                                        Capacity = 10
                                    };
                                    if (qaPaymentSchedule.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < qaPaymentSchedule.Rows.Count; i++)
                                        {
                                            IdPs = Convert.ToString(qaPaymentSchedule.Rows[i]["Id"]); // 4
                                            PatientAccountPs = Convert.ToString(qaPaymentSchedule.Rows[i]["PatientAccount"]); // 3
                                            EffectiveDatePs = Convert.ToString(qaPaymentSchedule.Rows[i]["EffectiveDate"]); // 2
                                            CardInfoIdPs = Convert.ToString(qaPaymentSchedule.Rows[i]["CardInfoId"]); // 5
                                            NumberOfPaymentsPs = Convert.ToString(qaPaymentSchedule.Rows[i]["NumberOfPayments"]); // 6
                                            AmountPs = Convert.ToString(qaPaymentSchedule.Rows[i]["Amount"]); // 1
                                            IsActivePs = Convert.ToString(qaPaymentSchedule.Rows[i]["IsActive"]); // 1


                                            qaPaymentScheduleStringList.Add("UCG_PaymentSchedule >>>>" + "--id-->" + IdPs + "--PatientAccount-->" + PatientAccountPs +
                                                                            "--EffectiveDate-->" + EffectiveDatePs + "--CardInfoId-->" + CardInfoIdPs
                                                                            + "--NumberOfPayments-->" + NumberOfPaymentsPs + "--Amount-->" + AmountPs + "--IsActive-->" + IsActivePs);
                                        }
                                    }



                                    //[UCG_CardInfo] variables
                                    string Idci = "";
                                    string PaymentMethodID = "";
                                    string EntryMode = "";
                                    string Type = "";
                                    string BinNumber = "";
                                    string LastFour = "";
                                    string ExpirationMonth = "";
                                    string ExpirationYear = "";
                                    string CardHolderName = "";
                                    string AssociateDebtorAcct = "";
                                    string IsActiveci = "";


                                    var qaUCG_CardInfo = _adoConnection.GetData("select * from UCG_CardInfo hs where hs.Id='" + CardInfoIdPs + "'", "T");

                                    if (qaUCG_CardInfo.Rows.Count > 0)
                                    {
                                        Idci = Convert.ToString(qaUCG_CardInfo.Rows[0]["Id"]); // 4
                                        PaymentMethodID = Convert.ToString(qaUCG_CardInfo.Rows[0]["PaymentMethodID"]); // 3
                                        EntryMode = Convert.ToString(qaUCG_CardInfo.Rows[0]["EntryMode"]); // 2
                                        Type = Convert.ToString(qaUCG_CardInfo.Rows[0]["Type"]); // 5
                                        BinNumber = Convert.ToString(qaUCG_CardInfo.Rows[0]["BinNumber"]); // 6
                                        LastFour = Convert.ToString(qaUCG_CardInfo.Rows[0]["LastFour"]); // 1
                                        ExpirationMonth = Convert.ToString(qaUCG_CardInfo.Rows[0]["ExpirationMonth"]); // 1
                                        ExpirationYear = Convert.ToString(qaUCG_CardInfo.Rows[0]["ExpirationYear"]); // 1
                                        CardHolderName = Convert.ToString(qaUCG_CardInfo.Rows[0]["CardHolderName"]); // 1
                                        AssociateDebtorAcct = Convert.ToString(qaUCG_CardInfo.Rows[0]["AssociateDebtorAcct"]); // 1
                                        IsActiveci = Convert.ToString(qaUCG_CardInfo.Rows[0]["IsActive"]); // 1
                                    }

                                    var qaUCG_CardInfoString = "UCG_CardInfo >>>>" + "--Idci-->" + Idci + "--PaymentMethodID-->" + PaymentMethodID + "--EntryMode-->" + EntryMode
                                                               + "--Type-->" + Type + "--BinNumber-->" + BinNumber + "--LastFour-->" + LastFour
                                                               + "--ExpirationMonth-->" + ExpirationMonth + "--ExpirationYear-->" + ExpirationYear + "--CardHolderName-->" + CardHolderName
                                                               + "--AssociateDebtorAcct-->" + AssociateDebtorAcct + "--CardHolderName-->" + CardHolderName + "--AssociateDebtorAcct-->" + AssociateDebtorAcct
                                                               + "--IsActive-->" + IsActiveci;


                                    var dbQA = new List<string>
                                    {
                                        Capacity = 5
                                    };

                                    dbQA.Add(ccPaymnetTable);
                                    dbQA.Add(qaUCG_CardInfoString);
                                    dbQA.Add(PaymentScheduleHistoryTable);
                                    dbQA.AddRange(qaPaymentScheduleStringList);


                                    //

                                    //return (IActionResult)_response.Response(true, true, res, dbQA);




                                    var response = _response.Response(true, true, res,dbQA);
                                    return Ok(response);
                                }

                            }

                            return Ok("Oops.. something went wrong. Please submit the report with request example.");
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
        ///


        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(typeof(ProcessCCResponse), 200)]
        [HttpGet("PreScheduledPaymentProcessing")]
        public async Task<IActionResult> PreScheduledPaymentProcessing()
        {
            Serilog.Log.Information("PreScheduledPaymentProcessing => POST");

            AutoProcessCcUniversalViewModel requestCcPayment = null;

            SaleRequestModelForInstamed universalSaleRequestModel = null;

            var _preScheduleLcgTablesViewModel = new LcgTablesViewModel();


            try
            {
                if (ModelState.IsValid)
                {

                    await GetAll("CBT");


                    async Task GetAll(string environment)
                    {



                        var _paymentSchedule = await _preSchedulePaymentProcessing.GetAllPreSchedulePaymentInfo(environment);


                        for (int i = 0; i < _paymentSchedule.Count; i++)
                        {

                            if (_paymentSchedule[i].IsActive == true)
                            {

                                await OpenOrder(_paymentSchedule[i].Id, environment);

                            }

                        }



                    }






                    async Task OpenOrder(int orderId, string environment)
                    {

                        _preScheduleLcgTablesViewModel =
                           await _preSchedulePaymentProcessing.GetDetailsOfPreSchedulePaymentInfo(orderId, environment);
                        //_tempAmount = _preScheduleLcgTablesViewModel.Amount;
                        //await ProcessSaleTrans();
                        requestCcPayment.AssociateDebtorAcct = _preScheduleLcgTablesViewModel.AssociateDebtorAcct;
                        //requestCcPayment.amount=_preScheduleLcgTablesViewModel.

                    }



                    if (requestCcPayment.AssociateDebtorAcct != null)
                    {

                        var scheduleDateTime = DateTime.Now;//todo 
                        var acctLimitTemp = requestCcPayment.AssociateDebtorAcct.Split('-');
                        var acctLimitCheck = Convert.ToInt64(acctLimitTemp[0] + acctLimitTemp[1]);
                        if (acctLimitCheck >= 4950000001 && acctLimitCheck < 4950999999)
                        {

                            ResponseModel response;

                            response = await _processCcUniversal.ProcessOnFileSaleTransForInstaMed(requestCcPayment, "CBT");

                            return Ok(response);

                        }

                        if (acctLimitCheck >= 4953000001 && acctLimitCheck < 4953999999)
                        {

                            ResponseModel response;

                            response = await _processCcUniversal.ProcessOnFileSaleTransForInstaMed(requestCcPayment, "CBT");

                            return Ok(response);

                        }

                        if (acctLimitCheck >= 4514000001 && acctLimitCheck < 4514999999)
                        {

                            ResponseModel response;

                            response = await _processCcUniversal.ProcessOnfileSaleTransForIProGateway(requestCcPayment, "CBT");

                            return Ok(response);

                        }

                        else
                        {

                            var gatewaySelect = _gatewaySelectionHelper.UniversalCcProcessGatewaySelectionHelper(requestCcPayment.AssociateDebtorAcct, "CBT");

                            if (gatewaySelect.Result == "ELAVON")
                            {

                                ResponseModel response;

                                response = await _processCcUniversal.ProcessOnfileSaleTransForElavon(requestCcPayment, "CBT");

                                return Ok(response);

                            }

                            if (gatewaySelect.Result == "")
                            {

                                //ProcessCcPaymentRequestModel requestForUsaEPay = new()
                                //{
                                //    ccNumber = requestCcPayment.ccNumber,
                                //    amount = requestCcPayment.amount,
                                //    cvv = requestCcPayment.cvv,
                                //    debtorAcc = requestCcPayment.debtorAcc,
                                //    expiredDate = requestCcPayment.expiredDate,
                                //    hsa = requestCcPayment.hsa,
                                //    key = requestCcPayment.key,
                                //    numberOfPayments = requestCcPayment.numberOfPayments,
                                //    pin = requestCcPayment.pin
                                //};


                                //var data = await _processCcPayment.ProcessCcPayment(requestForUsaEPay, "T");


                                //return Ok(data);


                            }

                            return Ok("Oops.. something went wrong. Please submit the report with request example.");

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