﻿using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Interfaces.ProcessCC;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.Models.Helper;
using AargonTools.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AargonTools.Manager.ProcessCCManager
{
    public class TmcElavonManager : IPaymentGateway
    {

        private readonly HttpClient _clientForInstaMed = new();
        private readonly IOptions<CentralizeVariablesModel> _centralizeVariablesModel;
        private readonly ResponseModel _response;
        private readonly IAddNotesV3 _addNotes;
        private readonly IAddCcPaymentV2 _addCcPayment;
        private readonly ICardTokenizationDataHelper _cardTokenizationHelper;
        private readonly ICryptoGraphy _crypto;
        private readonly GetTheCompanyFlag _getTheCompanyFlag;

        private readonly AdoDotNetConnection _adoConnection;
        //
        private readonly ExistingDataDbContext _context;
        private readonly TestEnvironmentDbContext _contextTest;
        private readonly ProdOldDbContext _contextProdOld;
        private readonly CurrentBackupTestEnvironmentDbContext _currentTestEnvironment;

        private readonly PostPaymentA _postPaymentAHelper;
        private readonly GatewaySelectionHelper _gatewaySelectionHelper;

        public TmcElavonManager(HttpClient clientForInstaMed,
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
        private txn _deserializeObjForElavon;
        private SaleResponseModelForInstamed _responseModelForInstamed;
        private readonly DateTime _scheduleDateTime = DateTime.Now;
        private int _lcgPaymentScheduleId;



        public async Task<string> ElavonSaleTmcPro(SaleRequestModelForInstamed request)
        {
            var url = _centralizeVariablesModel.Value.ElavonCredentials.BaseAddressPro;
            var result = "";
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";


            httpRequest.ContentType = "application/x-www-form-urlencoded ";
            httpRequest.Accept = "application/xml";

            var data = "xmldata=" +
                       @"<txn>" +
                       "<ssl_merchant_id>" + request.Outlet.MerchantID + "</ssl_merchant_id>" +//
                       "<ssl_user_id>" + request.Outlet.StoreID + "</ssl_user_id>" +//StoreID is user_id
                       "<ssl_pin>" + request.Outlet.TerminalID + "</ssl_pin>" +//TerminalID is pin
                       "<ssl_description>" + request.Patient.FirstName + " " + request.Patient.LastName + "</ssl_description>" +
                       "<ssl_patient_account_number>" + request.Patient.AccountNumber + "</ssl_patient_account_number>" +//patirnt account no 
                       "<ssl_first_name>" + request.Patient.FirstName + " </ssl_first_name>" +//patirnt name  1st 
                       "<ssl_last_name>" + request.Patient.LastName + "</ssl_last_name>" +//patirnt name  Lst 
                       "<ssl_merchant_txn_id>" + request.Patient.AccountNumber + "</ssl_merchant_txn_id>" +//marchent txn 
                       "<ssl_avs_address>" + request.BillingAddress.Street1 + " " + request.BillingAddress.City + " " + request.BillingAddress.State + "</ssl_avs_address>" +//address 
                       "<ssl_avs_zip>" + request.BillingAddress.Zip + "</ssl_avs_zip>" +//zip
                       "<ssl_invoice_number>" + request.Patient.AccountNumber + "</ssl_invoice_number>" +//ssl_invoice_number
                       "<ssl_transaction_type>ccsale</ssl_transaction_type>" +
                       "<ssl_card_number>" + request.Card.CardNumber + "</ssl_card_number>" +
                       "<ssl_exp_date>" + request.Card.Expiration + "</ssl_exp_date>" +
                       "<ssl_amount>" + request.Amount + "</ssl_amount>" +
                       "<ssl_cvv2cvc2>" + request.Card.CVN + "</ssl_cvv2cvc2>" +
                       "</txn>";

            //var data = "xmldata=" +
            //          @"<txn>" +
            //          "<ssl_merchant_id>" + request.Outlet.MerchantID + "</ssl_merchant_id>" +//
            //          "<ssl_user_id>" + request.Outlet.StoreID + "</ssl_user_id>" +//StoreID is user_id
            //          "<ssl_pin>" + request.Outlet.TerminalID + "</ssl_pin>" +//TerminalID is pin
            //          "<ssl_description>staging to avoid null</ssl_description>" +
            //          "<ssl_patient_account_number>staging</ssl_patient_account_number>" +//patirnt account no 
            //          "<ssl_first_name>staging</ssl_first_name>" +//patirnt name  1st 
            //          "<ssl_last_name>staging</ssl_last_name>" +//patirnt name  Lst 
            //          "<ssl_merchant_txn_id>staging</ssl_merchant_txn_id>" +//marchent txn 
            //          "<ssl_avs_address>staging</ssl_avs_address>" +//address 
            //          "<ssl_avs_zip>staging</ssl_avs_zip>" +//zip
            //          "<ssl_invoice_number>staging</ssl_invoice_number>" +//ssl_invoice_number
            //          "<ssl_transaction_type>ccsale</ssl_transaction_type>" +
            //          "<ssl_card_number>" + request.Card.CardNumber + "</ssl_card_number>" +
            //          "<ssl_exp_date>" + request.Card.Expiration + "</ssl_exp_date>" +
            //          "<ssl_amount>" + request.Amount + "</ssl_amount>" +
            //          "<ssl_cvv2cvc2>" + request.Card.CVN + "</ssl_cvv2cvc2>" +
            //          "</txn>";




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
                       "<ssl_merchant_id>" + request.Outlet.MerchantID + "</ssl_merchant_id>" +//
                       "<ssl_user_id>" + request.Outlet.StoreID + "</ssl_user_id>" +//StoreID is user_id
                       "<ssl_pin>" + request.Outlet.TerminalID + "</ssl_pin>" +//TerminalID is pin
                       "<ssl_description>Description</ssl_description>" +
                       "<ssl_patient_account_number>1231213</ssl_patient_account_number>" +//patirnt account no 
                       "<ssl_first_name>First </ssl_first_name>" +//patirnt name  1st 
                       "<ssl_last_name>Last</ssl_last_name>" +//patirnt name  Lst 
                       "<ssl_merchant_txn_id>1231213</ssl_merchant_txn_id>" +//marchent txn 
                       "<ssl_avs_address>Address 1 </ssl_avs_address>" +//address 
                       "<ssl_avs_zip>12998</ssl_avs_zip>" +//zip
                       "<ssl_invoice_number>122998</ssl_invoice_number>" +//ssl_invoice_number
                       "<ssl_transaction_type>ccsale</ssl_transaction_type>" +
                       "<ssl_card_number>" + request.Card.CardNumber + "</ssl_card_number>" +
                       "<ssl_exp_date>" + request.Card.Expiration + "</ssl_exp_date>" +
                       "<ssl_amount>" + request.Amount + "</ssl_amount>" +
                       "<ssl_cvv2cvc2>" + request.Card.CVN + "</ssl_cvv2cvc2>" +
                       "</txn>";


            //var data = "xmldata=" +
            //          @"<txn>" +
            //          "<ssl_merchant_id>807485</ssl_merchant_id>" +//
            //          "<ssl_user_id>8032017132</ssl_user_id>" +
            //          "<ssl_pin>59WAMNKEGMMZVZARP37FUEST83WIOZYU6AJXKOMLHAZIIDP4LKGIZ65DDSSBMJ9M</ssl_pin>" +
            //          "<ssl_transaction_type>CCSALE</ssl_transaction_type>" +
            //          "<ssl_card_number>4000000000000002</ssl_card_number>" +
            //          "<ssl_exp_date>1222</ssl_exp_date>" +
            //          "<ssl_amount>100.00</ssl_amount>" +
            //          "<ssl_cvv2cvc2_indicator>1</ssl_cvv2cvc2_indicator>" +
            //          "<ssl_cvv2cvc2>123</ssl_cvv2cvc2>" +
            //          "<ssl_healthcare_amount>32.00</ssl_healthcare_amount>" +
            //          "</txn>";






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


        public async Task<ResponseModel> ProcessPayment(ProcessCcPaymentUniversalRequestModel request, string environment)
        {

            var resultVerify = "";

            var saleRequestModel = new SaleRequestModelForInstamed();
            if (environment == "P")
            {
                string ElavonclientUser = "tmcapi";
                string ElavonclientPass = "UY4BFNX26LEZF0LKBXG8N0GD7KZK04CAX47CUI0PQZ380YOZKGTOXJ21I4VGR9C3";



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


                saleRequestModel = new SaleRequestModelForInstamed()
                {
                    Outlet = new InstaMedOutlet()
                    {
                        MerchantID = "2110915",
                        StoreID = ElavonclientUser,
                        TerminalID = ElavonclientPass

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
                resultVerify = await ElavonSaleTmcPro(saleRequestModel);
            }
            else
            {
                saleRequestModel = new SaleRequestModelForInstamed()
                {
                    Outlet = new InstaMedOutlet()
                    {
                        MerchantID = _centralizeVariablesModel.Value.ElavonCredentials.ssl_merchant_id,
                        StoreID = _centralizeVariablesModel.Value.ElavonCredentials.ssl_user_id,
                        TerminalID = _centralizeVariablesModel.Value.ElavonCredentials.ssl_pin
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
                resultVerify = await ElavonSale(saleRequestModel);
            }


            if (resultVerify.Contains("FieldErrors"))
            {
                return _response.Response(true, true, resultVerify);
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
                noteText = "Converge CC APPROVED FOR $" + request.amount + " " +
                           _deserializeObjForElavon.ssl_result_message.ToUpper() +
                           " AUTH #:" + _deserializeObjForElavon.ssl_approval_code;


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
                    ApprovalCode = _deserializeObjForElavon.ssl_approval_code,
                    OrderNumber = _deserializeObjForElavon.ssl_txn_id,
                    RefNumber = "ELAVON",
                    Sif = request.sif,
                    VoidSale = "N"
                };
                await _addCcPayment.AddCcPayment(ccPaymentObj, environment); //PO for prod_old & T is for test_db
            }
            else
            {
                if (_responseModelForInstamed != null)
                    noteText = "Converge CC DECLINED FOR $" + request.amount + " " +
                               _deserializeObjForElavon.ssl_result_message.ToUpper() +
                               " AUTH #:" + _deserializeObjForElavon.ssl_approval_code;
                // for DECLINED
                if (_responseModelForInstamed != null)
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
                        ApprovalCode = _deserializeObjForElavon.ssl_approval_code,
                        OrderNumber = _deserializeObjForElavon.ssl_txn_id,
                        RefNumber = "ELAVON",
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




            if (_deserializeObjForElavon != null)
            {
                var response = new CommonResponseModelForCCProcess()
                {
                    AuthorizationNumber = _deserializeObjForElavon.ssl_oar_data,
                    ResponseCode = _deserializeObjForElavon.ssl_approval_code,
                    ResponseMessage = _deserializeObjForElavon.ssl_result_message,
                    TransactionId = _deserializeObjForElavon.ssl_txn_id
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
                    ResponseCode = _deserializeObjForElavon.ssl_approval_code,
                    //AuthorizationNumber = _deserializeObjForElavon.,
                    AuthorizationText = "_username", //todo user name 
                    ResponseMessage = _deserializeObjForElavon.ssl_result_message,
                    PaymentScheduleId = _lcgPaymentScheduleId,
                    TransactionId = _deserializeObjForElavon.ssl_txn_id
                };

                await _cardTokenizationHelper.CreatePaymentScheduleHistory(paymentScheduleHistoryObj, environment);





                if (_scheduleDateTime.Date == DateTime.Now.Date)
                {

                    _adoConnection.GetData("INSERT INTO cc_payment " +
                        "(debtor_acct,company, user_id,user_name,charge_total,subtotal,payment_date," +
                        "approval_status,approval_code, order_number,billing_name, ref_number, sif)" +
                        "VALUES('" + request.debtorAcct + "', 'TOTAL CREDIT RECOVERY', '_username', '_username elavon -API'," +
                        " " + request.amount + "," + request.amount + ", '" + _scheduleDateTime + "', 'APPROVED', '" + _deserializeObjForElavon.ssl_approval_code + "'," +
                        " '" + _deserializeObjForElavon.ssl_txn_id + "', '', 'INSTAMEDLH', 'N'); ", environment);
                }
                //new implementation 
                var companyFlag = await _getTheCompanyFlag.GetStringFlagForAdoQuery(request.debtorAcct, environment);

                var balance = _adoConnection.GetData("SELECT CAST(balance as float) as balance FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcct + "'", environment);

                var interestAmount = _adoConnection.GetData("SELECT interest_amt_life FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcct + "'", environment);

                var placements = _adoConnection.GetData("SELECT ISNULL((SELECT  placement FROM debtor_acct_info" + companyFlag + " WHERE debtor_acct='" + request.debtorAcct + "'),0) as placement", environment);
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

                    string paymentType = "DIRECT";//for elavon it's valid





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
