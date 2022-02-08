using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AargonTools.ViewModel;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Request
{
    public class GetClientInvoicePaymentRequestExample : IExamplesProvider<GetClientInvoiceRequestModel>
    {
        public GetClientInvoiceRequestModel GetExamples()
        {
            return new GetClientInvoiceRequestModel()
            {
                Company = "A",
                ClientAccount = "0001",
                StartDate = Convert.ToDateTime("1-1-2000"),
                EndDate = DateTime.Now.Date
            };
        }
    }
}
