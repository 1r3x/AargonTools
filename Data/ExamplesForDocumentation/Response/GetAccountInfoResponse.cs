using System;
using Swashbuckle.AspNetCore.Filters;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class GetAccountInfoResponse : IExamplesProvider<GetAccountInfoResponse>
    {
        public string debtorAcct { get; set; }
        public string suppliedAcct { get; set; }
        public string acountStatus { get; set; }
        public double balance { get; set; }
        public string mailReturn { get; set; }
        public int employee { get; set; }
        public DateTime dateOfService { get; set; }
        public DateTime datePlaced { get; set; }
        public double lastPaymentAmt { get; set; }
        public double totalPayments { get; set; }
        public DateTime lastPayDate { get; set; }
        public GetAccountInfoResponse GetExamples()
        {
            return new GetAccountInfoResponse()
            {
                debtorAcct = "0001-000001",
                suppliedAcct = "1234",
                acountStatus = "ACTIVE",
                balance = 600,
                mailReturn = "N",
                employee= 390,
                dateOfService= new DateTime(1996, 5,15),
                datePlaced= new DateTime(1996, 6, 15),
                lastPaymentAmt= 3,
                totalPayments= 503,
                lastPayDate= new DateTime(2019, 4, 19)
            };
        }
    }
}
