using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AargonTools.Models.Helper
{
    public class CommonResponseModelForCCProcess
    {
        public string TransactionId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public string AuthorizationNumber { get; set; }
    }
}
