using AargonTools.Controllers;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.Interfaces.ProcessCC;
using AargonTools.Manager.GenericManager;
using AargonTools.Models.Helper;
using AargonTools.Models;
using AargonTools.ViewModel;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using Microsoft.Extensions.Options;
using System.Net.Http;

namespace AargonTools.Manager.ProcessCCManager
{
    public class UsaEPayManager : IPaymentGateway
    {
        private readonly HttpClient _clientForInstaMed = new();
        private readonly IOptions<CentralizeVariablesModel> _centralizeVariablesModel;
        private readonly ResponseModel _response;
        private readonly ICardTokenizationDataHelper _cardTokenizationHelper;

        private readonly IProcessCcPayment _usaEPay;
        private readonly PostPaymentA _postPaymentAHelper;
        private readonly ISetCCPayment _setCcPayment;

        //
        private readonly IAddNotesV3 _addNotes;
        private readonly GatewaySelectionHelper _gatewaySelectionHelper;
        private readonly Sp_larry_cc_postdateV2 _Sp_larry_cc_postdateV2;

        public UsaEPayManager(HttpClient clientForInstaMed,
            IOptions<CentralizeVariablesModel> centralizeVariablesModel,
            ICardTokenizationDataHelper cardTokenizationHelper, ResponseModel response,
             PostPaymentA postPaymentAHelper, IProcessCcPayment usaEPay, ISetCCPayment setCcPayment, IAddNotesV3 addNotes,
             GatewaySelectionHelper gatewaySelectionHelper, Sp_larry_cc_postdateV2 sp_larry_cc_postdateV2)
        {

            _cardTokenizationHelper = cardTokenizationHelper;
            _response = response;
            _centralizeVariablesModel = centralizeVariablesModel;
            _clientForInstaMed = clientForInstaMed;
            _clientForInstaMed.BaseAddress = new Uri(_centralizeVariablesModel.Value.InstaMedCredentials.BaseAddress);
            _postPaymentAHelper = postPaymentAHelper;
            _usaEPay = usaEPay;
            _setCcPayment = setCcPayment;
            _gatewaySelectionHelper = gatewaySelectionHelper;
            _addNotes = addNotes;
            _Sp_larry_cc_postdateV2 = sp_larry_cc_postdateV2;
        }




        public async Task<ResponseModel> ProcessPayment_old(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            ProcessCcPaymentRequestModel requestForUsaEPay = new()
            {
                ccNumber = request.ccNumber,
                amount = request.amount,
                cvv = request.cvv,
                debtorAcc = request.debtorAcct,
                expiredDate = request.expiredDate,
                hsa = request.hsa,
                numberOfPayments = request.numberOfPayments
            };

            //must be break the implementtaion 
            //var data = await _processCcPayment.ProcessCcPayment(requestForUsaEPay, "P");
            //
            var tokenizeDataJsonResult = _usaEPay.TokenizeCc(requestForUsaEPay.debtorAcc, requestForUsaEPay.ccNumber, requestForUsaEPay.expiredDate,
                requestForUsaEPay.hsa != null && (bool)requestForUsaEPay.hsa, environment).Result;
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
                 requestForUsaEPay.debtorAcc, request.cardHolderName, environment).Result.Data;

            //

            var json = JsonConvert.SerializeObject(processTransactionJsonResult, Formatting.Indented);

            var obj = JsonConvert.DeserializeObject<SetProcessCCResponse.TransactionDetails>(json);
            var response = new CommonResponseModelForCCProcess()
            {
                AuthorizationNumber = obj.authcode,
                ResponseCode = obj.result_code,
                ResponseMessage = obj.result,
                TransactionId = obj.refnum
            };
            var noteText = "";
            if (response.ResponseCode == "A")
            {
                noteText = "USAEPAY CC APPROVED FOR $" + request.amount + " " +
                          response.ResponseMessage.ToUpper() +
                          " AUTH #:" + response.AuthorizationNumber;
            }
            else
            {
                noteText = "USAEPAY CC DECLINED FOR $" + request.amount + " " +
                                   response.ResponseMessage.ToUpper() +
                                   " AUTH #:" + response.AuthorizationNumber;
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcct,
                Employee = await _gatewaySelectionHelper.CcProcessEmployeeNumberAccordingToFlag(request.debtorAcct, environment),
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db
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
                CardHolderName = request.cardHolderName
            };

            //deactivated cause it's been separated in the save card info 
            //await _cardTokenizationHelper.CreateCardInfo(cardInfoObj, "P");


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
                var lcgPaymentScheduleObj = new LcgPaymentSchedule() //UCG_PaymentSchedule
                {
                    CardInfoId = paymentScheduleObj.CardInfoId,
                    EffectiveDate = paymentDate,
                    IsActive = true,
                    NumberOfPayments = i,
                    PatientAccount = paymentScheduleObj.PatientAccount,
                    Amount = paymentScheduleObj.Amount
                };
                await _cardTokenizationHelper.CreatePaymentSchedule(lcgPaymentScheduleObj, environment);

                if (i == 1)
                {
                    _USAePayPaymentScheduleId = lcgPaymentScheduleObj.Id;
                }

                paymentDate = paymentDate.AddMonths(1);

            }


            var paymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
            {
                ResponseCode = response.ResponseCode,
                AuthorizationNumber = response.AuthorizationNumber,
                AuthorizationText = "_username", //todo user name 
                ResponseMessage = response.ResponseMessage,
                PaymentScheduleId = _USAePayPaymentScheduleId,
                TransactionId = response.TransactionId,
                TimeLog = DateTime.Now
            };

            await _cardTokenizationHelper.CreatePaymentScheduleHistory(paymentScheduleHistoryObj, environment);

            if (response.ResponseCode == "A")
            {
                //cc payment insert 
                await _setCcPayment.SetCCPayment(new CcPaymnetRequestModel()
                {
                    debtorAcc = request.debtorAcct,
                    approvalCode = response.AuthorizationNumber,
                    approvalStatus = "APPROVED",
                    chargeTotal = (decimal)request.amount,
                    company = "AARGON AGENCY",
                    sif = request.sif,
                    paymentDate = DateTime.Now,
                    refNo = "USAEPAY2",
                    orderNumber = response.TransactionId,
                    userId = "WEB",
                    void_sale = "N"
                }, environment);
            }
            else
            {
                //cc payment insert 
                await _setCcPayment.SetCCPayment(new CcPaymnetRequestModel()
                {
                    debtorAcc = request.debtorAcct,
                    approvalCode = response.AuthorizationNumber,
                    approvalStatus = "DECLINED",
                    chargeTotal = (decimal)request.amount,
                    company = "AARGON AGENCY",
                    sif = request.sif,
                    paymentDate = DateTime.Now,
                    refNo = "USAEPAY2",
                    orderNumber = response.TransactionId,
                    userId = "WEB",
                    void_sale = "N"
                }, environment);
            }



            return _response.Response(true, true, response);
        }

        public async Task<ResponseModel> ProcessPayment(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            Serilog.Log.Information("Starting ProcessPayment for debtor account: {debtorAcc}", request.debtorAcct);

            ProcessCcPaymentRequestModel requestForUsaEPay = new()
            {
                ccNumber = request.ccNumber,
                amount = request.amount,
                cvv = request.cvv,
                debtorAcc = request.debtorAcct,
                expiredDate = request.expiredDate,
                hsa = request.hsa,
                numberOfPayments = request.numberOfPayments
            };

            Serilog.Log.Information("Created requestForUsaEPay object");


            //old
            //var tokenizeDataJsonResult = _usaEPay.TokenizeCc(requestForUsaEPay.debtorAcc, requestForUsaEPay.ccNumber, requestForUsaEPay.expiredDate,
            //    requestForUsaEPay.hsa != null && (bool)requestForUsaEPay.hsa, environment).Result;

            //new
            var tokenizeDataJsonResult = await _usaEPay.TokenizeCc(requestForUsaEPay.debtorAcc, requestForUsaEPay.ccNumber, requestForUsaEPay.expiredDate,
                requestForUsaEPay.hsa != null && (bool)requestForUsaEPay.hsa, environment);

            Serilog.Log.Information("Tokenization result received");

            SaveCard tokenizeCObj = new SaveCard()
            {
                CardNumber = "",
                Key = "",
                Type = ""
            };

            if (tokenizeDataJsonResult.Data != null)
            {

                tokenizeCObj = JsonConvert.DeserializeObject<SaveCard>(tokenizeDataJsonResult.Data.ToString() ?? string.Empty);
                Serilog.Log.Information("Tokenization complete for processing payments key: {key}, card Type: {type}", tokenizeCObj.Key, tokenizeCObj.Type);
            }

            var processTransactionJsonResult = await _usaEPay.ProcessingTransactionV2(tokenizeCObj.Key,
                (decimal)requestForUsaEPay.amount, requestForUsaEPay.hsa != null && (bool)requestForUsaEPay.hsa,
                requestForUsaEPay.debtorAcc, request.cardHolderName, environment);

            Serilog.Log.Information("Transaction details just after processing: {processTransactionJsonResult}", processTransactionJsonResult.Data);

            var json = JsonConvert.SerializeObject(processTransactionJsonResult.Data, Formatting.Indented);
            var obj = JsonConvert.DeserializeObject<SetProcessCCResponse.TransactionDetails>(json);

            if (obj.result_code != "A")
            {
                Serilog.Log.Warning("Payment request for {debtorAcct} has failed with error code {error_code}.  The message is:  {error}", request.debtorAcct, obj.error_code, obj.error);
            }

            var response = new CommonResponseModelForCCProcess()
            {
                AuthorizationNumber = obj.authcode,
                ResponseCode = obj.result_code,
                ResponseMessage = obj.result,
                TransactionId = obj.refnum
            };

            Serilog.Log.Information("Response created with AuthorizationNumber: {AuthorizationNumber}, ResponseCode: {ResponseCode}", response.AuthorizationNumber, response.ResponseCode);

            var noteText = "";
            if (response.ResponseCode == "A")
            {
                noteText = "USAEPAY CC APPROVED FOR $" + request.amount + " " +
                           response.ResponseMessage.ToUpper() +
                           " AUTH #:" + response.AuthorizationNumber;
            }
            else
            {
                noteText = "USAEPAY CC DECLINED FOR $" + request.amount + " " +
                           response.ResponseMessage.ToUpper() +
                           " AUTH #:" + response.AuthorizationNumber;
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcct,
                Employee = await _gatewaySelectionHelper.CcProcessEmployeeNumberAccordingToFlag(request.debtorAcct, environment),
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null
            };

            Serilog.Log.Information("Creating note for debtor account: {debtorAcc}", request.debtorAcct);
            await _addNotes.CreateNotes(noteObj, environment);

            var ccNumber = requestForUsaEPay.ccNumber;
            var expSplit = request.expiredDate.Split("/");
            var cardInfoObj = new LcgCardInfo()
            {
                IsActive = true,
                EntryMode = "key",
                BinNumber = requestForUsaEPay.cvv,
                ExpirationMonth = Convert.ToInt32(expSplit[0]),
                ExpirationYear = Convert.ToInt32(expSplit[1]),
                LastFour = ccNumber.Substring(ccNumber.Length - 4),
                PaymentMethodId = tokenizeCObj.Key,
                Type = tokenizeCObj.Type,
                AssociateDebtorAcct = requestForUsaEPay.debtorAcc,
                CardHolderName = request.cardHolderName
            };

            Serilog.Log.Information("Card info object created for debtor account: {debtorAcc}", requestForUsaEPay.debtorAcc);

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

            //automated postdate from old website 
            if (requestForUsaEPay.numberOfPayments > 1)
            {
                await _Sp_larry_cc_postdateV2.PostDateCCProcess(request.debtorAcct, DateTime.Now, Convert.ToDouble(request.amount), obj.refnum
                    , "", "", "", Convert.ToInt32(request.numberOfPayments) - 1, environment);
            }


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

                Serilog.Log.Information("Creating payment schedule for payment number: {paymentNumber}", i);
                await _cardTokenizationHelper.CreatePaymentSchedule(lcgPaymentScheduleObj, environment);

                if (i == 1)
                {
                    _USAePayPaymentScheduleId = lcgPaymentScheduleObj.Id;
                }

                paymentDate = paymentDate.AddMonths(1);
            }

            var paymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
            {
                ResponseCode = response.ResponseCode,
                AuthorizationNumber = response.AuthorizationNumber,
                AuthorizationText = "_username", //todo user name 
                ResponseMessage = response.ResponseMessage,
                PaymentScheduleId = _USAePayPaymentScheduleId,
                TransactionId = response.TransactionId,
                TimeLog = DateTime.Now
            };

            Serilog.Log.Information("Creating payment schedule history for PaymentScheduleId: {PaymentScheduleId}", _USAePayPaymentScheduleId);
            await _cardTokenizationHelper.CreatePaymentScheduleHistory(paymentScheduleHistoryObj, environment);

            if (response.ResponseCode == "A")
            {
                Serilog.Log.Information("Payment approved, setting CC payment for debtor account: {debtorAcc}", request.debtorAcct);
                await _setCcPayment.SetCCPayment(new CcPaymnetRequestModel()
                {
                    debtorAcc = request.debtorAcct,
                    approvalCode = response.AuthorizationNumber,
                    approvalStatus = "APPROVED",
                    chargeTotal = (decimal)request.amount,
                    company = "AARGON AGENCY",
                    sif = request.sif,
                    paymentDate = DateTime.Now,
                    refNo = "USAEPAY2",
                    orderNumber = response.TransactionId,
                    userId = "WEB",
                    void_sale = "N"
                }, environment);
            }
            else
            {
                Serilog.Log.Information("Payment declined, setting CC payment for debtor account: {debtorAcc}", request.debtorAcct);
                await _setCcPayment.SetCCPayment(new CcPaymnetRequestModel()
                {
                    debtorAcc = request.debtorAcct,
                    approvalCode = response.AuthorizationNumber,
                    approvalStatus = "DECLINED",
                    chargeTotal = (decimal)request.amount,
                    company = "AARGON AGENCY",
                    sif = request.sif,
                    paymentDate = DateTime.Now,
                    refNo = "USAEPAY2",
                    orderNumber = response.TransactionId,
                    userId = "WEB",
                    void_sale = "N"
                }, environment);
            }

            Serilog.Log.Information("ProcessPayment completed for debtor account: {debtorAcc}", request.debtorAcct);
            return _response.Response(true, true, response);
        }


        public async Task<ResponseModel> SaveCardInfo_old(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            ProcessCcPaymentRequestModel requestForUsaEPay = new()
            {
                ccNumber = request.ccNumber,
                amount = request.amount,
                cvv = request.cvv,
                debtorAcc = request.debtorAcct,
                expiredDate = request.expiredDate,
                hsa = request.hsa,
                numberOfPayments = request.numberOfPayments
            };

            //must be break the implementtaion 
            //var data = await _processCcPayment.ProcessCcPayment(requestForUsaEPay, "P");
            //
            var tokenizeDataJsonResult = _usaEPay.TokenizeCc(requestForUsaEPay.debtorAcc, requestForUsaEPay.ccNumber, requestForUsaEPay.expiredDate,
                requestForUsaEPay.hsa != null && (bool)requestForUsaEPay.hsa, environment).Result;
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
                CardHolderName = request.cardHolderName
            };


            await _cardTokenizationHelper.CreateCardInfo(cardInfoObj, environment);



            return _response.Response(true, true, "Card saved successfully");

        }

        public async Task<ResponseModel> SaveCardInfo(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            Serilog.Log.Information("Starting SaveCardInfo for debtor account: {debtorAcc}", request.debtorAcct);

            ProcessCcPaymentRequestModel requestForUsaEPay = new()
            {
                ccNumber = request.ccNumber,
                amount = request.amount,
                cvv = request.cvv,
                debtorAcc = request.debtorAcct,
                expiredDate = request.expiredDate,
                hsa = request.hsa,
                numberOfPayments = request.numberOfPayments
            };

            Serilog.Log.Information("Created requestForUsaEPay object");

            var tokenizeDataJsonResult = await _usaEPay.TokenizeCc(requestForUsaEPay.debtorAcc, requestForUsaEPay.ccNumber, requestForUsaEPay.expiredDate,
                requestForUsaEPay.hsa != null && (bool)requestForUsaEPay.hsa, environment);

            Serilog.Log.Information("Tokenization result received");

            SaveCard tokenizeCObj = new SaveCard()
            {
                CardNumber = "",
                Key = "",
                Type = ""
            };

            if (tokenizeDataJsonResult.Data != null)
            {
                tokenizeCObj = JsonConvert.DeserializeObject<SaveCard>(tokenizeDataJsonResult.Data.ToString() ?? string.Empty);
                Serilog.Log.Information("Tokenization complete for processing payments key: {key}, card Type: {type}", tokenizeCObj.Key, tokenizeCObj.Type);
            }

            var ccNumber = requestForUsaEPay.ccNumber;

            var cardInfoObj = new LcgCardInfo()
            {
                IsActive = true,
                EntryMode = "key",
                BinNumber = requestForUsaEPay.cvv,
                ExpirationMonth = 1, //todo 
                ExpirationYear = 2, //todo
                LastFour = ccNumber.Substring(ccNumber.Length - 4),
                PaymentMethodId = tokenizeCObj.Key,
                Type = tokenizeCObj.Type,
                AssociateDebtorAcct = requestForUsaEPay.debtorAcc,
                CardHolderName = request.cardHolderName
            };

            Serilog.Log.Information("Card info object created for debtor account: {debtorAcc}", requestForUsaEPay.debtorAcc);

            await _cardTokenizationHelper.CreateCardInfo(cardInfoObj, environment);

            Serilog.Log.Information("Card info saved successfully for debtor account: {debtorAcc}", requestForUsaEPay.debtorAcc);

            return _response.Response(true, true, "Card saved successfully");
        }
    }
}
