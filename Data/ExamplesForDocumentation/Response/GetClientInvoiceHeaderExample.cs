using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetClientInvoiceHeaderExample : IExamplesProvider<GetClientInvoiceHeaderExample>
    {
        public bool Status { get; set; }
        public ClientInvoiceView Data { get; set; }
        public GetClientInvoiceHeaderExample GetExamples()
        {
            return new GetClientInvoiceHeaderExample()
            {
                Status = true,
                Data = new ClientInvoiceView()
                {
                    client_acct = "0001",
                    orig_creditor = "Duane Christy",
                    remit_full_pmt = "N",
                    address12 = "123 ALCOAN ST",
                    address22 = "SUITE B",
                    city2 = "LAS VEGAS",
                    state_code2 = "NV",
                    zip2 = "89115"
                }
            };
        }
        public class ClientInvoiceView
        {
            public string client_acct { get; set; }
            public string orig_creditor { get; set; }
            public string remit_full_pmt { get; set; }
            public string address12 { get; set; }
            public string address22 { get; set; }
            public string city2 { get; set; }
            public string state_code2 { get; set; }
            public string zip2 { get; set; }

        }
    }
}
