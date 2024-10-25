using AargonTools.Interfaces;
using AargonTools.Interfaces.ProcessCC;
using AargonTools.Manager.GenericManager;
using AargonTools.Models.Helper;
using AargonTools.Models;
using AargonTools.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Linq;
using System.IO;
using System.Net;
using Microsoft.VisualBasic;
using System.Data;

namespace AargonTools.Manager.ProcessCCManager
{
    public class IProClassManager : IPaymentGateway
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
        private static GetTheCompanyFlag _getTheCompanyFlag;

        private readonly AdoDotNetConnection _adoConnection;
        //
        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ProdOldDbContext _contextProdOld;
        private static CurrentBackupTestEnvironmentDbContext _currentTestEnvironment;

        private static PostPaymentA _postPaymentAHelper;

        private static GatewaySelectionHelper _gatewaySelectionHelper;

        public IProClassManager(HttpClient clientForInstaMed,
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

        private SaleResponseModelForIProGateway _responseModelForIProGateway;
        private readonly DateTime _scheduleDateTime = DateTime.Now;
        private int _lcgPaymentScheduleId;

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

        public Task<string> IProGatewaySale(SaleRequestModelForInstamed request)
        {
            //setup variables
            string ccNumber = request.Card.CardNumber;
            string ccExp = request.Card.Expiration;
            string cvv = request.Card.CVN;
            string amount = Convert.ToString(request.Amount);
            String security_key = _centralizeVariablesModel.Value.IClassProCredentials.security_key;
            String firstname = request.Patient.FirstName;
            String lastname = request.Patient.LastName;
            String supplied_acct = request.Patient.AccountNumber;




            String strPost = "security_key=" + security_key
                                             + "&firstname=" + firstname + "&lastname=" + lastname
                                             + "&payment=creditcard&type=sale"
                                             + "&amount=" + amount + "&ccnumber=" + ccNumber + "&ccexp=" + ccExp + "&cvv=" + cvv
                                             + "&merchant_defined_field_1=" + supplied_acct + "&merchant_defined_field_2=" + firstname + "&merchant_defined_field_3=" + lastname;



            return ReadHtmlPageAsync(_centralizeVariablesModel.Value.IClassProCredentials.BaseAddress, strPost);
        }
        public Task<string> IProGatewaySalePro(SaleRequestModelForInstamed request)
        {
            //setup variables
            string ccNumber = request.Card.CardNumber;
            string ccExp = request.Card.Expiration;
            string cvv = request.Card.CVN;
            string amount = Convert.ToString(request.Amount);
            String security_key = _centralizeVariablesModel.Value.IClassProCredentials.security_keyPro;
            String firstname = request.Patient.FirstName;
            String lastname = request.Patient.LastName;
            String supplied_acct = request.Patient.AccountNumber;




            String strPost = "security_key=" + security_key
                                             + "&firstname=" + firstname + "&lastname=" + lastname
                                             + "&payment=creditcard&type=sale"
                                             + "&amount=" + amount + "&ccnumber=" + ccNumber + "&ccexp=" + ccExp + "&cvv=" + cvv
                                             + "&merchant_defined_field_1=" + supplied_acct + "&merchant_defined_field_2=" + firstname + "&merchant_defined_field_3=" + lastname;



            return ReadHtmlPageAsync(_centralizeVariablesModel.Value.IClassProCredentials.BaseAddress, strPost);
        }


        public async Task<ResponseModel> ProcessPayment(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
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
                },
                Patient = new Patient()
                {
                    AccountNumber = patientAccountInfo.SuppliedAcct,
                    FirstName = patientInfo.FirstName,
                    LastName = patientInfo.LastName
                }

            };
            string resultVerify;
            if (environment == "P")
            {
                resultVerify = await IProGatewaySalePro(saleRequestModel);
            }
            else
            {
                resultVerify = await IProGatewaySale(saleRequestModel);
            }


            if (resultVerify.Contains("FieldErrors"))
            {
                return _response.Response(true, true, resultVerify);
            }
            else
            {
                _responseModelForIProGateway = new SaleResponseModelForIProGateway(resultVerify);
            }

            string noteText = null;
            if (_responseModelForIProGateway != null && _responseModelForIProGateway.response_code == "100")
            {
                noteText = "ICLASSPRO CC APPROVED FOR $" + request.amount + " " +
                           _responseModelForIProGateway.responsetext.ToUpper() +
                           " AUTH #:" + _responseModelForIProGateway.authcode;



            }
            else
            {
                if (_responseModelForIProGateway != null)
                    noteText = "ICLASSPRO CC DECLINED FOR $" + request.amount + " " +
                               _responseModelForIProGateway.responsetext.ToUpper() +
                               " AUTH #:" + _responseModelForIProGateway.authcode;
                // for DECLINED
                if (_responseModelForIProGateway != null)
                {
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
                        ApprovalStatus = "DECLINED",
                        BillingName = "", // todo is it valid  for api transaction ? 
                        ApprovalCode = _responseModelForIProGateway.response_code,
                        OrderNumber = _responseModelForIProGateway.transactionid,
                        RefNumber = "ICLASSPRO",
                        Sif = "N",
                        VoidSale = "N"
                    };
                    await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
                }
            }

            var noteObj = new NoteMaster()
            {
                DebtorAcct = request.debtorAcct,
                Employee = await _gatewaySelectionHelper.CcProcessEmployeeNumberAccordingToFlag(request.debtorAcct,environment),
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


                return _response.Response(true, true, response);
            }
            else
            {
                return _response.Response(true, "Oops! Something went wrong. ");
            }
        }

        public async Task<ResponseModel> SaveCardInfo(ProcessCcPaymentUniversalRequestModel request, string environment)
        {
            try
            {
                var ccNUmber = request.ccNumber;

                var (Key, IVBase64) = _crypto.InitSymmetricEncryptionKeyIv();

                var encryptedCC = _crypto.Encrypt(ccNUmber, IVBase64, Key);

                var expSplit = request.expiredDate.Split("/");

                var cardInfoObj = new LcgCardInfo()
                {
                    IsActive = true,
                    EntryMode = "key",
                    BinNumber = request.cvv,
                    ExpirationMonth = Convert.ToInt32(expSplit[0]),
                    ExpirationYear = Convert.ToInt32(expSplit[1]),
                    LastFour = ccNUmber.Substring(ccNUmber.Length - 4),
                    PaymentMethodId = encryptedCC,
                    Type = "VISA",
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
                    ResponseCode = _responseModelForIProGateway.response_code,
                    AuthorizationNumber = _responseModelForIProGateway.authcode,
                    AuthorizationText = "_username", //todo user name 
                    ResponseMessage = _responseModelForIProGateway.cvvresponse,
                    PaymentScheduleId = _lcgPaymentScheduleId,
                    TransactionId = _responseModelForIProGateway.transactionid
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
                        ApprovalCode = _responseModelForIProGateway.response_code,
                        OrderNumber = _responseModelForIProGateway.transactionid,
                        RefNumber = "IPROGATEWAY",
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


                }

                return _response.Response(false, false, "dasdasd");
            }
            catch (Exception e)
            {
                return _response.Response(false, false, "dasdasd");
                throw;
            }
        }
    }
}
