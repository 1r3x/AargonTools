using AargonTools.Interfaces;
using AargonTools.Interfaces.ProcessCC;
using AargonTools.Manager.GenericManager;
using AargonTools.Models.Helper;
using AargonTools.Models;
using AargonTools.ViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Microsoft.VisualBasic;
using System.Data;

namespace AargonTools.Manager.ProcessCCManager
{
    public class InstaMedManager : IPaymentGateway
    {
        private static HttpClient _clientForInstaMed = new();
        private static IOptions<CentralizeVariablesModel> _centralizeVariablesModel;
        private static ResponseModel _response;
        private static IAddNotesV3 _addNotes;
        private static IAddCcPaymentV2 _addCcPayment;
        private static ICardTokenizationDataHelper _cardTokenizationHelper;
        private static ICryptoGraphy _crypto;
        private static GetTheCompanyFlag _getTheCompanyFlag;

        private readonly AdoDotNetConnection _adoConnection;
        //
        private readonly ExistingDataDbContext _context;
        private readonly TestEnvironmentDbContext _contextTest;
        private readonly ProdOldDbContext _contextProdOld;
        private readonly CurrentBackupTestEnvironmentDbContext _currentTestEnvironment;

        private static PostPaymentA _postPaymentAHelper;
        //
        private static GatewaySelectionHelper _gatewaySelectionHelper;

        public InstaMedManager(HttpClient clientForInstaMed,
            IOptions<CentralizeVariablesModel> centralizeVariablesModel, IAddNotesV3 addNotes,
            IAddCcPaymentV2 addCcPayment,
            ICardTokenizationDataHelper cardTokenizationHelper, ICryptoGraphy crypto, ResponseModel response,
            AdoDotNetConnection adoConnection, GetTheCompanyFlag getTheCompanyFlag, ExistingDataDbContext context, TestEnvironmentDbContext contextTest, ProdOldDbContext contextProdOld,
            CurrentBackupTestEnvironmentDbContext currentTestEnvironment, PostPaymentA postPaymentAHelper, GatewaySelectionHelper gatewaySelectionHelper)
        {
            _addCcPayment = addCcPayment;
            _addNotes = addNotes;
            _cardTokenizationHelper = cardTokenizationHelper;
            _crypto = crypto;
            _response = response;
            _adoConnection = adoConnection;
            _getTheCompanyFlag = getTheCompanyFlag;
            //
            _context = context;
            _contextTest = contextTest;
            _contextProdOld = contextProdOld;
            _currentTestEnvironment = currentTestEnvironment;
            //
            _centralizeVariablesModel = centralizeVariablesModel;
            _clientForInstaMed = clientForInstaMed;
            _clientForInstaMed.BaseAddress = new Uri(_centralizeVariablesModel.Value.InstaMedCredentials.BaseAddress);

            _postPaymentAHelper = postPaymentAHelper;
            _gatewaySelectionHelper = gatewaySelectionHelper;

        }

        private SaleResponseModelForInstamed _responseModelForInstamed;
        private SaleResponseModelForIProGateway _responseModelForIProGateway;
        private txn _deserializeObjForElavon;
        private int _lcgPaymentScheduleId;
        private readonly DateTime _scheduleDateTime = DateTime.Now;


        public async Task<string> InstaMedSale(SaleRequestModelForInstamed request)
        {
            _clientForInstaMed.DefaultRequestHeaders.Clear();
            _clientForInstaMed.DefaultRequestHeaders.Add("api-key",
               _centralizeVariablesModel.Value.InstaMedCredentials.APIkey);
            _clientForInstaMed.DefaultRequestHeaders.Add("api-secret",
                _centralizeVariablesModel.Value.InstaMedCredentials.APIsecret);
            _clientForInstaMed.DefaultRequestHeaders.Accept.Clear();
            _clientForInstaMed.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _clientForInstaMed.PostAsJsonAsync(
                "rest/payment/sale", request);
            var resultString = response.Content.ReadAsStringAsync();
            //var ensureSuccessStatusCode = response.EnsureSuccessStatusCode();
            return resultString.Result;
        }

        public async Task<string> InstaMedSalePro(SaleRequestModelForInstamed request)
        {
            _clientForInstaMed.DefaultRequestHeaders.Add("api-key",
                _centralizeVariablesModel.Value.InstaMedCredentials.APIkeyPro);
            _clientForInstaMed.DefaultRequestHeaders.Add("api-secret",
                _centralizeVariablesModel.Value.InstaMedCredentials.APIsecretPro);
            _clientForInstaMed.DefaultRequestHeaders.Accept.Clear();
            _clientForInstaMed.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await _clientForInstaMed.PostAsJsonAsync(
                "rest/payment/sale", request);
            var resultString = response.Content.ReadAsStringAsync();
            //var ensureSuccessStatusCode = response.EnsureSuccessStatusCode();
            return resultString.Result;
        }

        public async Task<string> InstaMedTokenization(SaleRequestModelForInstamed request)
        {

            var response = await _clientForInstaMed.PostAsJsonAsync(
                "rest/payment/paymentplan", request);
            var resultString = response.Content.ReadAsStringAsync();
            return resultString.Result;
        }

        public async Task<string> InstaMedTokenizationPro(SaleRequestModelForInstamed request)
        {

            var response = await _clientForInstaMed.PostAsJsonAsync(
                "rest/payment/paymentplan", request);
            var resultString = response.Content.ReadAsStringAsync();
            return resultString.Result;
        }



        public async Task<ResponseModel> ProcessPayment(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            var scheduleDateTime = DateTime.Now;//todo 
            var acctLimitTemp = request.debtorAcct.Split('-');
            var acctLimitCheck = Convert.ToInt64(acctLimitTemp[0] + acctLimitTemp[1]);

            var patientBalanceCheck = await _getTheCompanyFlag.GetFlagForDebtorAccount(request.debtorAcct, environment)
                .Result.Where(x => x.DebtorAcct == request.debtorAcct).Select(i =>
                       new DebtorAcctInfoT()
                       {
                           SuppliedAcct = i.SuppliedAcct,
                           Balance = i.Balance
                       }).SingleOrDefaultAsync();

            //if (Convert.ToDecimal(request.numberOfPayments) * Convert.ToDecimal(request.amount) <= patientBalanceCheck?.Balance)
            if (0 <= patientBalanceCheck?.Balance)  //for testing remove before publish
            {

                string merchantId;
                string storeId;
                string terminalId;
                if (environment == "P")
                {
                    merchantId = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantIDPro;

                    storeId = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID;

                    if (acctLimitCheck >= 4950000001 && acctLimitCheck < 4950999999 || acctLimitCheck >= 4984000001 && acctLimitCheck < 4984999999)
                    {
                        terminalId = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalIDProHB;
                    }
                    else
                    {
                        terminalId = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalIDProPB;
                    }

                }
                else
                {
                    merchantId = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID;
                    storeId = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID;
                    terminalId = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID;
                }
                //patient info
                var patientAccountInfo = await _getTheCompanyFlag.GetFlagForDebtorAccount(request.debtorAcct, environment).Result.Where(x => x.DebtorAcct == request.debtorAcct).Select(i =>
                      new DebtorAcctInfoT()
                      {
                          SuppliedAcct = i.SuppliedAcct,
                          Balance = i.Balance
                      }).SingleOrDefaultAsync();



                var patientInfo = await _context.PatientMasters.Where(x => x.DebtorAcct == request.debtorAcct).Select(i =>
                 new PatientMaster()
                 {
                     FirstName = i.FirstName,
                     LastName = i.LastName,
                     StateCode = i.StateCode,
                     Zip = i.Zip,
                     Address1 = i.Address1,
                     Address2 = i.Address2,
                     City = i.City
                 }).SingleOrDefaultAsync();
                //patient info end 
                var saleRequestModel = new SaleRequestModelForInstamed()
                {
                    Outlet = new InstaMedOutlet()
                    {
                        MerchantID = merchantId,
                        StoreID = storeId,
                        TerminalID = terminalId
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
                    },
                    Patient = new Patient()
                    {
                        AccountNumber = patientAccountInfo.SuppliedAcct,
                        FirstName = patientInfo.FirstName,
                        LastName = patientInfo.LastName
                    },
                    BillingAddress = new BillingAddress()
                    {
                        City = patientInfo.City,
                        State = patientInfo.StateCode,
                        Zip = patientInfo.Zip,
                        Street1 = patientInfo.Address1,
                        Street2 = patientInfo.Address2
                    }

                };
                string resultVerify;
                if (environment == "P")
                {
                    resultVerify = await InstaMedSalePro(saleRequestModel);
                }
                else
                {
                    resultVerify = await InstaMedSale(saleRequestModel);
                }

                if (resultVerify.Contains("FieldErrors"))
                {
                    //have to check the field error 
                    return _response.Response(true, false, resultVerify);

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


                    var ccPaymentObj = new CcPayment()
                    {
                        DebtorAcct = request.debtorAcct,
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
                        Sif = request.sif,
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
                            DebtorAcct = request.debtorAcct,
                            Company = "TOTAL CREDIT RECOVERY",
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
                    DebtorAcct = request.debtorAcct,
                    Employee = await _gatewaySelectionHelper.CcProcessEmployeeNumberAccordingToFlag(request.debtorAcct, environment),
                    ActivityCode = "RA",
                    NoteText = noteText,
                    Important = "N",
                    ActionCode = null

                };

                await _addNotes.CreateNotes(noteObj, environment); //PO for prod_old & T is for test_db



                return _response.Response(true, true, _responseModelForInstamed);
            }
            else
            {
                Serilog.Log.Information("ProcessCcV2 => POST request received with Debtor Account: {@debtorAcct} and" +
                    " Total amount to be paid multiplying the number of payments by the amount of each payment is" +
                    " bigger than the customer’s available balance", request.debtorAcct);
                return _response.Response(false, false, "Total amount to be paid multiplying the number " +
                    "of payments by the amount of each payment is bigger than the customer’s available balance");
            }
        }


        public async Task<ResponseModel> SaveCardInfo(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            string merchantId;
            string storeId;
            string terminalId;
            if (environment == "P")
            {
                merchantId = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantIDPro;

                storeId = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID;
                var acctLimitTemp = request.debtorAcct.Split('-');
                var acctLimitCheck = Convert.ToInt64(acctLimitTemp[0] + acctLimitTemp[1]);
                if (acctLimitCheck >= 4950000001 && acctLimitCheck < 4950999999 || acctLimitCheck >= 4984000001 && acctLimitCheck < 4984999999)
                {
                    terminalId = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalIDProHB;
                }
                else
                {
                    terminalId = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalIDProPB;
                }

            }
            else
            {
                merchantId = _centralizeVariablesModel.Value.InstaMedOutlet.MerchantID;
                storeId = _centralizeVariablesModel.Value.InstaMedOutlet.StoreID;
                terminalId = _centralizeVariablesModel.Value.InstaMedOutlet.TerminalID;
            }


            var saleRequestModel = new SaleRequestModelForInstamed()
            {
                Outlet = new InstaMedOutlet()
                {
                    MerchantID = merchantId,
                    StoreID = storeId,
                    TerminalID = terminalId
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
                string resultVerify;
                if (environment == "P")
                {
                    resultVerify = await InstaMedTokenizationPro(saleRequestModel);
                }
                else
                {
                    resultVerify = await InstaMedTokenization(saleRequestModel);
                }

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
                    AssociateDebtorAcct = request.debtorAcct,
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



                //--------------new logic on 7 nov 24



                //var patientBalanceCheck = await _getTheCompanyFlag.GetFlagForDebtorAccount(request.debtorAcct, environment)
                //.Result.Where(x => x.DebtorAcct == request.debtorAcct).Select(i =>
                //       new DebtorAcctInfoT()
                //       {
                //           SuppliedAcct = i.SuppliedAcct,
                //           Balance = i.Balance
                //       }).SingleOrDefaultAsync();

                //if (request.numberOfPayments * request.amount > patientBalanceCheck.Balance)
                //{
                //    if (patientBalanceCheck.Balance % request.amount > 0)
                //    {
                //        // Adjust final payment amount to be the remainder of the balance after the prior payments
                //        for (var i = 1; i <= request.numberOfPayments; i++)
                //        {
                //            var lcgPaymentScheduleObj = new LcgPaymentSchedule()
                //            {
                //                CardInfoId = paymentScheduleObj.CardInfoId,
                //                EffectiveDate = paymentDate,
                //                IsActive = true,
                //                NumberOfPayments = i,
                //                PatientAccount = paymentScheduleObj.PatientAccount,
                //                Amount = paymentScheduleObj.Amount
                //            };
                //            await _cardTokenizationHelper.CreatePaymentSchedule(lcgPaymentScheduleObj, environment);

                //            if (i == 1)
                //            {
                //                _lcgPaymentScheduleId = lcgPaymentScheduleObj.Id;
                //            }

                //            paymentDate = paymentDate.AddMonths(1);

                //        }
                //        // Implement the logic to adjust the final payment
                //         var FinallcgPaymentScheduleObj = new LcgPaymentSchedule()
                //        {
                //            CardInfoId = paymentScheduleObj.CardInfoId,
                //            EffectiveDate = paymentDate,
                //            IsActive = true,
                //            NumberOfPayments = (int)(request.numberOfPayments+1),
                //            PatientAccount = paymentScheduleObj.PatientAccount,
                //            Amount = patientBalanceCheck.Balance % request.amount
                //         };
                //        await _cardTokenizationHelper.CreatePaymentSchedule(FinallcgPaymentScheduleObj, environment);
                //    }
                //    else
                //    {
                //        // Error - Overpayment
                //        throw new InvalidOperationException("Overpayment error: The total payment amount exceeds the balance.");
                //    }
                //}
                //else
                //{
                //    for (var i = 1; i <= request.numberOfPayments; i++)
                //    {
                //        var lcgPaymentScheduleObj = new LcgPaymentSchedule()
                //        {
                //            CardInfoId = paymentScheduleObj.CardInfoId,
                //            EffectiveDate = paymentDate,
                //            IsActive = true,
                //            NumberOfPayments = i,
                //            PatientAccount = paymentScheduleObj.PatientAccount,
                //            Amount = paymentScheduleObj.Amount
                //        };
                //        await _cardTokenizationHelper.CreatePaymentSchedule(lcgPaymentScheduleObj, environment);

                //        if (i == 1)
                //        {
                //            _lcgPaymentScheduleId = lcgPaymentScheduleObj.Id;
                //        }

                //        paymentDate = paymentDate.AddMonths(1);

                //    }
                //}



                //----------commented out as an old business logic 


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

                //----------commented out end

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
                        DebtorAcct = request.debtorAcct,
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


                //new implementation 
                var companyFlag = await _getTheCompanyFlag.GetStringFlagForAdoQuery(request.debtorAcct, environment);

                var balance = _adoConnection.GetData("SELECT CAST(balance as float) as balance FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcct + "'", environment); ;

                var interestAmount = _adoConnection.GetData("SELECT interest_amt_life FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcct + "'", environment); ;

                var placements = _adoConnection.GetData("SELECT ISNULL(placement,0) as placement FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcct + "'", environment);
                DataTable feePct = null;
                decimal feePctSimplified = 0;

                if (placements.Columns.Count > 1 && balance.Columns.Count > 1 && interestAmount.Columns.Count > 1)
                {

                    switch ((int)placements.Rows[0]["placement"])
                    {
                        case 1:
                            feePct = _adoConnection.GetData("SELECT commission_pct1 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcct, 4) + "'", environment);
                            feePctSimplified = (decimal)feePct.Rows[0]["commission_pct1"];
                            break;
                        case 2:
                            feePct = _adoConnection.GetData("SELECT commission_pct2 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcct, 4) + "'", environment);
                            feePctSimplified = (decimal)feePct.Rows[0]["commission_pct2"];
                            break;
                    }


                    switch ((int)placements.Rows[0]["placement"])
                    {
                        case 1:
                            feePct = _adoConnection.GetData("SELECT commission_pct1 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcct, 4) + "'", environment);
                            break;
                        case 2:
                            feePct = _adoConnection.GetData("SELECT commission_pct2 FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcct, 4) + "'", environment);
                            break;
                    }


                    var qFrom = _adoConnection.GetData("SELECT ISNULL(employee,0) as employee FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcct + "'", environment);
                    //for now
                    var qTo = _adoConnection.GetData("SELECT ISNULL(employee,0) as employee FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcct + "'", environment);

                    var remit = _adoConnection.GetData("SELECT remit_full_pmt FROM client_acct_info" + companyFlag + " WHERE client_acct='" + Strings.Left(request.debtorAcct, 4) + "'", environment);
                    var adminAmount = _adoConnection.GetData("SELECT CAST(total_fees_balance as float) as total_fees_balance FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcct + "'", environment);

                    string paymentType;
                    if (companyFlag == "_t")
                    {
                        paymentType = "DIRECT";
                    }
                    else
                    {
                        paymentType = "CREDIT CARD";
                    }




                    // debugger
                    var a = request.debtorAcct;
                    var b = request.amount;
                    var balanceT = balance.Rows[0]["balance"];
                    var interestAmountT = interestAmount.Rows[0]["interest_amt_life"];
                    var e = feePctSimplified;
                    var qFromT = qFrom.Rows[0]["employee"];
                    var qToT = qTo.Rows[0]["employee"];
                    var remitT = remit.Rows[0]["remit_full_pmt"];
                    var m = paymentType;
                    var j = companyFlag;
                    var adminAmountT = adminAmount.Rows[0]["total_fees_balance"];
                    var l = request.debtorAcct;
                    //
                    var balanceC = Convert.ToSingle(balanceT);
                    var interestAmountC = Convert.ToDecimal(interestAmountT);
                    var qFromC = Convert.ToInt32(qFromT);
                    var qToC = Convert.ToInt32(qToT);
                    var remitC = Convert.ToString(remitT);
                    var adminAmountC = Convert.ToSingle(adminAmountT);




                    await _postPaymentAHelper.Post(request.debtorAcct, request.amount, balanceC,
                        interestAmountC, feePctSimplified, request.sif,
                        qFromC, qToC, remitC,
                        paymentType, companyFlag, adminAmountC, request.debtorAcct, environment);
                    return _response.Response(false, false, "dasdasd");
                }

            }
            catch (Exception e)
            {
                Serilog.Log.Information(e.Message);
                throw;
            }

            return _response.Response(false, false, "dasdasd");
        }

    }


}
