using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Models.Helper
{
    public class SaleResponseModelForIProGateway
    {
        public SaleResponseModelForIProGateway(string rawString)
        {
            string[] splitedString = rawString.Split("&");

            foreach (var data in splitedString)
            {
                if (data.Contains("response"))
                {
                    var temp = data.Split("=");
                    response = temp[1];
                }

                if (data.Contains("responsetext"))
                {
                    var temp = data.Split("=");
                    responsetext = temp[1];
                }

                if (data.Contains("authcode"))
                {
                    var temp = data.Split("=");
                    authcode = temp[1];
                }

                if (data.Contains("transactionid"))
                {
                    var temp = data.Split("=");
                    transactionid = temp[1];
                }

                if (data.Contains("avsresponse"))
                {
                    var temp = data.Split("=");
                    avsresponse = temp[1];
                }

                if (data.Contains("cvvresponse"))
                {
                    var temp = data.Split("=");
                    avsresponse = temp[1];
                }

                if (data.Contains("orderid"))
                {
                    var temp = data.Split("=");
                    orderid = temp[1];
                }

                if (data.Contains("response_code"))
                {
                    var temp = data.Split("=");
                    response_code = temp[1];
                }

                if (data.Contains("customer_vault_id"))
                {
                    var temp = data.Split("=");
                    customer_vault_id = temp[1];
                }

                if (data.Contains("checkaba"))
                {
                    var temp = data.Split("=");
                    checkaba = temp[1];
                }

                if (data.Contains("checkaccount"))
                {
                    var temp = data.Split("=");
                    checkaccount = temp[1];
                }
            }
        }

        public string response { get; set; }
        public string responsetext { get; set; }
        public string authcode { get; set; }
        public string transactionid { get; set; }
        public string avsresponse { get; set; }
        public string cvvresponse { get; set; }
        public string orderid { get; set; }
        public string response_code { get; set; }
        public string cc_number { get; set; }
        public string customer_vault_id { get; set; }
        public string checkaba { get; set; }
        public string checkaccount { get; set; }
    }
}
