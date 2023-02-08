using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class ProcessCCResponse : IMultipleExamplesProvider<ProcessCCResponse>
    {
        public bool Status { get; set; }
        public bool TransactionStatus { get; set; }
        public TransactionDetailsForUniversalCc Data { get; set; }

        public IEnumerable<SwaggerExample<ProcessCCResponse>> GetExamples()
        {

            return new SwaggerExample<ProcessCCResponse>[]
            {
                new SwaggerExample<ProcessCCResponse>()
                {
                    Name = "Successful Example",
                    Value = new ProcessCCResponse()
                    {
                        Data = new TransactionDetailsForUniversalCc()
                        {
                            ResponseCode = "000",
                            ResponseMessage = "APPROVAL",
                            TransactionId = "8deb5b1128664d8aa4a9f96f4e9855d5",
                            AuthorizationNumber = "B2BD14"
                        },
                        Status = true,
                        TransactionStatus = true
                    },
                    Summary = "Successful Response"
                }
            };
        }

        public class TransactionDetailsForUniversalCc
        {
            public string TransactionId { get; set; }
            public string ResponseCode { get; set; }
            public string ResponseMessage { get; set; }
            public string AuthorizationNumber { get; set; }

        }
    }
}
