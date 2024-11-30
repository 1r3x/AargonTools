﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using AargonTools.Models.Helper;
using AargonTools.ViewModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace AargonTools.Manager
{
    public class ProcessCcPaymentManager : IProcessCcPayment
    {

        private readonly ExistingDataDbContext _context;
        private readonly TestEnvironmentDbContext _contextTest;
        private static ResponseModel _response;
        private readonly AdoDotNetConnection _adoConnection;
        private static IUserService _userService;
        private static ISetCCPayment _setCcPayment;//fro ccPayment Insert
        private static IAddNotes _addNotes;//fro notes Insert
        private static IOptions<CentralizeVariablesModel> _centralizeVariablesModel;
        private static GetTheCompanyFlag _companyFlag;

        public ProcessCcPaymentManager(ExistingDataDbContext context, ResponseModel response,
            TestEnvironmentDbContext contextTest, IAddNotes addNotes, AdoDotNetConnection adoConnection, IUserService userService,
            ISetCCPayment setCcPayment, IOptions<CentralizeVariablesModel> centralizeVariablesModel, GetTheCompanyFlag companyFlag)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _adoConnection = adoConnection;
            _userService = userService;
            _setCcPayment = setCcPayment;
            _addNotes = addNotes;
            _centralizeVariablesModel = centralizeVariablesModel;
            _companyFlag = companyFlag;
        }
        //models
        private class SaveCard
        {
            public string Type { get; set; }
            public string Key { get; set; }
            public string CardNumber { get; set; }
        }

        //for tokenize the cc
        public async Task<ResponseModel> TokenizeCc(string debtorAccount, string cardNo, string expireDate, bool hsa, string environment)
        {
            if (environment == "P")
            {
                string tempkey;
                string tempPin;
                if (await _context.ClientMasterPs.Where(x => x.ClientAcct == debtorAccount.Substring(0, 4)).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    tempkey = _centralizeVariablesModel.Value.USAePayDefault.KeyProd_Psuffex;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.PinProd;
                }
                else if (hsa == false)
                {
                    tempkey = _centralizeVariablesModel.Value.USAePayDefault.KeyProd;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.PinProd;
                }
                else
                {
                    tempkey = _centralizeVariablesModel.Value.USAePayDefault.KeyProdHSA;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.PinProd;

                }
                USAePay.API.SetURL(_centralizeVariablesModel.Value.USAePayDefault.UrlProd);
                USAePay.API.SetAuthentication(tempkey, tempPin);

                var creditCardObj = new Dictionary<string, object>();
                creditCardObj["number"] = cardNo;
                creditCardObj["expiration"] = expireDate;

                //creditCardObj["number"] = "4929000000006";
                //creditCardObj["expiration"] = "1221";

                var request = new Dictionary<string, object> { ["command"] = "cc:save", ["creditcard"] = creditCardObj };


                var tokenizeObjects = await USAePay.API.Transactions.PostAsync(request);

                string success = JsonConvert.SerializeObject(tokenizeObjects.ElementAt(5).Value, Formatting.Indented);
                if (success != "\"Approved\"") return _response.Response(null);
                var json = JsonConvert.SerializeObject(tokenizeObjects.ElementAt(8).Value, Formatting.Indented);
                return _response.Response(json);
            }
            else
            {
                string tempKey;
                string tempPin;
                if (await _context.ClientMasterPs.Where(x => x.ClientAcct == debtorAccount.Substring(0, 4)).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    tempKey = _centralizeVariablesModel.Value.USAePayDefault.Key;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.Pin;
                }
                else if (hsa == false)
                {
                    tempKey = _centralizeVariablesModel.Value.USAePayDefault.Key;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.Pin;
                }
                else
                {
                    tempKey = _centralizeVariablesModel.Value.USAePayDefault.Key;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.Pin;

                }
                USAePay.API.SetURL(_centralizeVariablesModel.Value.USAePayDefault.Url);
                USAePay.API.SetAuthentication(tempKey, tempPin);

                var creditCardObj = new Dictionary<string, object>();
                creditCardObj["number"] = cardNo;
                creditCardObj["expiration"] = expireDate;

                //creditCardObj["number"] = "4929000000006";
                //creditCardObj["expiration"] = "1221";

                var request = new Dictionary<string, object> { ["command"] = "cc:save", ["creditcard"] = creditCardObj };

                var tokenizeObjects = await USAePay.API.Transactions.PostAsync(request);

                string success = JsonConvert.SerializeObject(tokenizeObjects.ElementAt(5).Value, Formatting.Indented);
                if (success != "\"Approved\"") return _response.Response(null);
                var json = JsonConvert.SerializeObject(tokenizeObjects.ElementAt(8).Value, Formatting.Indented);
                return _response.Response(json);
            }

        }

        public async Task<ResponseModel> ProcessingTransaction(string tokenizeCc, decimal amount, bool hsa, string environment)
        {
            if (environment == "P")
            {
                string tempkey;
                string tempPin;
                if (hsa == false)
                {
                    tempkey = _centralizeVariablesModel.Value.USAePayDefault.KeyProd;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.PinProd;
                }
                else
                {
                    tempkey = _centralizeVariablesModel.Value.USAePayDefault.KeyProdHSA;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.PinProd;

                }
                USAePay.API.SetURL(_centralizeVariablesModel.Value.USAePayDefault.UrlProd);
                USAePay.API.SetAuthentication(tempkey, tempPin);

                var creditCardObj = new Dictionary<string, object> { ["number"] = tokenizeCc };

                var request = new Dictionary<string, object>
                {
                    ["command"] = "cc:sale",
                    ["amount"] = amount,
                    ["creditcard"] = creditCardObj
                };


                var processSaleObjects = await USAePay.API.Transactions.PostAsync(request);
                var json = JsonConvert.SerializeObject(processSaleObjects.Values, Formatting.Indented);

                if (json.Contains("Approved"))
                {
                    return _response.Response(processSaleObjects);
                }

                return _response.Response(processSaleObjects);
            }
            else
            {
                string tempKey;
                string tempPin;
                if (hsa == false)
                {
                    tempKey = _centralizeVariablesModel.Value.USAePayDefault.Key;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.Pin;
                }
                else
                {
                    tempKey = _centralizeVariablesModel.Value.USAePayDefault.Key;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.Pin;

                }
                USAePay.API.SetURL(_centralizeVariablesModel.Value.USAePayDefault.Url);
                USAePay.API.SetAuthentication(tempKey, tempPin);

                var creditCardObj = new Dictionary<string, object> { ["number"] = tokenizeCc };

                var request = new Dictionary<string, object>
                {
                    ["command"] = "cc:sale",
                    ["amount"] = amount,
                    ["creditcard"] = creditCardObj
                };


                var processSaleObjects = await USAePay.API.Transactions.PostAsync(request);
                var json = JsonConvert.SerializeObject(processSaleObjects.Values, Formatting.Indented);
                var test = json.Contains("Approved");
                if (json.Contains("Approved"))
                {
                    return _response.Response(processSaleObjects);
                }

                return _response.Response("Error 404");

            }

        }


        public async Task<ResponseModel> ProcessingTransactionV2(string tokenizeCc, decimal amount, bool hsa,
            string debtorAccount, string cardHolder, string environment)
        {
            

            if (environment == "P")
            {
                string tempkey;
                string tempPin;
                if (await _context.ClientMasterPs.Where(x => x.ClientAcct == debtorAccount.Substring(0, 4)).Select(x => x.ClientAcct).FirstOrDefaultAsync() != null)
                {
                    tempkey = _centralizeVariablesModel.Value.USAePayDefault.KeyProd_Psuffex;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.PinProd;
                }
                else if (hsa == false)
                {
                    tempkey = _centralizeVariablesModel.Value.USAePayDefault.KeyProd;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.PinProd;
                }
                else
                {
                    tempkey = _centralizeVariablesModel.Value.USAePayDefault.KeyProdHSA;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.PinProd;

                }

                //

                var patientAccountInfo = await _companyFlag.GetFlagForDebtorAccount(debtorAccount, environment)
                 .Result.Where(a => a.DebtorAcct == debtorAccount)
                 .Select(a => new
                 {
                     a.Balance,
                     a.SuppliedAcct,
                 }).SingleOrDefaultAsync();

                var patientInfo = await _companyFlag
                                        .GetFlagForDebtorMaster(debtorAccount, environment).Result
                                        .Where(x => x.DebtorAcct == debtorAccount).Select (x=>
                                        new DebtorMaster()
                                        {
                                            FirstName = x.FirstName,
                                            LastName = x.LastName,
                                            StateCode = x.StateCode,
                                            Zip = x.Zip,
                                            Address1 = x.Address1,
                                            Address2 = x.Address2,
                                            City = x.City,

                                        }).SingleOrDefaultAsync();


                //var patientInfo_fake = await _context.PatientMasters.Where(x => x.DebtorAcct == debtorAccount).Select(i =>
                // new PatientMaster()
                // {
                //     FirstName = i.FirstName,
                //     LastName = i.LastName,
                //     StateCode = i.StateCode,
                //     Zip = i.Zip,
                //     Address1 = i.Address1,
                //     Address2 = i.Address2,
                //     City = i.City,
                // }).SingleOrDefaultAsync();

                if (patientInfo == null)
                {
                    patientInfo = new DebtorMaster()
                    {
                        FirstName = "Not",
                        LastName = "Available",
                        StateCode = "",
                        Zip = "",
                        Address1 = "",
                        Address2 = "",
                        City = ""
                    };
                }
                var cardHolderTemp = cardHolder;
                if (cardHolder == "" || cardHolder == null)
                {
                    cardHolderTemp = patientInfo.FirstName + " " + patientInfo.LastName;
                }

                //
                USAePay.API.SetURL(_centralizeVariablesModel.Value.USAePayDefault.UrlProd);
                USAePay.API.SetAuthentication(tempkey, tempPin);
                var creditCardObj = new Dictionary<string, object> { ["number"] = tokenizeCc, ["cardholder"] = cardHolderTemp };
                var billingAddressObj = new Dictionary<string, object>
                {
                    ["firstname"] = patientInfo.FirstName,
                    ["lastname"] = patientInfo.LastName,
                    ["street"] = patientInfo.Address1,
                    ["city"] = patientInfo.City,
                    ["state"] = patientInfo.StateCode,
                    ["postalcode"] = ""
                };

                var request = new Dictionary<string, object>
                {
                    ["command"] = "cc:sale",
                    ["invoice"] = debtorAccount.Replace("-", ""),
                    ["customerid"] = debtorAccount.Replace("-", ""),
                    ["ponum"] = debtorAccount.Replace("-", ""),
                    ["orderid"] = debtorAccount.Replace("-", ""),
                    ["description"] = "Repayment of Debt for account " + debtorAccount.Replace("-", ""),
                    ["send_receipt"] = "false",
                    ["amount"] = amount,
                    ["creditcard"] = creditCardObj,
                    ["billing_address"] = billingAddressObj

                };


                var processSaleObjects = await USAePay.API.Transactions.PostAsync(request);
                var json = JsonConvert.SerializeObject(processSaleObjects.Values, Formatting.Indented);

                if (json.Contains("Approved"))
                {
                    return _response.Response(processSaleObjects);
                }

                return _response.Response(processSaleObjects);
            }
            else
            {
                string tempKey;
                string tempPin;

                if (hsa == false)
                {
                    tempKey = _centralizeVariablesModel.Value.USAePayDefault.Key;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.Pin;
                }
                else
                {
                    tempKey = _centralizeVariablesModel.Value.USAePayDefault.Key;
                    tempPin = _centralizeVariablesModel.Value.USAePayDefault.Pin;

                }
                USAePay.API.SetURL(_centralizeVariablesModel.Value.USAePayDefault.Url);
                USAePay.API.SetAuthentication(tempKey, tempPin);

                var creditCardObj = new Dictionary<string, object> { ["number"] = tokenizeCc };

                var request = new Dictionary<string, object>
                {
                    ["command"] = "cc:sale",
                    ["amount"] = amount,
                    ["creditcard"] = creditCardObj
                };


                var processSaleObjects = await USAePay.API.Transactions.PostAsync(request);
                var json = JsonConvert.SerializeObject(processSaleObjects.Values, Formatting.Indented);
                var test = json.Contains("Approved");
                if (json.Contains("Approved"))
                {
                    return _response.Response(processSaleObjects);
                }

                return _response.Response("Error 404");

            }

        }

        //no implementation for now....
        private async Task<ResponseModel> VoidTransaction(string transactionKey)
        {
            USAePay.API.SetURL(_centralizeVariablesModel.Value.USAePayDefault.Url, "v2");
            USAePay.API.SetAuthentication(_centralizeVariablesModel.Value.USAePayDefault.Key, _centralizeVariablesModel.Value.USAePayDefault.Pin);


            var request = new Dictionary<string, object>
            {
                ["command"] = "void",
                ["trankey"] = transactionKey
            };


            var voidTransactionObjects = await USAePay.API.Transactions.PostAsync(request);

            return _response.Response(voidTransactionObjects);
        }


        //for executing the sp_larry_cc_postdate
        public async Task<ResponseModel> SchedulePostData(string debtorAcct, DateTime postDate, decimal amount, string cardNumber, int numberOfPayments,
            string expMonth, string expYear, string environment)
        {
            var rawAdo = _adoConnection.GetData("DECLARE @return_value int EXEC " +
                                                "@return_value = [dbo].[sp_larry_cc_postdate]" +
                                                "@Debtor_Acct = N'" + debtorAcct + "'," +
                                                "@Post_Date = N'" + postDate + "'," +
                                                "@Amount = " + amount + "," +
                                                "@Card_Num = N'" + cardNumber + "'," +
                                                "@Exp_Month = N'" + expMonth + "'," +
                                                "@Exp_Year = N'" + expYear + "'," +
                                                "@Total_PD = " + numberOfPayments +
                                                "SELECT  'Return Value' = @return_value;", environment);
            await Task.CompletedTask;

            if (Convert.ToDecimal(rawAdo.Rows[0]["Return Value"]) == 0)
            {
                return _response.Response("Successfully Set Post Date Checks");
            }
            else
            {
                return _response.Response("Oops Something went wrong.");
            }


        }


        //for executing the sp_larry_cc_postdate
        public async Task<ResponseModel> SchedulePostDataV2(SchedulePostDateRequest request, string environment)
        {
            var rawAdo = _adoConnection.GetData("DECLARE @return_value int EXEC " +
                                                "@return_value = [dbo].[sp_larry_cc_postdate]" +
                                                "@Debtor_Acct = N'" + request.debtorAcct + "'," +
                                                "@Post_Date = N'" + request.postDate + "'," +
                                                "@Amount = " + request.amount + "," +
                                                "@Card_Num = N'" + request.cardNumber + "'," +
                                                "@Exp_Month = N'" + request.expMonth + "'," +
                                                "@Exp_Year = N'" + request.expYear + "'," +
                                                "@Total_PD = " + request.numberOfPayments +
                                                "SELECT  'Return Value' = @return_value;", environment);
            await Task.CompletedTask;

            if (Convert.ToDecimal(rawAdo.Rows[0]["Return Value"]) == 0)
            {
                return _response.Response(true, true, "Successfully Set Post Date Checks");
            }
            else
            {
                return _response.Response(true, false, "Oops Something went wrong.");
            }


        }

        //post payment
        private async Task<ResponseModel> PostPayment(string debtorAccount, decimal paymentAmount, DateTime tranDate, string tranDescription, decimal feePct, string environment)
        {
            try
            {

                var refreshClientFeePercentages = _adoConnection.GetData("SELECT cai.client_acct,commission_pct1" +
                                                                         " FROM client_master AS cm,client_acct_info AS cai" +
                                                                         " WHERE cai.client_acct = cm.client_acct AND acct_status = 'A' AND " +
                                                                         "cai.client_acct=" + debtorAccount.Substring(0, 4)
                    , environment);

                var codeMasterUpdate = _adoConnection.GetData("UPDATE code_master " +
                                                         "SET code_value = code_value + 1 " +
                                                         "OUTPUT inserted.code_value " +
                                                         "WHERE code_type = 'PAYMENT'"
           , environment);

                var debtorData = _adoConnection.GetData("SELECT * FROM debtor_acct_info WHERE debtor_acct = '" + debtorAccount + "'"
                    , environment);

                string statusCode = "";

                if ((Convert.ToDecimal(debtorData.Rows[0]["balance"]) - paymentAmount == 0))
                {
                    statusCode = "    'PIF',";
                }
                else
                {
                    statusCode = "    '" + debtorData.Rows[0]["status_code"] + "',";
                }


                var insertDebtorPayment = _adoConnection.GetData("INSERT INTO debtor_payment_master(" +
                                                                 "payment_code," +
                                                                 "debtor_acct," +
                                                                 "purge_flag," +
                                                                 "payment_type," +
                                                                 "payment_date," +
                                                                 "tran_date," +
                                                                 "total_payment_amt," +
                                                                 "client_amt," +
                                                                 "agency_amt_decl," +
                                                                 "agency_amt_not_decl," +
                                                                 "amt_decl_desc," +
                                                                 "interest_paid," +
                                                                 "admin_fees_paid," +
                                                                 "collection_fees_paid," +
                                                                 "costs_paid," +
                                                                 "attorney_fees_paid," +
                                                                 "damages_paid," +
                                                                 "return_check_fees_paid," +
                                                                 "balance," +
                                                                 "fee_pct," +
                                                                 "remit_full_pmt," +
                                                                 "check_num," +
                                                                 "sequence_num," +
                                                                 "employee," +
                                                                 "queue," +
                                                                 "status_code," +
                                                                 "show_on_invoice," +
                                                                 "rev_payment_code," +
                                                                 "vendor_code," +
                                                                 "coll_emp1," +
                                                                 "coll_pct1," +
                                                                 "coll_emp2," +
                                                                 "coll_pct2," +
                                                                 "comment) " +
                                                                 "SELECT " +
                                                                 codeMasterUpdate.Rows[0]["code_value"] + ","
                                                                 + debtorAccount + "," +
                                                                 "'N'," +
                                                                 "'DIRECT',"
                                                                 + DateTime.Today.ToString("MM/dd/yyyy") + "," +
                                                                 "GETDATE(),"
                                                                 + paymentAmount + "," +
                                                                 +paymentAmount + "," +
                                                                 "0," +
                                                                 "0," +
                                                                 "'PAYMENT'," +
                                                                 "0," +
                                                                 "0," +
                                                                 "0," +
                                                                 "0," +
                                                                 "0," +
                                                                 "0," +
                                                                 "0," +
                                                                 (Convert.ToDecimal(debtorData.Rows[0]["balance"]) - paymentAmount) + "," +
                                                                 (Convert.ToDecimal(refreshClientFeePercentages.Rows[0]["commission_pct1"])) + "," +
                                                                  "'Y'," +
                                                                  "NULL," +
                                                                  "NULL," +

                                                                  "1," +//_userService.GetLoginUserName() + "," +//user name from api but no implementation default 1 

                                                                 "'" + debtorData.Rows[0]["employee"] + "'," +//the queue set from the debtor account's employee 
                                                                  statusCode +
                                                                  "'Y'," +
                                                                  "NULL," +
                                                                  "'DEBTOR W/O PAYMENT ARRANGEMENTS'," +
                                                                  "NULL," +
                                                                  "NULL," +
                                                                  "NULL," +
                                                                  "NULL," +
                                                                  "'AUTOMATED PAYMENT POSTED FROM AMCP DIRECTS FILE'"
                    , environment);
                var updateDebtorLogic = "";
                var removeMults = false;
                if ((Convert.ToDecimal(debtorData.Rows[0]["balance"]) - paymentAmount) <= 0)
                {
                    updateDebtorLogic = "status_code = 'PIF'," +
                                        "acct_status = 'I'," +
                                        //commented out duo to debugger reports 
                                        //"date_inactivated = CONVERT(VARCHAR,GETDATE(),101)," +
                                        "date_inactivated = CONVERT(VARCHAR, GETDATE(), 101),";
                    removeMults = true;
                }

                var updateDebtorAccount = _adoConnection.GetData(
                    "UPDATE debtor_acct_info " +
                    "SET " +
                    "begin_age_date = '" + tranDate + "'," +
                    "date_last_accessed = GETDATE()," +
                    "payment_amt_life = payment_amt_life + " + paymentAmount + "," +
                    "last_payment_amt = " + paymentAmount + "," +
                    "balance = balance - " + paymentAmount + "," +
                    "out_of_statute = 'N'," +
                    updateDebtorLogic +
                    "activity_code = 'PM'" +
                    "WHERE debtor_acct = '" + debtorAccount + "'"
                    , environment);


                var updateClientAccount = _adoConnection.GetData(
                    "UPDATE client_acct_info " +
                    "SET payment_amt_life = payment_amt_life + " + paymentAmount +
                    "WHERE client_acct = '" + debtorAccount.Substring(0, 4) + "'"
                    , environment);




                var insertNotes = _adoConnection.GetData(
                    "INSERT INTO note_master " +
                    "(debtor_acct,note_date,employee,activity_code,note_text)" +
                    "SELECT '" + debtorAccount + "'," +
                    "    CONVERT(DATETIME,CONVERT(VARCHAR,GETDATE(),101) + ' ' + SUBSTRING(CONVERT(VARCHAR,GETDATE(),108),1,5))," +
                    "1," +//this is static user id 
                    "'PM'," +
                    "'AUTOMATED AMCP DIRECT PAYMENT OF " + paymentAmount + "'"
                    , environment);



                if (removeMults)
                {
                    var deleteDebtorMultiples = _adoConnection.GetData(
                        "DELETE FROM debtor_multiples " +
                        "WHERE debtor_acct = '" + debtorAccount + "'" +
                        " OR debtor_acct2 = '" + debtorAccount + "'"
                        , environment);

                }


            }
            catch (Exception e)
            {
                return _response.Response(e);
            }


            await Task.CompletedTask;

            return _response.Response(true);
        }


        public async Task<ResponseModel> ProcessCcPayment(ProcessCcPaymentRequestModel request, string environment)
        {
            //todo (if the card already tokenize) 
            //todo
            try
            {
                var tokenizeDataJsonResult = TokenizeCc(request.debtorAcc,request.ccNumber, request.expiredDate, request.hsa != null && (bool)request.hsa, environment).Result;
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



                if (request.amount != null && tokenizeCObj.CardNumber != "" && tokenizeCObj.Key != "")
                {

                    var processTransactionJsonResult = ProcessingTransaction(tokenizeCObj.Key, (decimal)request.amount, request.hsa != null && (bool)request.hsa, environment).Result.Data;


                    //var responseResults = JsonConvert.DeserializeObject<SetProcessCCResponse>(processTransactionJsonResult.ToString() ?? string.Empty);


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
                        orderNumber = "",
                        userId = "WEB",
                    }, environment);

                    if (request.numberOfPayments > 1)
                    {
                        if (request.expiredDate != null)
                        {
                            var spResult = await SchedulePostData(request.debtorAcc, DateTime.Now,
                                (decimal)request.amount, request.ccNumber, (int)request.numberOfPayments,
                                request.expiredDate.Substring(0, 2),
                                request.expiredDate.Substring(2, 2), environment);
                        }
                    }

                    if (processTransactionJsonResult.ToString() != "Error 404")
                    {
                        try
                        {
                            await PostPayment(request.debtorAcc, (decimal)request.amount, DateAndTime.Now, "", Convert.ToDecimal(0), environment);

                            var debtorData = _adoConnection.GetData(
                                "INSERT INTO note_master " +
                                "( debtor_acct," +
                                " note_date," +
                                "employee," +
                                "note_text," +
                                "activity_code)" +
                                //values
                                "VALUES('" + request.debtorAcc + "'," +
                                "GETDATE()," +
                                "0," +
                                "'API PAYMENT " + "Successful" + ": $" + request.amount + " - " + "auth" + "; CC - ' + UPPER('" + tokenizeCObj.Type + "') + ' - XXXX-" + request.ccNumber.Substring(request.ccNumber.Length - 4) + "'," +
                                "'PM')"
                                , environment);

                        }
                        catch (Exception e)
                        {
                            return _response.Response(true, false, e);
                        }
                        return _response.Response(true, true, processTransactionJsonResult);

                    }
                }
            }
            catch (Exception e)
            {
                return _response.Response(true, false, e);
                throw;
            }

            return _response.Response(true, false, "Oops something went wrong");



        }


    }
}
