﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AargonTools.Data.ADO;
using AargonTools.Data.ExamplesForDocumentation.Response;
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Swashbuckle.AspNetCore.SwaggerGen;
using USAePay;

namespace AargonTools.Manager
{
    public class ProcessCcPaymentManager : IProcessCcPayment
    {

        private static ExistingDataDbContext _context;
        private static TestEnvironmentDbContext _contextTest;
        private static ResponseModel _response;
        private readonly AdoDotNetConnection _adoConnection;
        private readonly IUserService _userService;

        public ProcessCcPaymentManager(ExistingDataDbContext context, ResponseModel response,
            TestEnvironmentDbContext contextTest, IAddNotes addNotes, AdoDotNetConnection adoConnection, IUserService userService)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _adoConnection = adoConnection;
            _userService = userService;
        }
        //models
        private class SaveCard
        {
            public string Type { get; set; }
            public string Key { get; set; }
            public string CardNumber { get; set; }
        }

        //for tokenize the cc
        private static async Task<ResponseModel> TokenizeCc(string cardNo, string expireDate, string environment)
        {
            if (environment == "P")
            {
                USAePay.API.SetURL("https://sandbox.usaepay.com", "v2");
                USAePay.API.SetAuthentication("_OF5UaX8RaAZ4h8XG4ppDP1VjnvH294Y", "1122333");

                var creditCardObj = new Dictionary<string, object>();
                creditCardObj["number"] = cardNo;
                creditCardObj["expiration"] = expireDate;

                //creditCardObj["number"] = "4929000000006";
                //creditCardObj["expiration"] = "1221";

                var request = new Dictionary<string, object> { ["command"] = "cc:save", ["creditcard"] = creditCardObj };


                var tokenizeObjects = await USAePay.API.Transactions.PostAsync(request);

                string json = JsonConvert.SerializeObject(tokenizeObjects.ElementAt(8).Value, Formatting.Indented);

                return _response.Response(json);
            }
            else
            {
                USAePay.API.SetURL("https://sandbox.usaepay.com", "v2");
                USAePay.API.SetAuthentication("_OF5UaX8RaAZ4h8XG4ppDP1VjnvH294Y", "1122333");

                var creditCardObj = new Dictionary<string, object>();
                creditCardObj["number"] = cardNo;
                creditCardObj["expiration"] = expireDate;

                //creditCardObj["number"] = "4929000000006";
                //creditCardObj["expiration"] = "1221";

                var request = new Dictionary<string, object> { ["command"] = "cc:save", ["creditcard"] = creditCardObj };


                var tokenizeObjects = await USAePay.API.Transactions.PostAsync(request);

                string json = JsonConvert.SerializeObject(tokenizeObjects.ElementAt(8).Value, Formatting.Indented);

                return _response.Response(json);
            }

        }

        private static async Task<ResponseModel> ProcessingTransaction(string tokenizeCc, decimal amount, string environment)
        {
            if (environment == "P")
            {
                USAePay.API.SetURL("https://sandbox.usaepay.com", "v2");
                USAePay.API.SetAuthentication("_OF5UaX8RaAZ4h8XG4ppDP1VjnvH294Y", "1122333");

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

                return _response.Response("Oops something went wrong");
            }
            else
            {
                USAePay.API.SetURL("https://sandbox.usaepay.com", "v2");
                USAePay.API.SetAuthentication("_OF5UaX8RaAZ4h8XG4ppDP1VjnvH294Y", "1122333");

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

                return _response.Response(false);

            }

        }

        //no implementation for now....
        private static async Task<ResponseModel> VoidTransaction(string transactionKey)
        {
            USAePay.API.SetURL("https://sandbox.usaepay.com", "v2");
            USAePay.API.SetAuthentication("_OF5UaX8RaAZ4h8XG4ppDP1VjnvH294Y", "1122333");

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

                                                                  "1," +//_userService.GetLoginUserName() + "," +// no implementation default 1 
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
                                        "date_inactivated = CONVERT(VARCHAR,GETDATE(),101)," +
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
                    1 +//this is static user id 
                    "    'PM'," +
                    "    'AUTOMATED AMCP DIRECT PAYMENT OF " + paymentAmount + "'"
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


        public async Task<ResponseModel> ProcessCcPayment(string debtorAcc, string ccNumber, string expiredDate, string cvv, int numberOfPayments, decimal amount, string environment)
        {
            //todo (if the card already tokenize) 
            //todo

            var tokenizeDataJsonResult = TokenizeCc(ccNumber, expiredDate, environment).Result;
            var tokenizeCObj = JsonConvert.DeserializeObject<SaveCard>(tokenizeDataJsonResult.Data.ToString() ?? string.Empty);
            var processTransactionJsonResult = ProcessingTransaction(tokenizeCObj.Key, amount, environment).Result.Data;
            if (numberOfPayments > 1)
            {
                var spResult = await SchedulePostData(debtorAcc, DateTime.Now, amount, ccNumber, numberOfPayments, expiredDate.Substring(0, 2),
                    expiredDate.Substring(2, 2), environment);
            }

            if (processTransactionJsonResult.ToString() != "Oops something went wrong")
            {
                try
                {
                    await PostPayment(debtorAcc, amount, DateAndTime.Now, "", Convert.ToDecimal(0), environment);
                }
                catch (Exception e)
                {
                    return _response.Response(e);
                }
                return _response.Response(processTransactionJsonResult);

            }

          

            return _response.Response(processTransactionJsonResult);
        }
    }
}