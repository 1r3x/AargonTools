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
        private static HttpClient _clientForInstaMed = new();
        private readonly IOptions<CentralizeVariablesModel> _centralizeVariablesModel;
        private static ResponseModel _response;
        private static ICardTokenizationDataHelper _cardTokenizationHelper;

        private static IProcessCcPayment _usaEPay;
        private static PostPaymentA _postPaymentAHelper;
        private readonly ISetCCPayment _setCcPayment;

        //
        private static IAddNotesV3 _addNotes;
        private static GatewaySelectionHelper _gatewaySelectionHelper;

        public UsaEPayManager(HttpClient clientForInstaMed,
            IOptions<CentralizeVariablesModel> centralizeVariablesModel,
            ICardTokenizationDataHelper cardTokenizationHelper, ResponseModel response,
             PostPaymentA postPaymentAHelper, IProcessCcPayment usaEPay, ISetCCPayment setCcPayment, IAddNotesV3 addNotes,
             GatewaySelectionHelper gatewaySelectionHelper)
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
        }




        public async Task<ResponseModel> ProcessPayment(ProcessCcPaymentUniversalRequestModel request, string environment)
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
            var tokenizeDataJsonResult = _usaEPay.TokenizeCc(requestForUsaEPay.debtorAcc,requestForUsaEPay.ccNumber, requestForUsaEPay.expiredDate,
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


        public async Task<ResponseModel> SaveCardInfo(ProcessCcPaymentUniversalRequestModel request, string environment)
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
    }
}
