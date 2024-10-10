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

        public UsaEPayManager(HttpClient clientForInstaMed,
            IOptions<CentralizeVariablesModel> centralizeVariablesModel,
            ICardTokenizationDataHelper cardTokenizationHelper, ResponseModel response,
             PostPaymentA postPaymentAHelper, IProcessCcPayment usaEPay, ISetCCPayment setCcPayment)
        {
            
            _cardTokenizationHelper = cardTokenizationHelper;
            _response = response;
            _centralizeVariablesModel = centralizeVariablesModel;
            _clientForInstaMed = clientForInstaMed;
            _clientForInstaMed.BaseAddress = new Uri(_centralizeVariablesModel.Value.InstaMedCredentials.BaseAddress);
            _postPaymentAHelper = postPaymentAHelper;
            _usaEPay = usaEPay;
            _setCcPayment = setCcPayment;
        }




        public async Task<ResponseModel> ProcessPayment(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            ProcessCcPaymentRequestModel requestForUsaEPay = new()
            {
                ccNumber = request.ccNumber,
                amount = request.amount,
                cvv = request.cvv,
                debtorAcc = request.debtorAcc,
                expiredDate = request.expiredDate,
                hsa = request.hsa,
                numberOfPayments = request.numberOfPayments
            };

            //must be break the implementtaion 
            //var data = await _processCcPayment.ProcessCcPayment(requestForUsaEPay, "P");
            //
            var tokenizeDataJsonResult = _usaEPay.TokenizeCc(requestForUsaEPay.ccNumber, requestForUsaEPay.expiredDate,
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
                 requestForUsaEPay.debtorAcc, request.cardHolderName, "P").Result.Data;

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
                var lcgPaymentScheduleObj = new LcgPaymentSchedule()
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
                ResponseCode = res.ResponseCode,
                AuthorizationNumber = res.AuthorizationNumber,
                AuthorizationText = "_username", //todo user name 
                ResponseMessage = res.ResponseMessage,
                PaymentScheduleId = _USAePayPaymentScheduleId,
                TransactionId = res.TransactionId,
                TimeLog = DateTime.Now
            };

            await _cardTokenizationHelper.CreatePaymentScheduleHistory(paymentScheduleHistoryObj, environment);


            //cc payment insert 
            await _setCcPayment.SetCCPayment(new CcPaymnetRequestModel()
            {
                debtorAcc = request.debtorAcc,
                approvalCode = "",
                approvalStatus = "APPROVED",
                chargeTotal = (decimal)request.amount,
                company = "AARGON AGENCY",
                sif = "Y",
                paymentDate = DateTime.Now,
                refNo = "USAEPAY2",
                orderNumber = res.TransactionId,
                userId = "WEB",
            }, "T");


            //

            var response = _response.Response(true, true, res);

            return _response.Response(true, true, response);
        }


        public async Task<ResponseModel> SaveCardInfo(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            ProcessCcPaymentRequestModel requestForUsaEPay = new()
            {
                ccNumber = request.ccNumber,
                amount = request.amount,
                cvv = request.cvv,
                debtorAcc = request.debtorAcc,
                expiredDate = request.expiredDate,
                hsa = request.hsa,
                numberOfPayments = request.numberOfPayments
            };

            //must be break the implementtaion 
            //var data = await _processCcPayment.ProcessCcPayment(requestForUsaEPay, "P");
            //
            var tokenizeDataJsonResult = _usaEPay.TokenizeCc(requestForUsaEPay.ccNumber, requestForUsaEPay.expiredDate,
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
