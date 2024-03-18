using System.Collections.Generic;
using Swashbuckle.AspNetCore.Filters;
using static AargonTools.Data.ExamplesForDocumentation.Response.ViewingSchedulePaymentsResponse;

namespace AargonTools.Data.ExamplesForDocumentation.Response
{
    public class ViewingSchedulePaymentsResponse : IExamplesProvider<ViewingSchedulePaymentsResponse>
    {
        public bool Status { get; set; }
        public List<SchedulePaymentsObject> Data { get; set; }
        public ViewingSchedulePaymentsResponse GetExamples()
        {
            return new ViewingSchedulePaymentsResponse()
            {
                Status = true,
                Data = new List<SchedulePaymentsObject>()
                {
                    new SchedulePaymentsObject()
                    {
                      id="2",
                      effectiveDate="2023-09-06T10:43:03.453",
                      numberOfPayments=2,
                      patientAccount= "",
                      associateDebtorAcct="1902-000010",
                      cardHolderName= "",
                      expirationMonth= 1,
                      expirationYear= 2
                    },
                    new SchedulePaymentsObject()
                    {
                        id="2",
                        effectiveDate="2023-09-06T10:43:03.453",
                        numberOfPayments=2,
                         patientAccount= "",
                        associateDebtorAcct="1902-000010",
                        cardHolderName= "",
                        expirationMonth=1,
                        expirationYear=2
                    },
                }
            };
        }

        public class SchedulePaymentsObject
        {
            public string id { get; set; }
            public string effectiveDate { get; set; }
            public int numberOfPayments { get; set; }
            public string patientAccount { get; set; }
            public string associateDebtorAcct { get; set; }
            public string cardHolderName { get; set; }
            public int expirationMonth { get; set; }
            public int expirationYear { get; set; }
        }
    }
}
