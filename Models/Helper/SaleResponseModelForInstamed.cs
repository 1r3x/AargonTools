using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace AargonTools.Models.Helper
{
    public class SaleResponseModelForInstamed
    {
        public SaleResponseModelForInstamed(string jsonResponse)
        {
            var jObject = JObject.Parse(jsonResponse);
            ResponseCode = (string)jObject["ResponseCode"];
            ResponseMessage = (string)jObject["ResponseMessage"];
            AuthorizationNumber = (string)jObject["AuthorizationNumber"];
            TransactionId = (string)jObject["TransactionID"];
        }
        public string TransactionId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string AuthorizationNumber { get; set; }
    }
}
