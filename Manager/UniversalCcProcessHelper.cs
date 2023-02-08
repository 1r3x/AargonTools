using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.Models.Helper;
using AargonTools.ViewModel;
using Microsoft.Extensions.Options;

namespace AargonTools.Manager
{
    public class UniversalCcProcessHelper : IUniversalCcProcessHelper
    {
        private readonly IOptions<CentralizeVariablesModel> _centralizeVariablesModel;
        private readonly IUniversalCcProcessApiService _ccProcessApiService;
        private static ResponseModel _response;
        private static IAddNotesV3 _addNotes;
        private static IAddCcPaymentV2 _addCcPayment;
        private static ICardTokenizationDataHelper _cardTokenizationHelper;
        private static ICryptoGraphy _crypto;


        public UniversalCcProcessHelper(IOptions<CentralizeVariablesModel> centralizeVariablesModel,
            IUniversalCcProcessApiService ccProcessApiService,

            ResponseModel response, IAddNotesV3 addNotes, IAddCcPaymentV2 addCcPayment,
            ICardTokenizationDataHelper cardTokenizationHelper, ICryptoGraphy crypto, GatewaySelectionHelper gatewaySelectionHelper)
        {
            _centralizeVariablesModel = centralizeVariablesModel;
            _ccProcessApiService = ccProcessApiService;
            _response = response;
            _addNotes = addNotes;
            _addCcPayment = addCcPayment;
            _cardTokenizationHelper = cardTokenizationHelper;
            _crypto = crypto;
        }

        private SaleResponseModelForInstamed _responseModelForInstamed;
        private SaleResponseModelForIProGateway _responseModelForIProGateway;
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
                    MerchantID = _centralizeVariablesModel.Value.Outlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.Outlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.Outlet.TerminalID
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
            try
            {
                var resultVerify = await _ccProcessApiService.InstaMedTokenization(saleRequestModel);

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
                    MerchantID = _centralizeVariablesModel.Value.Outlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.Outlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.Outlet.TerminalID
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
            var resultVerify = await _ccProcessApiService.InstaMedSale(saleRequestModel);

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

            return _response.Response(true, resultVerify);
        }

        public async Task<ResponseModel> ProcessCardAuthorizationForInstaMed(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.Outlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.Outlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.Outlet.TerminalID
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
            var resultVerify = await _ccProcessApiService.InstaMedAuth(saleRequestModel);

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

            return _response.Response(true, resultVerify);
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
                    MerchantID = _centralizeVariablesModel.Value.Outlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.Outlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.Outlet.TerminalID
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
            var resultVerify = await _ccProcessApiService.IProGatewaySale(saleRequestModel);

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

            return _response.Response(true, resultVerify);
        }

        public async Task<ResponseModel> ProcessCardAuthorizationForIProGateway(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.Outlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.Outlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.Outlet.TerminalID
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
            var resultVerify = await _ccProcessApiService.IProGatewayAuth(saleRequestModel);

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

        public async Task<ResponseModel> ProcessSaleTransForElavon(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.Outlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.Outlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.Outlet.TerminalID
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
            var resultVerify = await _ccProcessApiService.ElavonSale(saleRequestModel);

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

            return _response.Response(true, resultVerify);
        }

        public async Task<ResponseModel> ProcessCardAuthorizationForElavon(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = _centralizeVariablesModel.Value.Outlet.MerchantID,
                    StoreID = _centralizeVariablesModel.Value.Outlet.StoreID,
                    TerminalID = _centralizeVariablesModel.Value.Outlet.TerminalID
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
            var resultVerify = await _ccProcessApiService.ElavonAuth(saleRequestModel);

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
                await SaveCardInfoAndScheduleDataForIProGateway(request, environment);
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

            return _response.Response(true, resultVerify);
        }
    }
}
