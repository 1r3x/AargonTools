using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.Models.Helper;
using AargonTools.ViewModel;
using Microsoft.Extensions.Options;

namespace AargonTools.Manager
{
    public class UniversalCcProcessApiService : IUniversalCcProcessApiService
    {
        private static HttpClient _clientForInstaMed = new();
        private readonly IOptions<CentralizeVariablesModel> _centralizeVariablesModel;
        //private readonly IUniversalCcProcessHelper _ccProcessHelper;
        //



        private static ResponseModel _response;
        private static IAddNotesV3 _addNotes;
        private static IAddCcPaymentV2 _addCcPayment;
        private static ICardTokenizationDataHelper _cardTokenizationHelper;
        private static ICryptoGraphy _crypto;
        //

        public UniversalCcProcessApiService(HttpClient clientForInstaMed, IOptions<CentralizeVariablesModel> centralizeVariablesModel, IAddNotesV3 addNotes, IAddCcPaymentV2 addCcPayment,
            ICardTokenizationDataHelper cardTokenizationHelper, ICryptoGraphy crypto, ResponseModel response)
        {
            _addCcPayment = addCcPayment;
            _addNotes = addNotes;
            _cardTokenizationHelper = cardTokenizationHelper;
            _crypto = crypto;
            _response = response;
            //
            _centralizeVariablesModel = centralizeVariablesModel;
            _clientForInstaMed = clientForInstaMed;
            _clientForInstaMed.BaseAddress = new Uri(_centralizeVariablesModel.Value.InstaMedCredentials.BaseAddress);
            _clientForInstaMed.DefaultRequestHeaders.Add("api-key", _centralizeVariablesModel.Value.InstaMedCredentials.APIkey);
            _clientForInstaMed.DefaultRequestHeaders.Add("api-secret", _centralizeVariablesModel.Value.InstaMedCredentials.APIsecret);
            _clientForInstaMed.DefaultRequestHeaders.Accept.Clear();
            _clientForInstaMed.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<string> InstaMedSale(SaleRequestModelForInstamed request)
        {
            var response = await _clientForInstaMed.PostAsJsonAsync(
                "rest/payment/sale", request);
            var resultString = response.Content.ReadAsStringAsync();
            //var ensureSuccessStatusCode = response.EnsureSuccessStatusCode();
            return resultString.Result;
        }

        public async Task<string> InstaMedAuth(SaleRequestModelForInstamed request)
        {
            var response = await _clientForInstaMed.PostAsJsonAsync(
                "rest/payment/auth", request);
            var resultString = response.Content.ReadAsStringAsync();
            return resultString.Result;
        }

        public async Task<string> InstaMedTokenization(SaleRequestModelForInstamed request)
        {
            var response = await _clientForInstaMed.PostAsJsonAsync(
                "rest/payment/paymentplan", request);
            var resultString = response.Content.ReadAsStringAsync();
            return resultString.Result;
        }

        public Task<string> IProGatewaySale(SaleRequestModelForInstamed request)
        {
            //setup variables
            string ccNumber = request.Card.CardNumber;
            string ccExp = request.Card.Expiration;
            string cvv = request.Card.CVN;
            string amount = Convert.ToString(request.Amount);
            String security_key = _centralizeVariablesModel.Value.IClassProCredentials.security_key;
            //String firstname = request.Patient.FirstName;
            //String lastname = request.Patient.LastName;

            String firstname = "First Name";
            String lastname = "Last Name";



            String strPost = "security_key=" + security_key
                                             + "&firstname=" + firstname + "&lastname=" + lastname
                                             + "&payment=creditcard&type=sale"
                                             + "&amount=" + amount + "&ccnumber=" + ccNumber + "&ccexp=" + ccExp + "&cvv=" + cvv;



            return ReadHtmlPageAsync(_centralizeVariablesModel.Value.IClassProCredentials.BaseAddress, strPost);
        }

        public Task<string> IProGatewayAuth(SaleRequestModelForInstamed request)
        {
            //setup variables
            string ccNumber = request.Card.CardNumber;
            string ccExp = request.Card.Expiration;
            string cvv = request.Card.CVN;
            string amount = Convert.ToString(request.Amount);
            String security_key = _centralizeVariablesModel.Value.IClassProCredentials.security_key;
            String firstname = request.Patient.FirstName;
            String lastname = request.Patient.LastName;


            String strPost = "security_key=" + security_key
                                             + "&firstname=" + firstname + "&lastname=" + lastname
                                             + "&payment=creditcard&type=auth"
                                             + "&amount=" + 1 + "&ccnumber=" + ccNumber + "&ccexp=" + ccExp + "&cvv=" + cvv;



            return ReadHtmlPageAsync(_centralizeVariablesModel.Value.IClassProCredentials.BaseAddress, strPost);
        }

        public async Task<string> ElavonSale(SaleRequestModelForInstamed request)
        {
            var url = _centralizeVariablesModel.Value.ElavonCredentials.BaseAddress;
            var result = "";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";


            httpRequest.ContentType = "application/x-www-form-urlencoded ";
            httpRequest.Accept = "application/xml";

            var data = "xmldata=" +
                       @"<txn>" +
                       "<ssl_merchant_id>" + _centralizeVariablesModel.Value.ElavonCredentials.ssl_merchant_id + "</ssl_merchant_id>" +
                       "<ssl_user_id>" + _centralizeVariablesModel.Value.ElavonCredentials.ssl_user_id + "</ssl_user_id>" +
                       "<ssl_pin>" + _centralizeVariablesModel.Value.ElavonCredentials.ssl_pin + "</ssl_pin>" +
                       "<ssl_description>Test Authorization for 1.00</ssl_description>" +
                       "<ssl_transaction_type>ccsale</ssl_transaction_type>" +
                       "<ssl_card_number>" + request.Card.CardNumber + "</ssl_card_number>" +
                       "<ssl_exp_date>" + request.Card.Expiration + "</ssl_exp_date>" +
                       "<ssl_amount>" + request.Amount + "</ssl_amount>" +
                       "<ssl_cvv2cvc2>" + request.Card.CVN + "</ssl_cvv2cvc2>" +
                       "<ssl_cardholder_ip>103.112.55.88</ssl_cardholder_ip>" +
                       "</txn>";






            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = await streamReader.ReadToEndAsync();
            }
            return result;
        }

        public async Task<string> ElavonAuth(SaleRequestModelForInstamed request)
        {
            var url = _centralizeVariablesModel.Value.ElavonCredentials.BaseAddress;
            var result = "";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.ContentType = "application/x-www-form-urlencoded ";
            httpRequest.Accept = "application/xml";

            var data = "xmldata=" +
                       "<txn>" +
                       "<ssl_merchant_id>" + _centralizeVariablesModel.Value.ElavonCredentials.ssl_merchant_id + "</ssl_merchant_id>" +
                       "<ssl_user_id>" + _centralizeVariablesModel.Value.ElavonCredentials.ssl_user_id + "</ssl_user_id>" +
                       "<ssl_pin>" + _centralizeVariablesModel.Value.ElavonCredentials.ssl_pin + " </ssl_pin>" +
                       "<ssl_transaction_type>ccauthonly</ssl_transaction_type>" +
                       "<ssl_description>Test Authorization for 1.00</ssl_description>" +
                       "<ssl_card_number>" + request.Card.CardNumber + "</ssl_card_number>" +
                       "<ssl_exp_date>" + request.Card.Expiration + "</ssl_exp_date>" +
                       "<ssl_amount>" + request.Amount + "</ssl_amount>" +
                       "<ssl_cvv2cvc2 >" + 123 + "</ssl_cvv2cvc2>" +
                       "<ssl_cardholder_ip >103.112.55.88</ssl_cardholder_ip>" +
                       "</txn>";

            using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            {
                streamWriter.Write(data);
            }

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = await streamReader.ReadToEndAsync();
            }

            return result;
        }

        public Task<string> ElavonTokenization(SaleRequestModelForInstamed request)
        {
            throw new NotImplementedException();
        }



        //


        private SaleResponseModelForInstamed _responseModelForInstamed;
        private SaleResponseModelForIProGateway _responseModelForIProGateway;
        private txn _deserializeObjForElavon;
        private int _lcgPaymentScheduleId;
        private readonly DateTime _scheduleDateTime = DateTime.Now;






        public async Task<string> ReadHtmlPageAsync(string url, string post)
        {
            String result = "";
            String strPost = post;
            StreamWriter myWriter = null;

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch (Exception e)
            {
                return e.Message;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr =
                   new StreamReader(objResponse.GetResponseStream()))
            {
                result = await sr.ReadToEndAsync();

                // Close and clean up the StreamReader
                sr.Close();
            }
            return result;
        }

        public async Task SaveCardInfoAndScheduleData(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                PaymentPlanType = "SaveOnFile",
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }
            };
            try
            {
                var resultVerify = await InstaMedTokenization(saleRequestModel);

                var cardInfoData = new TokenizeResponseModelForInstaMed(resultVerify);


                var cardInfoObj = new LcgCardInfo()
                {
                    IsActive = true,
                    EntryMode = cardInfoData.CardInfo.EntryMode,
                    BinNumber = cardInfoData.CardInfo.BinNumber,
                    ExpirationMonth = cardInfoData.CardInfo.ExpirationMonth,
                    ExpirationYear = cardInfoData.CardInfo.ExpirationYear,
                    LastFour = cardInfoData.CardInfo.LastFour,
                    PaymentMethodId = cardInfoData.CardInfo.PaymentMethodId,
                    Type = cardInfoData.CardInfo.Type,
                    AssociateDebtorAcct = request.debtorAcc,
                    CardHolderName = ""
                };


                await _cardTokenizationHelper.CreateCardInfo(cardInfoObj, environment);


                var paymentScheduleObj = new LcgPaymentSchedule()
                {
                    CardInfoId = cardInfoObj.Id,
                    IsActive = true,
                    EffectiveDate = _scheduleDateTime,
                    NumberOfPayments = (int)request.numberOfPayments,
                    PatientAccount = "", //todo patient account
                    Amount = request.amount
                };



                var paymentDate = paymentScheduleObj.EffectiveDate;
                for (var i = 1; i <= request.numberOfPayments; i++)
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
                        _lcgPaymentScheduleId = lcgPaymentScheduleObj.Id;
                    }

                    paymentDate = paymentDate.AddMonths(1);

                }


                var paymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
                {
                    ResponseCode = _responseModelForInstamed.ResponseCode,
                    AuthorizationNumber = _responseModelForInstamed.AuthorizationNumber,
                    AuthorizationText = "_username", //todo user name 
                    ResponseMessage = _responseModelForInstamed.ResponseMessage,
                    PaymentScheduleId = _lcgPaymentScheduleId,
                    TransactionId = _responseModelForInstamed.TransactionId
                };

                await _cardTokenizationHelper.CreatePaymentScheduleHistory(paymentScheduleHistoryObj, environment);

                if (_scheduleDateTime.Date == DateTime.Now.Date)
                {
                    await _cardTokenizationHelper.InactivePaymentSchedule(_lcgPaymentScheduleId, environment);
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        UserId = "_username", //todo user name
                        UserName = "_username" + " -LCG", //todo user name
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = _scheduleDateTime,
                        ApprovalStatus = "APPROVED",
                        BillingName = "", //todo billing person name 
                        ApprovalCode = _responseModelForInstamed.ResponseCode,
                        OrderNumber = _responseModelForInstamed.TransactionId,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }

            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<ResponseModel> ProcessSaleTransForInstaMed(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await InstaMedSale(saleRequestModel);

            if (resultVerify.Contains("FieldErrors"))
            {

                //todo  return the result of specific error
            }
            else
            {
                _responseModelForInstamed = new SaleResponseModelForInstamed(resultVerify);
            }

            string noteText = null;
            if (_responseModelForInstamed != null && _responseModelForInstamed.ResponseCode == "000")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _responseModelForInstamed.ResponseMessage.ToUpper() +
                           " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;

                //magic 
                await SaveCardInfoAndScheduleData(request, environment);
                // for success
                var ccPaymentObj = new CcPayment()
                {
                    DebtorAcct = request.debtorAcc,
                    Company = "TOTAL CREDIT RECOVERY",
                    //UserId = username,
                    UserId = "_username", //todo   
                    //UserName = username + " -LCG",
                    UserName = "_username", //todo 
                    ChargeTotal = request.amount,
                    Subtotal = request.amount,
                    PaymentDate = DateTime.Now,
                    ApprovalStatus = "APPROVED",
                    BillingName = "", // todo is it valid  for api transaction ? 
                    ApprovalCode = _responseModelForInstamed.ResponseCode,
                    OrderNumber = _responseModelForInstamed.TransactionId,
                    RefNumber = "INSTAMEDLH",
                    Sif = "N",
                    VoidSale = "N"
                };
                await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_responseModelForInstamed != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _responseModelForInstamed.ResponseMessage.ToUpper() +
                               " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;
                // for DECLINED
                if (_responseModelForInstamed != null)
                {
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = "_username", //todo   
                        //UserName = username + " -LCG",
                        UserName = "_username", //todo 
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _responseModelForInstamed.ResponseCode,
                        OrderNumber = _responseModelForInstamed.TransactionId,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N",
                        VoidSale = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            return _response.Response(true,true, _responseModelForInstamed);
        }

        public async Task<ResponseModel> ProcessCardAuthorizationForInstaMed(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await InstaMedAuth(saleRequestModel);

            if (resultVerify.Contains("FieldErrors"))
            {
            }
            else
            {
                _responseModelForInstamed = new SaleResponseModelForInstamed(resultVerify);
            }

            string noteText = null;
            if (_responseModelForInstamed != null && _responseModelForInstamed.ResponseCode == "000")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _responseModelForInstamed.ResponseMessage.ToUpper() +
                           " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;

                //magic 
                await SaveCardInfoAndScheduleData(request, environment);
                // for success
            }
            else
            {
                if (_responseModelForInstamed != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _responseModelForInstamed.ResponseMessage.ToUpper() +
                               " AUTH #:" + _responseModelForInstamed.AuthorizationNumber;
                // for DECLINED
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            return _response.Response(true,true, resultVerify);
        }




        public async Task SaveCardInfoAndScheduleDataForIProGateway(ProcessCcPaymentUniversalRequestModel request, string environment)
        {

            try
            {
                var ccNUmber = request.ccNumber;

                var (Key, IVBase64) = _crypto.InitSymmetricEncryptionKeyIv();

                var encryptedCC = _crypto.Encrypt(ccNUmber, IVBase64, Key);

                var cardInfoObj = new LcgCardInfo()
                {
                    IsActive = true,
                    EntryMode = "key",
                    BinNumber = request.cvv,
                    ExpirationMonth = 1,//todo 
                    ExpirationYear = 2,//todo
                    LastFour = ccNUmber.Substring(ccNUmber.Length - 4),
                    PaymentMethodId = encryptedCC,
                    Type = "VISA",
                    AssociateDebtorAcct = request.debtorAcc,
                    CardHolderName = ""
                };


                await _cardTokenizationHelper.CreateCardInfo(cardInfoObj, environment);


                var paymentScheduleObj = new LcgPaymentSchedule()
                {
                    CardInfoId = cardInfoObj.Id,
                    IsActive = true,
                    EffectiveDate = _scheduleDateTime,
                    NumberOfPayments = (int)request.numberOfPayments,
                    PatientAccount = "", //todo patient account
                    Amount = request.amount
                };



                var paymentDate = paymentScheduleObj.EffectiveDate;
                for (var i = 1; i <= request.numberOfPayments; i++)
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
                        _lcgPaymentScheduleId = lcgPaymentScheduleObj.Id;
                    }

                    paymentDate = paymentDate.AddMonths(1);

                }


                var paymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
                {
                    ResponseCode = _responseModelForInstamed.ResponseCode,
                    AuthorizationNumber = _responseModelForInstamed.AuthorizationNumber,
                    AuthorizationText = "_username", //todo user name 
                    ResponseMessage = _responseModelForInstamed.ResponseMessage,
                    PaymentScheduleId = _lcgPaymentScheduleId,
                    TransactionId = _responseModelForInstamed.TransactionId
                };

                await _cardTokenizationHelper.CreatePaymentScheduleHistory(paymentScheduleHistoryObj, environment);

                if (_scheduleDateTime.Date == DateTime.Now.Date)
                {
                    await _cardTokenizationHelper.InactivePaymentSchedule(_lcgPaymentScheduleId, environment);
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        UserId = "_username", //todo user name
                        UserName = "_username" + " -LCG", //todo user name
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = _scheduleDateTime,
                        ApprovalStatus = "APPROVED",
                        BillingName = "", //todo billing person name 
                        ApprovalCode = _responseModelForInstamed.ResponseCode,
                        OrderNumber = _responseModelForInstamed.TransactionId,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }

            }
            catch (Exception)
            {

                throw;
            }


        }

        public async Task<ResponseModel> ProcessSaleTransForIProGateway(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await IProGatewaySale(saleRequestModel);

            if (resultVerify.Contains("FieldErrors"))
            {
            }
            else
            {
                _responseModelForIProGateway = new SaleResponseModelForIProGateway(resultVerify);
            }

            string noteText = null;
            if (_responseModelForIProGateway != null && _responseModelForIProGateway.response_code == "000")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _responseModelForIProGateway.responsetext.ToUpper() +
                           " AUTH #:" + _responseModelForIProGateway.authcode;

                //magic 
                await SaveCardInfoAndScheduleDataForIProGateway(request, environment);
                // for success
                var ccPaymentObj = new CcPayment()
                {
                    DebtorAcct = request.debtorAcc,
                    Company = "TOTAL CREDIT RECOVERY",
                    //UserId = username,
                    UserId = "_username", //todo   
                    //UserName = username + " -LCG",
                    UserName = "_username", //todo 
                    ChargeTotal = request.amount,
                    Subtotal = request.amount,
                    PaymentDate = DateTime.Now,
                    ApprovalStatus = "APPROVED",
                    BillingName = "", // todo is it valid  for api transaction ? 
                    ApprovalCode = _responseModelForIProGateway.response_code,
                    OrderNumber = _responseModelForIProGateway.transactionid,
                    RefNumber = "INSTAMEDLH",
                    Sif = "N",
                    VoidSale = "N"
                };
                await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_responseModelForIProGateway != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _responseModelForIProGateway.responsetext.ToUpper() +
                               " AUTH #:" + _responseModelForIProGateway.authcode;
                // for DECLINED
                if (_responseModelForIProGateway != null)
                {
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = "_username", //todo   
                        //UserName = username + " -LCG",
                        UserName = "_username", //todo 
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _responseModelForIProGateway.response_code,
                        OrderNumber = _responseModelForIProGateway.transactionid,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N",
                        VoidSale = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            if (_responseModelForIProGateway != null)
            {
                var response = new CommonResponseModelForCCProcess()
                {
                    AuthorizationNumber = _responseModelForIProGateway.authcode,
                    ResponseCode = _responseModelForIProGateway.response_code,
                    ResponseMessage = _responseModelForIProGateway.responsetext,
                    TransactionId = _responseModelForIProGateway.transactionid
                };


                return _response.Response(true,true, response);
            }
            else
            {
                return _response.Response(true, "Oops! Something went wrong. ");
            }
        }

        public async Task<ResponseModel> ProcessCardAuthorizationForIProGateway(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await IProGatewayAuth(saleRequestModel);

            if (resultVerify.Contains("FieldErrors"))
            {
            }
            else
            {
                _responseModelForIProGateway = new SaleResponseModelForIProGateway(resultVerify);
            }

            string noteText = null;
            if (_responseModelForIProGateway != null && _responseModelForIProGateway.response_code == "000")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _responseModelForIProGateway.responsetext.ToUpper() +
                           " AUTH #:" + _responseModelForIProGateway.authcode;

                //magic 
                await SaveCardInfoAndScheduleData(request, environment);
                // for success
            }
            else
            {
                if (_responseModelForIProGateway != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _responseModelForIProGateway.responsetext.ToUpper() +
                               " AUTH #:" + _responseModelForIProGateway.authcode;
                // for DECLINED
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            return _response.Response(true, resultVerify);
        }



        public async Task SaveCardInfoAndScheduleDataForElavon(ProcessCcPaymentUniversalRequestModel request, string environment)
        {

            try
            {
                var ccNUmber = request.ccNumber;

                var (Key, IVBase64) = _crypto.InitSymmetricEncryptionKeyIv();

                var encryptedCC = _crypto.Encrypt(ccNUmber, IVBase64, Key);

                var cardInfoObj = new LcgCardInfo()
                {
                    IsActive = true,
                    EntryMode = "key",
                    BinNumber = request.cvv,
                    ExpirationMonth = 1,//todo 
                    ExpirationYear = 2,//todo
                    LastFour = ccNUmber.Substring(ccNUmber.Length - 4),
                    PaymentMethodId = encryptedCC,
                    Type = "VISA",
                    AssociateDebtorAcct = request.debtorAcc,
                    CardHolderName = ""
                };


                await _cardTokenizationHelper.CreateCardInfo(cardInfoObj, environment);


                var paymentScheduleObj = new LcgPaymentSchedule()
                {
                    CardInfoId = cardInfoObj.Id,
                    IsActive = true,
                    EffectiveDate = _scheduleDateTime,
                    NumberOfPayments = (int)request.numberOfPayments,
                    PatientAccount = "", //todo patient account
                    Amount = request.amount
                };



                var paymentDate = paymentScheduleObj.EffectiveDate;
                for (var i = 1; i <= request.numberOfPayments; i++)
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
                        _lcgPaymentScheduleId = lcgPaymentScheduleObj.Id;
                    }

                    paymentDate = paymentDate.AddMonths(1);

                }


                var paymentScheduleHistoryObj = new LcgPaymentScheduleHistory()
                {
                    ResponseCode = _deserializeObjForElavon.ssl_approval_code,
                    AuthorizationNumber = _deserializeObjForElavon.ssl_oar_data,
                    AuthorizationText = "_username", //todo user name 
                    ResponseMessage = _deserializeObjForElavon.ssl_result_message,
                    PaymentScheduleId = _lcgPaymentScheduleId,
                    TransactionId = _deserializeObjForElavon.ssl_txn_id
                };

                await _cardTokenizationHelper.CreatePaymentScheduleHistory(paymentScheduleHistoryObj, environment);

                if (_scheduleDateTime.Date == DateTime.Now.Date)
                {
                    //await _cardTokenizationHelper.InactivePaymentSchedule(_lcgPaymentScheduleId, environment);
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        UserId = "_username", //todo user name
                        UserName = "_username" + " -LCG", //todo user name
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = _scheduleDateTime,
                        ApprovalStatus = "APPROVED",
                        BillingName = "", //todo billing person name 
                        ApprovalCode = _deserializeObjForElavon.ssl_approval_code,
                        OrderNumber = _deserializeObjForElavon.ssl_txn_id,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }

            }
            catch (Exception)
            {

                throw;
            }


        }


        public async Task<ResponseModel> ProcessSaleTransForElavon(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await ElavonSale(saleRequestModel);
            if (resultVerify.Contains("FieldErrors"))
            {
            }
            else
            {
                var ser = new XmlSerializer(typeof(txn));

                using var sr = new StringReader(resultVerify);
                _deserializeObjForElavon = (txn)ser.Deserialize(sr);

            }

            string noteText = null;
            if (_deserializeObjForElavon != null && _deserializeObjForElavon.ssl_issuer_response == "00")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           _deserializeObjForElavon.ssl_result_message.ToUpper() +
                           " AUTH #:" + _deserializeObjForElavon.ssl_oar_data;

                //magic
                await SaveCardInfoAndScheduleDataForElavon(request, environment);
                // for success
                var ccPaymentObj = new CcPayment()
                {
                    DebtorAcct = request.debtorAcc,
                    Company = "TOTAL CREDIT RECOVERY",
                    //UserId = username,
                    UserId = "_username", //todo   
                    //UserName = username + " -LCG",
                    UserName = "_username", //todo 
                    ChargeTotal = request.amount,
                    Subtotal = request.amount,
                    PaymentDate = DateTime.Now,
                    ApprovalStatus = "APPROVED",
                    BillingName = "", // todo is it valid  for api transaction ? 
                    ApprovalCode = _deserializeObjForElavon.ssl_approval_code,
                    OrderNumber = _deserializeObjForElavon.ssl_txn_id,
                    RefNumber = "INSTAMEDLH",
                    Sif = "N",
                    VoidSale = "N"
                };
                await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_responseModelForInstamed != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               _deserializeObjForElavon.ssl_result_message.ToUpper() +
                               " AUTH #:" + _deserializeObjForElavon.ssl_oar_data;
                // for DECLINED
                if (_responseModelForInstamed != null)
                {
                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcc,
                        Company = "TOTAL CREDIT RECOVERY",
                        //UserId = username,
                        UserId = "_username", //todo   
                        //UserName = username + " -LCG",
                        UserName = "_username", //todo 
                        ChargeTotal = request.amount,
                        Subtotal = request.amount,
                        PaymentDate = DateTime.Now,
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _deserializeObjForElavon.ssl_approval_code,
                        OrderNumber = _deserializeObjForElavon.ssl_txn_id,
                        RefNumber = "INSTAMEDLH",
                        Sif = "N",
                        VoidSale = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db




            if (_deserializeObjForElavon != null)
            {
                var response = new CommonResponseModelForCCProcess()
                {
                    AuthorizationNumber = _deserializeObjForElavon.ssl_oar_data,
                    ResponseCode = _deserializeObjForElavon.ssl_approval_code,
                    ResponseMessage = _deserializeObjForElavon.ssl_result_message,
                    TransactionId = _deserializeObjForElavon.ssl_txn_id
                };


                return _response.Response(true,true, response);
            }
            else
            {
                return _response.Response(true, "Oops! Something went wrong. ");
            }
        }

        public async Task<ResponseModel> ProcessCardAuthorizationForElavon(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID
                },
                Amount = request.amount,
                PaymentMethod = "Card",
                Card = new Card()
                {
                    CVN = request.cvv,
                    CardNumber = request.ccNumber,
                    EntryMode = "key",
                    Expiration = request.expiredDate,
                    IsCardDataEncrypted = false,
                    IsEMVCapableDevice = false,
                }

            };
            var resultVerify = await ElavonAuth(saleRequestModel);
            var objs = new txn();
            if (resultVerify.Contains("FieldErrors"))
            {
            }
            else
            {
                var ser = new XmlSerializer(typeof(txn));

                using var sr = new StringReader(resultVerify);
                objs = (txn)ser.Deserialize(sr);
            }

            string noteText = null;
            if (objs != null && _responseModelForInstamed.ResponseCode == "00")
            {
                noteText = "INSTAMED CC APPROVED FOR $" + request.amount + " " +
                           objs.ssl_result_message.ToUpper() +
                           " AUTH #:" + objs.ssl_oar_data;

                //magic 
                await SaveCardInfoAndScheduleDataForElavon(request, environment);
                // for success
            }
            else
            {
                if (objs != null)
                    noteText = "INSTAMED CC DECLINED FOR $" + request.amount + " " +
                               objs.ssl_result_message.ToUpper().ToUpper() +
                               " AUTH #:" + objs.ssl_oar_data;
                // for DECLINED
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcc,
                Employee = 31950,
                ActivityCode = "RA",
                NoteText = noteText,
                Important = "N",
                ActionCode = null

            };

            await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db

            return _response.Response(true,true, resultVerify);
        }




    }
}
