using System;
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
using AargonTools.Interfaces;
using AargonTools.Manager.GenericManager;
using AargonTools.Models;
using Microsoft.IdentityModel.Tokens;
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

        public ProcessCcPaymentManager(ExistingDataDbContext context, ResponseModel response,
            TestEnvironmentDbContext contextTest, IAddNotes addNotes, AdoDotNetConnection adoConnection)
        {
            _context = context;
            _response = response;
            _contextTest = contextTest;
            _adoConnection = adoConnection;
        }
        //models
        private class SaveCard
        {
            public string Type { get; set; }
            public string Key { get; set; }
            public string CardNumber { get; set; }
        }

        //
        private static async Task<ResponseModel> TokenizeCc(string cardNo,string expireDate,string environment)
        {
            if (environment=="P")
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

        private static async Task<ResponseModel> ProcessingTransaction(string tokenizeCc,decimal amount, string environment)
        {
            if (environment=="P")
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

                return _response.Response(processSaleObjects);
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

                return _response.Response(processSaleObjects);
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



        public async Task<ResponseModel> ProcessCcPayment(string debtorAcc, string ccNumber, string expiredDate, string cvv, int numberOfPayments, decimal amount,string environment)
        {
            //todo (if the card already tokenize) 
            //todo

            var tokenizeDataJsonResult = TokenizeCc(ccNumber,expiredDate,environment).Result;
            var tokenizeCObj = JsonConvert.DeserializeObject<SaveCard>(tokenizeDataJsonResult.Data.ToString() ?? string.Empty);
            var processTransactionJsonResult = ProcessingTransaction(tokenizeCObj.Key, amount,environment).Result.Data;

            if (numberOfPayments>1)
            {
                var spResult =await SchedulePostData(debtorAcc, DateTime.Now, amount, ccNumber, numberOfPayments, expiredDate.Substring(0,2),
                    expiredDate.Substring(2,2), environment);
            }


            return _response.Response(processTransactionJsonResult);
        }
    }
}
